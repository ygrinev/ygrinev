using System;
using System.Threading;
using System.Diagnostics;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.DataAccess;
using FCT.EPS.WSP.SRA.Resources;
using FCT.EPS.WSP.SRA.BusinessLogic;
using FCT.EPS.WindowsServiceAgentInterface;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FCT.EPS.WSP.SRA.Implementation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace FCT.EPS.WSP.SRA.Implementation
{
    public class SendReportAgent : MarshalByRefObject, IAgent, IDisposable
    {
        private Timer timer;
        private int milliseconds;
        private int howManyPaymentTransactionRecordsToGet;
        private int maxAllowedRetry;
        private bool IsConfigFileRead = false;
        private string endpoint = string.Empty;
        private string sqlConnection = string.Empty;
        private bool _disposed = false;
        private ReportGenerator ReportGeneratorObject;
        private int HowManyRowBetweenHeaderAndData;
        private string FileArchiveLocation;
        private string DMSWrkDirLocation;

        public SendReportAgent()
        {
            SolutionTraceClass.appTraceSwitch = new TraceSwitch(AgentConstants.Misc.LOGGING_APPLICATION_TITLE, "SendReportAgent Defined in config file.");
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
                LoggingHelper.LogErrorActivity("SendReportAgent : Exception was thrown.", ex);
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private void GetConfigValues()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (!IsConfigFileRead)
            {
                this.milliseconds = Settings.Default.AgentPollingIntervalInMilliseconds;
                SolutionTraceClass.WriteLineInfo(string.Format("this.milliseconds = {0}", this.milliseconds));

                this.howManyPaymentTransactionRecordsToGet = Settings.Default.HowManyPaymentTransactionRecordsToGet;
                SolutionTraceClass.WriteLineInfo(string.Format("this.recordsToGet = {0}", this.howManyPaymentTransactionRecordsToGet));

                this.maxAllowedRetry = Settings.Default.MaxAllowedRetry;
                SolutionTraceClass.WriteLineInfo(string.Format("this.maxAllowedRetry = {0}", this.maxAllowedRetry));

                this.HowManyRowBetweenHeaderAndData = Settings.Default.HowManyRowBetweenHeaderAndData;
                SolutionTraceClass.WriteLineInfo(string.Format("this.HowManyRowBetweenHeaderAndData = {0}", this.HowManyRowBetweenHeaderAndData));

                this.FileArchiveLocation = Settings.Default.FileArchiveLocation;
                SolutionTraceClass.WriteLineInfo(string.Format("this.FileArchiveLocation = {0}", this.FileArchiveLocation));

                this.DMSWrkDirLocation = Settings.Default.DMSWrkDirLocation;
                SolutionTraceClass.WriteLineInfo(string.Format("this.DMSWrkDirLocation = {0}", this.DMSWrkDirLocation));
                
                
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

                if (ReportGeneratorObject == null)
                {
                    ReportGeneratorObject = new ReportGenerator(HowManyRowBetweenHeaderAndData, FileArchiveLocation, DMSWrkDirLocation);
                    if (ReportGeneratorObject == null) throw new Exception("Unable to create Payment Tracking WebService Client");
                }


                ReportGeneratorObject.GenerateReports(this.howManyPaymentTransactionRecordsToGet, this.maxAllowedRetry);

            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Exception was thrown. ->" + ex.Message);
                LoggingHelper.LogErrorActivity(string.Format("SendReportAgent : Exception was thrown."), ex);
            }
            finally
            {
                ReportGeneratorObject.Dispose();
                ReportGeneratorObject = null;
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
                LoggingHelper.LogErrorActivity("SendReportAgent : Exception was thrown.", ex);
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
                LoggingHelper.LogErrorActivity("SendReportAgent : Exception was thrown.", ex);
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
                OnContinue();
                SolutionTraceClass.WriteLineVerbose("End");
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineWarning("Exception was thrown message is -> " + ex.Message);
                LoggingHelper.LogErrorActivity("SendReportAgent : Exception was thrown.", ex);
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
                LoggingHelper.LogErrorActivity("SendReportAgent : Exception was thrown.", ex);
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
                    if (this.ReportGeneratorObject != null)
                    {
                        this.ReportGeneratorObject.Dispose();
                    }
                    if (this.timer != null)
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
