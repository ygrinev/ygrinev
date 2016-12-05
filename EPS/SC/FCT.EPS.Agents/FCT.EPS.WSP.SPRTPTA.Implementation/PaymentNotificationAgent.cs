using System;
using System.Threading;
using System.Diagnostics;
using FCT.EPS.Notification;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.DataAccess;
using FCT.EPS.WSP.SPRTPTA.Resources;
using FCT.EPS.WSP.SPRTPTA.BusinessLogic;
using FCT.EPS.WindowsServiceAgentInterface;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FCT.EPS.WSP.SPRTPTA.Implementation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace FCT.EPS.WSP.SPRTPTA.Implementation
{
    public class PaymentNotificationAgent : MarshalByRefObject, IAgent, IDisposable
    {
        private Timer timer;
        private int milliseconds;
        private int recordsToGet;
        private int maxAllowedRetry;
        private bool IsConfigFileRead = false;
        private string endpoint = string.Empty;
        private string sqlConnection = string.Empty;
        private DateTime lastBatchCreatedAt = DateTime.Now;
        private DateTime timeOfDayToCreateBatch = DateTime.Now;
        private string BindingNameOfPaymentTrackingService = string.Empty;
        PaymentTrackingWebServiceClient _paymentTrackingServiceClient = null;
        private bool _disposed = false;

        public PaymentNotificationAgent()
        {
            SolutionTraceClass.appTraceSwitch = new TraceSwitch(AgentConstants.Misc.LOGGING_APPLICATION_TITLE, "FCT.EPS.WSP.PaymentNotificationAgents Defined in config file.");
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
                IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
                LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
                Logger.SetLogWriter(logWriterFactory.Create());

            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Exception was thrown. ->" + ex.Message);
                LoggingHelper.LogErrorActivity("PaymentNotificationAgent : Exception was thrown.",ex);
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private void GetConfigValues()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (!IsConfigFileRead)
            {
                    this.milliseconds = Settings.Default.SubmitToPaymentTrackerIntervalInMilliSeconds;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.milliseconds = {0}", this.milliseconds));

                    this.recordsToGet = Settings.Default.HowManyPaymentRequestToGet;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.recordsToGet = {0}", this.recordsToGet));

                    this.maxAllowedRetry = Settings.Default.MaxAllowedRetry;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.maxAllowedRetry = {0}", this.maxAllowedRetry));

                    this.BindingNameOfPaymentTrackingService = Settings.Default.BindingNameOfPaymentTrackingService;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.BindingNameOfPaymentTrackingService = {0}", this.BindingNameOfPaymentTrackingService));

                    IsConfigFileRead = true;
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }

        private void TimerTick(object stateInfo)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {
                //Heartbeat
                LoggingHelper.LogAuditingActivity("TimerTick");

                this.OnPause();

                if (_paymentTrackingServiceClient == null)
                {
                    _paymentTrackingServiceClient = new PaymentTrackingWebServiceClient(BindingNameOfPaymentTrackingService);
                    if (_paymentTrackingServiceClient == null) throw new PaymentNotificationAgentException ("Unable to create Payment Tracking WebService Client");
                }


                PaymentNotificationRequestHandler service = new PaymentNotificationRequestHandler(_paymentTrackingServiceClient);

                if (service != null)
                {
                    service.SubmitPaymentRequestToPaymentTracker(this.recordsToGet, this.maxAllowedRetry);
                }

            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Exception was thrown. ->" + ex.Message);
                LoggingHelper.LogErrorActivity(string.Format("PaymentNotificationAgent : Exception was thrown."), ex);
            }
            finally
            {
                this.OnContinue();
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }

        #region FCT.EPS.WindowsServiceAgentInterface.IAgent

        public string ConfigPath { get; set; }

        public bool AutoLog
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool CanHandlePowerEvent
        {
            get
            {
                SolutionTraceClass.WriteLineVerbose("Start");
                return false;
            }
        }

        public bool CanHandleSessionChangeEvent
        {
            get
            {
                SolutionTraceClass.WriteLineVerbose("Start");
                return false;
            }
        }

        public bool CanPauseAndContinue
        {
            get
            {
                SolutionTraceClass.WriteLineVerbose("Start");
                return true;
            }
        }

        public bool CanShutdown
        {
            get
            {
                SolutionTraceClass.WriteLineVerbose("Start");
                return true;
            }
        }

        public bool CanStop
        {
            get
            {
                SolutionTraceClass.WriteLineVerbose("Start");
                return true;
            }
        }

        public string Name
        {
            get
            {
                SolutionTraceClass.WriteLineVerbose("Start");
                return this.GetType().Name;
            }
        }

        public void OnContinue()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {
            GetConfigValues();

            this.OnStop();
            this.timer = new Timer(this.TimerTick, null, this.milliseconds, this.milliseconds);
                        }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Exception was thrown message is -> " + ex.Message);
                LoggingHelper.LogErrorActivity("PaymentNotificationAgent : Exception was thrown.",ex);
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        public void OnCustomCommand(int command)
        {
           
        }

        public void OnPause()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {
                if (this.timer != null)
                    this.timer.Dispose();
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Exception was thrown message is -> " + ex.Message);
                LoggingHelper.LogErrorActivity("PaymentNotificationAgent : Exception was thrown.",ex);
            }
            finally
            {
                this.timer = null;
            }
            SolutionTraceClass.WriteLineVerbose("Start");
        }

        public bool OnPowerEvent(System.ServiceProcess.PowerBroadcastStatus powerStatus)
        {
            throw new NotImplementedException();
        }

        public void OnSessionChange(System.ServiceProcess.SessionChangeDescription changeDescription)
        {
           
        }

        public void OnShutdown()
        {
           
        }

        public void OnStart(string[] args)
        {
            try
            {
            SolutionTraceClass.WriteLineVerbose("Start");
            GetConfigValues();
            OnContinue();
            SolutionTraceClass.WriteLineVerbose("End");
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineWarning("Exception was thrown message is -> " + ex.Message);
                LoggingHelper.LogErrorActivity("PaymentNotificationAgent : Exception was thrown.",ex);
            }
        }

        public void OnStop()
        {
            try
            {
                OnPause();
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineWarning("Exception was thrown message is -> " + ex.Message);
                LoggingHelper.LogErrorActivity("PaymentNotificationAgent : Exception was thrown.",ex);
            }
        }

        public void Stop()
        {
           
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (this._paymentTrackingServiceClient != null)
                    {
                        this._paymentTrackingServiceClient.Dispose();
                    }
                    if(this.timer!=null)
                    {
                        this.timer.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
