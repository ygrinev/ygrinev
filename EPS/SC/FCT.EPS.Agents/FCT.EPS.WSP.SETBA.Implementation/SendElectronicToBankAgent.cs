using FCT.EPS.WindowsServiceAgentInterface;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.SETBA.BusinessLogic;
using FCT.EPS.WSP.SETBA.Implementation.Properties;
using FCT.EPS.WSP.SETBA.Resources;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.SETBA.Implementation
{
    public class SendElectronicToBankAgent : MarshalByRefObject, IAgent, IDisposable
    {
        private Timer timer;
        private bool IsConfigFileRead = false;
        private string electronicRequestFileFolderPath = string.Empty;
        private int milliseconds = 0;
        private int maxAllowedRetry = 0;
        private bool _disposed = false;
        private string electronicRequestFileArchivePath = string.Empty;
        private string routingHeader = string.Empty;
        private IList<DateTime> schedules = new List<DateTime>();
        private int timeSpanForFileCreation = 0;
        private string electronicReportFileArchivePath = string.Empty;
        private EmailItems reportEmail = new EmailItems();
        private EmailItems BPSFileEmail = new EmailItems();

        public SendElectronicToBankAgent()
        {
            SolutionTraceClass.appTraceSwitch = new TraceSwitch(AgentConstants.Misc.LOGGING_APPLICATION_TITLE, AgentConstants.Misc.LOGGING_APPLICATION_TITLE + " Notification Agent Defined in config file.");
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
                LoggingHelper.LogErrorActivity("SendElectronicToBankAgent : Exception was thrown.", ex);
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private void GetConfigValues()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (!IsConfigFileRead)
            {
                this.electronicRequestFileFolderPath = Settings.Default.ElectronicRequestFileFolderPath;
                SolutionTraceClass.WriteLineInfo(string.Format("this.ElectronicRequestFileFolderPath = {0}", this.electronicRequestFileFolderPath));

                this.milliseconds = Settings.Default.SubmitElectronicRequestIntervalInMilliSeconds;
                SolutionTraceClass.WriteLineInfo(string.Format("this.milliseconds = {0}", this.milliseconds));

                this.maxAllowedRetry = Settings.Default.MaxAllowedRetry;
                SolutionTraceClass.WriteLineInfo(string.Format("this.maxAllowedRetry = {0}", this.maxAllowedRetry));

                this.electronicRequestFileArchivePath = Settings.Default.ElectronicRequestFileArchivePath;
                SolutionTraceClass.WriteLineInfo(string.Format("this.electronicRequestFileArchivePath = {0}", this.electronicRequestFileArchivePath));

                this.electronicReportFileArchivePath = Settings.Default.ElectronicReportFileArchivePath;
                SolutionTraceClass.WriteLineInfo(string.Format("this.electronicReportFileArchivePath = {0}", this.electronicReportFileArchivePath));

                this.timeSpanForFileCreation = Settings.Default.TimeSpanForFileCreation;
                SolutionTraceClass.WriteLineInfo(string.Format("this.timeSpanForFileCreation = {0}", this.timeSpanForFileCreation));

                this.routingHeader = Settings.Default.RoutingHeader;
                SolutionTraceClass.WriteLineInfo(string.Format("this.fileHeader = {0}", this.routingHeader));

                this.reportEmail.EmailAddress = Settings.Default.ReportEmailAddress;
                SolutionTraceClass.WriteLineInfo(string.Format("this.reportEmail.EmailAddress = {0}", this.reportEmail.EmailAddress));

                this.reportEmail.EmailSubject = Settings.Default.ReportEmailSubject;
                SolutionTraceClass.WriteLineInfo(string.Format("this.reportEmail.EmailSubject = {0}", this.reportEmail.EmailSubject));

                this.reportEmail.EmailBody = Settings.Default.ReportEmailBody;
                SolutionTraceClass.WriteLineInfo(string.Format("this.reportEmail.EmailBody = {0}", this.reportEmail.EmailBody));

                this.BPSFileEmail.EmailAddress = Settings.Default.BPSFileEmailAddress;
                SolutionTraceClass.WriteLineInfo(string.Format("this.BPSFileEmail.EmailAddress = {0}", this.BPSFileEmail.EmailAddress));

                this.BPSFileEmail.EmailSubject = Settings.Default.BPSFileEmailSubject;
                SolutionTraceClass.WriteLineInfo(string.Format("this.BPSFileEmail.EmailSubject = {0}", this.BPSFileEmail.EmailSubject));

                this.BPSFileEmail.EmailBody = Settings.Default.BPSFileEmailBody;
                SolutionTraceClass.WriteLineInfo(string.Format("this.BPSFileEmail.EmailBody = {0}", this.BPSFileEmail.EmailBody));

                
                CollectionConfigSection collectionConfigSection = (CollectionConfigSection)ConfigurationManager.GetSection("CollectionConfigSection");


                ConfigElement element = collectionConfigSection.ConfigElements.First(c => c.Key == "Schedule");
                foreach (var subElement in element.SubElements.AsEnumerable())
                {
                    schedules.Add(Convert.ToDateTime(subElement.Key));
                }

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

                SendElectronicToBankHandler service = new SendElectronicToBankHandler();

                if (service != null)
                {
                    service.CreateElectronicTransactions();
                    foreach(DateTime timeToRun in schedules)
                    {
                        service.CreateElectronicRequest(this.maxAllowedRetry, this.electronicRequestFileArchivePath, this.timeSpanForFileCreation, timeToRun, this.routingHeader);
                    }
                    service.SubmitElectronicFiles(this.maxAllowedRetry, this.electronicRequestFileArchivePath, this.electronicRequestFileFolderPath, this.electronicReportFileArchivePath, this.reportEmail, this.BPSFileEmail);
                }

            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Exception was thrown. ->" + ex.Message);
                LoggingHelper.LogErrorActivity("SendElectronicToBankAgent : Exception was thrown.", ex);
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
                LoggingHelper.LogErrorActivity("SendElectronicToBankAgent : Exception was thrown.", ex);
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
                LoggingHelper.LogErrorActivity("SendElectronicToBankAgent : Exception was thrown.", ex);
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
                LoggingHelper.LogErrorActivity("SendElectronicToBankAgent : Exception was thrown.", ex);
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
                LoggingHelper.LogErrorActivity("SendElectronicToBankAgent : Exception was thrown.", ex);
            }
        }

        public void Stop()
        {

        }

        #endregion
        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

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
