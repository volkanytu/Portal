using Microsoft.Xrm.Sdk;
using GK.Library.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GK.WindowsServices.SendNpsSurvey
{
    public partial class SendNpsSurvey : ServiceBase
    {
        private System.Timers.Timer _timer;
        private Task _task = null;
        private SqlDataAccess _sda = null;
        private IOrganizationService _service = null;
        private ServiceProcess _serviceProcess;

        private string LOG_PATH;
        private string ERROR_LOG_PATH;

        private CancellationTokenSource _tokenSource = null;
        private CancellationToken? _token = null;

        public SendNpsSurvey()
        {
            InitializeComponent();

            try
            {
                LOG_PATH = Globals.FileLogPath;
                ERROR_LOG_PATH = Globals.FileLogPath;

                _tokenSource = new CancellationTokenSource();
                _token = _tokenSource.Token;

                _timer = new System.Timers.Timer();
                _timer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["serviceInterval"]);
                _timer.AutoReset = true;
                _timer.Enabled = true;

                _timer.Elapsed += timer_Elapsed;

                _sda = new SqlDataAccess();
                _sda.openConnection(Globals.ConnectionString);

                _service = MSCRM.GetOrgService(true);

                _serviceProcess = new ServiceProcess(_sda, _service);
            }
            catch (Exception ex)
            {
                FileLogHelper.LogFunction(this.GetType().Name, "SendNpsSurvey_SendNpsSurvey_EXCEPTION:" + ex.Message, ERROR_LOG_PATH);
            }
        }

        internal void DebugService(string[] args)
        {
            StartOperations();
        }

        protected override void OnStart(string[] args)
        {
            FileLogHelper.LogFunction(this.GetType().Name, "SendNpsSurvey_OnStart_SERVICE_STARTED", LOG_PATH);

            StartOperations();
        }

        protected override void OnStop()
        {
            FileLogHelper.LogFunction(this.GetType().Name, "SendNpsSurvey_OnStop_SERVICE_STOPPED", LOG_PATH);

            StopOperations();
        }

        private void StartOperations()
        {
            _timer.Start();
        }

        private void StopOperations()
        {
            _tokenSource.Cancel();

            if (_sda != null)
            {
                _sda.closeConnection();
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();
                _timer.Enabled = false;

                _task = Task.Factory.StartNew(_ =>
                {
                    _serviceProcess.Process(_sda);
                }
                , _token);

                _task.Wait();

                _timer.Enabled = true;
                _timer.Start();
            }
            catch (Exception ex)
            {
                FileLogHelper.LogFunction(this.GetType().Name, "SendNpsSurvey_timer_Elapsed_EXCEPTION:" + ex.Message, ERROR_LOG_PATH);
            }
        }
    }
}
