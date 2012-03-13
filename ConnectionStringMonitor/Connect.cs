using System;
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
            if (ExecuteOption == vsCommandExecOption.vsCommandExecOptionDoDefault
                && CmdName == _command.Name)
            {
                if (_monitor != null)
                    _monitor.OpenFile();
                Handled = true;
            }
        }

        public void QueryStatus(
            string CmdName,
            vsCommandStatusTextWanted NeededText,
            ref vsCommandStatus StatusOption,
            ref object CommandText)
        {
            if (CmdName == _command.Name)
            {
                CommandText = _monitor.ConnectionString;
                StatusOption = vsCommandStatus.vsCommandStatusEnabled
                    | vsCommandStatus.vsCommandStatusSupported;
            }
        }

        #endregion

        private string FullCommandName
        {
            get
            {
                return string.Format("{0}.{1}",
                    _addInInstance.ProgID, _commandName);
            }
        }

        private void Initialize()
        {
            if (_isInitialized)
                return;
            
            RegisterCommands();
            _monitor = new WebConfigMonitor(_applicationObject);

            _isInitialized = true;
        }

        private void RegisterCommands()
        {
            object[] contextUIGuids = { };

            try
            {
                _command = _applicationObject.Commands.Item(FullCommandName);
            }
            catch
            {
            }

            if (_command == null)
            {
                _command = _applicationObject.Commands.AddNamedCommand(
                    _addInInstance,
                    _commandName,
                    "Connection string",
                    "Active connection string",
                    false,
                    58,
                    contextUIGuids,
                    (int)(vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled));
            }
        }

        private AddIn _addInInstance;
        private DTE2 _applicationObject;
        private Command _command;
        private const string _commandName = "ConnectionStringMonitor";
        private bool _isInitialized;
        private WebConfigMonitor _monitor;
    }
}