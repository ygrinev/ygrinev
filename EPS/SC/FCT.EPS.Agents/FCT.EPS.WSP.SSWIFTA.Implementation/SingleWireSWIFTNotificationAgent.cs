using System;
using System.Threading;
using System.Diagnostics;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.SSWIFTA.Resources;
using FCT.EPS.WSP.SSWIFTA.BusinessLogic;
using FCT.EPS.WindowsServiceAgentInterface;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FCT.EPS.WSP.SSWIFTA.Implementation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace FCT.EPS.WSP.SSWIFTA.Implementation
{
    public class SingleWireSWIFTNotificationAgent : MarshalByRefObject, IAgent, IDisposable
    {
        private Timer timer;
        private int milliseconds;
        private int recordsToGet;
        private int maxAllowedRetry;
        private bool IsConfigFileRead = false;
        private string endpoint = string.Empty;
        private string sqlConnection = string.Empty;
        private string SingleWireFileFolderPath = string.Empty;
        private DateTime lastBatchCreatedAt = DateTime.Now;
        private DateTime timeOfDayToCreateBatch = DateTime.Now;
        private bool _disposed = false;

        public SingleWireSWIFTNotificationAgent()
        {
            SolutionTraceClass.appTraceSwitch = new TraceSwitch(AgentConstants.Misc.LOGGING_APPLICATION_TITLE, "FCT.EPS.WSP.SSWIFT Notification Agent Defined in config file.");
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
                LoggingHelper.LogErrorActivity("SingleWireSWIFTNotificationAgent : Exception was thrown.", ex);
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private void GetConfigValues()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (!IsConfigFileRead)
            {
                    this.SingleWireFileFolderPath = Settings.Default.SingleWireFileFolderPath;

                    this.milliseconds = Settings.Default.SubmitToSWIFTIntervalInMilliSeconds;

                    SolutionTraceClass.WriteLineInfo(string.Format("this.milliseconds = {0}", this.milliseconds));

                    this.recordsToGet = Settings.Default.HowManyPaymentRequestToGet;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.recordsToGet = {0}", this.recordsToGet));

                    this.maxAllowedRetry = Settings.Default.MaxAllowedRetry;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.maxAllowedRetry = {0}", this.maxAllowedRetry));

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

                SingleWireSWIFTNotificationRequestHandler service = new SingleWireSWIFTNotificationRequestHandler();

                if (service != null)
                {
                    service.CreateSingleWireSWIFTBatch();
                    service.SubmitWireRequestToSWIFT(this.recordsToGet, this.maxAllowedRetry, SingleWireFileFolderPath);
                }

            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Exception was thrown. ->" + ex.Message);
                LoggingHelper.LogErrorActivity("SingleWireSWIFTNotificationAgent : Exception was thrown.", ex);
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
                LoggingHelper.LogErrorActivity("SingleWireSWIFTNotificationAgent : Exception was thrown.", ex); 
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
                LoggingHelper.LogErrorActivity("SingleWireSWIFTNotificationAgent : Exception was thrown.", ex);
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
                LoggingHelper.LogErrorActivity("SingleWireSWIFTNotificationAgent : Exception was thrown.", ex);
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
                LoggingHelper.LogErrorActivity("SingleWireSWIFTNotificationAgent : Exception was thrown.", ex);
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
