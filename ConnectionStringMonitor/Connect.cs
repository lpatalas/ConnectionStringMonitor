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
using System.Linq;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;

namespace ConnectionStringMonitor
{
	public class Connect : IDTExtensibility2, IDTCommandTarget
	{
		public Connect()
		{
		}

		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;

			if (connectMode == ext_ConnectMode.ext_cm_AfterStartup)
			{
				Initialize();
			}
		}

		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
			if (disconnectMode == ext_DisconnectMode.ext_dm_HostShutdown
				|| disconnectMode == ext_DisconnectMode.ext_dm_UserClosed)
			{
				if (_settingsCommandButton != null)
				{
					_settingsCommandButton.Delete();
					_settingsCommandButton = null;
				}

				if (_settingsCommand != null)
				{
					_settingsCommand.Delete();
					_settingsCommand = null;
				}
			}
		}

		public void OnAddInsUpdate(ref Array custom)
		{
		}

		public void OnStartupComplete(ref Array custom)
		{
			Initialize();
		}

		public void OnBeginShutdown(ref Array custom)
		{
		}

		#region IDTCommandTarget Members

		public void Exec(
			string CmdName,
			vsCommandExecOption ExecuteOption,
			ref object VariantIn,
			ref object VariantOut,
			ref bool Handled)
		{
			if (ExecuteOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
				if (CmdName == _statusCommand.Name)
				{
					if (_monitor != null)
						_monitor.OpenFile();
					Handled = true;
				}
				else if (CmdName == _settingsCommand.Name)
				{
					var settingsForm = new SettingsForm();
					settingsForm.ShowDialog();
					Handled = true;
				}
			}
		}

		public void QueryStatus(
			string CmdName,
			vsCommandStatusTextWanted NeededText,
			ref vsCommandStatus StatusOption,
			ref object CommandText)
		{
			if (CmdName == _statusCommand.Name)
			{
				CommandText = _monitor.ConnectionString;
				StatusOption = vsCommandStatus.vsCommandStatusEnabled
					| vsCommandStatus.vsCommandStatusSupported;
			}
			else if (CmdName == _settingsCommand.Name)
			{
				CommandText = "Connection String Monitor Settings";
				StatusOption = vsCommandStatus.vsCommandStatusEnabled
					| vsCommandStatus.vsCommandStatusSupported;
			}
		}

		#endregion

		private void AddSettingsMenuItem()
		{
			var toolMenuBar = GetMenuCommandBar("Tools");
			_settingsCommandButton = (CommandBarButton)_settingsCommand.AddControl(toolMenuBar, 1);
			_settingsCommandButton.Caption = "Connection String Monitor Settings";
		}

		private Command CreateSettingsCommand()
		{
			var command = GetExistingCommand(_settingsCommandName);

			if (command == null)
			{
				command = _applicationObject.Commands.AddNamedCommand(
					_addInInstance,
					_settingsCommandName,
					"Connection String Monitor Settings",
					"Connection String Monitor Settings",
					false,
					58,
					_contextUIGuids,
					(int)(vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled));
			}

			return command;
		}

		private Command CreateStatusCommand()
		{
			var command = GetExistingCommand(_statusCommandName);

			if (command == null)
			{
				command = _applicationObject.Commands.AddNamedCommand(
					_addInInstance,
					_statusCommandName,
					"Connection string",
					"Active connection string",
					false,
					58,
					_contextUIGuids,
					(int)(vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled));
			}

			return command;
		}

		private Command GetExistingCommand(string commandName)
		{
			try
			{
				var fullName = _addInInstance.ProgID + '.' + commandName;
				return _applicationObject.Commands.Item(fullName);
			}
			catch
			{
				return null;
			}
		}

		private CommandBar GetMenuCommandBar(string menuName)
		{
			var menuBar = GetStandardMenuBar();
			return (from CommandBarControl control in menuBar.Controls
					where control.Type == MsoControlType.msoControlPopup
					let popup = (CommandBarPopup)control
					where popup.CommandBar.Name.Equals(menuName, StringComparison.Ordinal)
					select popup.CommandBar)
					.First();
		}

		private CommandBar GetStandardMenuBar()
		{
			return ((CommandBars)_applicationObject.CommandBars)["MenuBar"];
		}

		private void Initialize()
		{
			if (_isInitialized)
				return;

			_monitor = new WebConfigMonitor(_applicationObject);
			RegisterCommands();

			_isInitialized = true;
		}

		private void RegisterCommands()
		{
			_settingsCommand = CreateSettingsCommand();
			_statusCommand = CreateStatusCommand();
			AddSettingsMenuItem();
		}

		private AddIn _addInInstance;
		private DTE2 _applicationObject;
		private readonly object[] _contextUIGuids = { };
		private bool _isInitialized;
		private WebConfigMonitor _monitor;
		private Command _settingsCommand;
		private CommandBarButton _settingsCommandButton;
		private const string _settingsCommandName = "ConnectionStringMonitorSettings";
		private Command _statusCommand;
		private const string _statusCommandName = "ConnectionStringMonitorStatus";
	}
}