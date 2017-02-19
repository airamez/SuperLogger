using SuperLogger.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperLogger.Service
{
    public partial class SuperLoggerService : ServiceBase
    {
        private static string SOURCE_NAME = "SuperLogger";
        private static string LOG_NAME = "";
        private Thread _serviceThread;
        private SuperLoggerServiceModel _superLoggerServiceModel;

        public SuperLoggerService()
        {
            InitializeComponent();
            if (!EventLog.SourceExists(SOURCE_NAME))
            {
                EventLog.CreateEventSource(SOURCE_NAME, LOG_NAME);
            }
            this.SuperLoggerEventLog.Source = SOURCE_NAME;
            this.SuperLoggerEventLog.Log = LOG_NAME;
        }

        protected override void OnStart(string[] args)
        {
            InitializeServiceModel();

            StartServiceThread();
        }

        private void StartServiceThread()
        {
            _serviceThread = new Thread(_superLoggerServiceModel.Run);
            _serviceThread.Name = "SuperLogger Service Thread";
            _serviceThread.IsBackground = true;
            _serviceThread.Start();
        }

        protected override void OnStop()
        {
            _superLoggerServiceModel.Stop();
        }

        private void InitializeServiceModel()
        {
            try
            {
                _superLoggerServiceModel = new SuperLoggerServiceModel();
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error initializing the service Model: Error = {0}; StackTrace = {1}", ex.Message, ex.StackTrace);
                this.SuperLoggerEventLog.WriteEntry(msg, EventLogEntryType.Error);
            }
        }
    }
}
