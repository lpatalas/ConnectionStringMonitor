// Copyright (c) 2012 Łukasz Patalas
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies
// or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
// AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace ConnectionStringMonitor
{
	internal sealed class WebConfigMonitor : IDisposable, IVsSolutionEvents
	{
		public string ConnectionString
		{
			get;
			private set;
		}

		public WebConfigMonitor(DTE2 dte)
		{
			_dte = dte;

			var vsSolution = GetSolutionService();
			if (vsSolution != null)
				vsSolution.AdviseSolutionEvents(this, out _cookie);

			HookSolution();

			Settings.Default.PropertyChanged += Settings_PropertyChanged;
		}

		public void Dispose()
		{
			UnhookSolution();

			if (_dte != null)
			{
				var vsSolution = GetSolutionService();
				if (vsSolution != null)
					vsSolution.UnadviseSolutionEvents(_cookie);

				_dte = null;
			}
		}

		public void OpenFile()
		{
			if (_webConfigItem != null)
			{
				var window = _webConfigItem.Open(EnvDTE.Constants.vsViewKindPrimary);
				window.Activate();
			}
		}

		private IVsSolution GetSolutionService()
		{
			var serviceProvider = new ServiceProvider(
				_dte as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
			if (serviceProvider != null)
			{
				return serviceProvider.GetService(typeof(SVsSolution)) as IVsSolution;
			}

			return null;
		}

		private void HookSolution()
		{
			UnhookSolution();

			var solution = _dte.Solution;
			if ((solution == null) || !solution.IsOpen)
				return;

			var startupProjects = (Array)solution.SolutionBuild.StartupProjects;
			if (startupProjects != null && startupProjects.Length > 0)
			{
				var startupProjectName = (string)startupProjects.GetValue(0);

				var startupProject = (from Project project in solution.Projects
									  where project.UniqueName.Equals(startupProjectName, StringComparison.OrdinalIgnoreCase)
									  select project)
									  .FirstOrDefault();

				if (startupProject != null)
				{
					_webConfigItem = (from ProjectItem item in startupProject.ProjectItems
									  where item.Name.Equals("web.config", StringComparison.OrdinalIgnoreCase)
									  select item)
									  .FirstOrDefault();

					if (_webConfigItem != null)
					{
						var fileName = _webConfigItem.get_FileNames(1);
						var path = Path.GetDirectoryName(fileName);
						var name = Path.GetFileName(fileName);

						_fileSystemWatcher = new FileSystemWatcher(path, name);
						_fileSystemWatcher.Changed += (sender, e) =>
						{
							if (e.ChangeType == WatcherChangeTypes.Changed)
							{
								RefreshConnectionString();
							}
						};
						_fileSystemWatcher.EnableRaisingEvents = true;

						RefreshConnectionString();
					}
				}
			}
		}

		private void RefreshConnectionString()
		{
			if (_webConfigItem == null)
				return;

			var fileName = _webConfigItem.get_FileNames(1);
			if (!File.Exists(fileName))
				return;

			var config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(
				new System.Configuration.ExeConfigurationFileMap { ExeConfigFilename = fileName },
				System.Configuration.ConfigurationUserLevel.None);

			var connectionStringName = Settings.Default.ConnectionStringName;
			var connectionString = (from ConnectionStringSettings item in config.ConnectionStrings.ConnectionStrings
									where item.Name.Equals(connectionStringName, StringComparison.Ordinal)
									select item.ConnectionString)
									.FirstOrDefault();

			this.ConnectionString = ConnectionStringFormatter.Format(connectionString);
		}

		private void UnhookSolution()
		{
			if (_fileSystemWatcher != null)
			{
				_fileSystemWatcher.Dispose();
				_fileSystemWatcher = null;
			}

			_webConfigItem = null;
		}

		private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			HookSolution();
		}

		private uint _cookie;
		private DTE2 _dte;
		private FileSystemWatcher _fileSystemWatcher;
		private ProjectItem _webConfigItem;

		#region IVsSolutionEvents Members

		int IVsSolutionEvents.OnAfterCloseSolution(object pUnkReserved)
		{
			UnhookSolution();
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
		{
			HookSolution();
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnBeforeCloseSolution(object pUnkReserved)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
		{
			return VSConstants.S_OK;
		}

		#endregion
	}
}
