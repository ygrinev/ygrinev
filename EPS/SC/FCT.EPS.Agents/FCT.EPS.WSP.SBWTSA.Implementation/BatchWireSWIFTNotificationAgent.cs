using System;
using System.Threading;
using System.Diagnostics;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.SBWTSA.Resources;
using FCT.EPS.WSP.SBWTSA.BusinessLogic;
using FCT.EPS.WindowsServiceAgentInterface;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace FCT.EPS.WSP.SBWTSA.Implementation
{
    public class BatchWireSWIFTNotificationAgent : MarshalByRefObject, IAgent, IDisposable
    {
        private Timer timer;
        private int timerTickInMilliseconds;
        private int recordsToGet;
        private int maxAllowedRetry;
        private bool IsConfigFileRead = false;
        private string sqlConnection = string.Empty;
        private string DestinationFileFolderPath = string.Empty;
        private DateTime lastBatchCreatedAt = DateTime.Now;
        private DateTime timeOfDayToCreateBatch = DateTime.Now;
        private bool _disposed = false;
        private int scheduleTimeSpanInSeconds = 300;

        public BatchWireSWIFTNotificationAgent()
        {
            SolutionTraceClass.appTraceSwitch = new TraceSwitch(AgentConstants.Misc.LOGGING_APPLICATION_TITLE, "FCT.EPS.WSP.SBWTSA Notification Agent Defined in config file.");
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
                LoggingHelper.LogErrorActivity(ex);
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private void GetConfigValues()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (!IsConfigFileRead)
            {
                    this.DestinationFileFolderPath = Properties.Settings.Default.DestinationFileFolderPath;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.DestinationFileFolderPath = '{0}'", this.DestinationFileFolderPath));

                    this.timerTickInMilliseconds = Properties.Settings.Default.SendSwiftBatchAgentIntervalInMilliSeconds;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.milliseconds = '{0}'", this.timerTickInMilliseconds));

                    this.recordsToGet = Properties.Settings.Default.HowManyRecordsToGet;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.recordsToGet = '{0}'", this.recordsToGet));

                    this.maxAllowedRetry = Properties.Settings.Default.MaxAllowedRetry;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.maxAllowedRetry = '{0}'", this.maxAllowedRetry));

                    this.scheduleTimeSpanInSeconds = Properties.Settings.Default.ScheduleTimeSpanInSeconds;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.scheduleTimeSpanInSeconds = '{0}'", this.scheduleTimeSpanInSeconds));

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

                BatchWireSWIFTNotificationRequestHandler.CreateWireSWIFTBatch(scheduleTimeSpanInSeconds);

                BatchWireSWIFTNotificationRequestHandler.SubmitWireRequestToSWIFT(this.recordsToGet, this.maxAllowedRetry, this.DestinationFileFolderPath);

            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Exception was thrown. ->" + ex.Message);
                LoggingHelper.LogErrorActivity(ex);
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
            this.timer = new Timer(this.TimerTick, null, this.timerTickInMilliseconds, this.timerTickInMilliseconds);
                        }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Exception was thrown message is -> " + ex.Message);
                LoggingHelper.LogErrorActivity(ex);
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
                LoggingHelper.LogErrorActivity(ex);
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
                LoggingHelper.LogErrorActivity(ex);
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
                LoggingHelper.LogErrorActivity(ex);
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
