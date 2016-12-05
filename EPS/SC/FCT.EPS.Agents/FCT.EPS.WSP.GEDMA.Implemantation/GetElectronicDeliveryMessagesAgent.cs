using FCT.EPS.WindowsServiceAgentInterface;
using FCT.EPS.WSP.GEDMA.BusinessLogic;
using FCT.EPS.WSP.GEDMA.Resources;
using FCT.EPS.WSP.Resources;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.GEDMA.Implemantation
{
    public class GetElectronicDeliveryMessagesAgent : MarshalByRefObject, IAgent, IDisposable
    {
        private Timer timer;
        private bool _disposed = false;
        private bool IsConfigFileRead = false;
        private int milliseconds;
        private string pathToElectronicRejectedTransactionsFiles;
        private string pathToElectronicRejectedTransactionsFilesArchive;
        private string electronicReportFileArchivePath;
        private EmailItems reportEmail = new EmailItems();

        public GetElectronicDeliveryMessagesAgent()
        {
            SolutionTraceClass.appTraceSwitch = new TraceSwitch(AgentConstants.Misc.LOGGING_APPLICATION_TITLE, AgentConstants.Misc.LOGGING_APPLICATION_TITLE + " Defined in config file.");
            SolutionTraceClass.WriteLineVerbose("Start");

            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create());

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private void GetConfigValues()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (!IsConfigFileRead)
            {
                try
                {
                    this.milliseconds = Properties.Settings.Default.GetElectronicMessagesIntervalInMilliSeconds;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.milliseconds = {0}", this.milliseconds));

                    this.pathToElectronicRejectedTransactionsFiles = Properties.Settings.Default.pathToElectronicRejectedTransactionsFiles;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.pathToElectronicRejectedTransactionsFiles = {0}", this.pathToElectronicRejectedTransactionsFiles));

                    this.pathToElectronicRejectedTransactionsFilesArchive = Properties.Settings.Default.pathToElectronicRejectedTransactionsFilesArchive;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.pathToElectronicRejectedTransactionsFilesArchive = {0}", this.pathToElectronicRejectedTransactionsFilesArchive));

                    this.electronicReportFileArchivePath = Properties.Settings.Default.ElectronicReportFileArchivePath;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.electronicReportFileArchivePath = {0}", this.electronicReportFileArchivePath));

                    this.reportEmail.EmailAddress = Properties.Settings.Default.ReportEmailAddress;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.reportEmail.EmailAddress = {0}", this.reportEmail.EmailAddress));

                    this.reportEmail.EmailSubject = Properties.Settings.Default.ReportEmailSubject;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.reportEmail.EmailSubject = {0}", this.reportEmail.EmailSubject));

                    this.reportEmail.EmailBody = Properties.Settings.Default.ReportEmailBody;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.reportEmail.EmailBody = {0}", this.reportEmail.EmailBody));
                }
                catch (Exception ex)
                {
                    SolutionTraceClass.WriteLineError("Exception was thrown. ->" + ex.Message);
                    LoggingHelper.LogErrorActivity(ex);
                    throw;
                }
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

                BusinessLayer.ProcessElectronicDeliveryRejectedTransactionsFiles(this.pathToElectronicRejectedTransactionsFiles, this.pathToElectronicRejectedTransactionsFilesArchive, this.reportEmail, this.electronicReportFileArchivePath);
                //BusinessLayer.ProcessElectronicDeliveryStatus(this.pathToElectronicRejectedTransactionsFiles);

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
            GetConfigValues();
            this.timer = null;

            this.OnStop();
            this.timer = new Timer(this.TimerTick, null, this.milliseconds, this.milliseconds);

            SolutionTraceClass.WriteLineVerbose("End");
        }

        public void OnCustomCommand(int command)
        {
            throw new NotImplementedException();
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
                throw;
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
            throw new NotImplementedException();
        }

        public void OnShutdown()
        {
            throw new NotImplementedException();
        }

        public void OnStart(string[] args)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            GetConfigValues();
            OnContinue();
            SolutionTraceClass.WriteLineVerbose("End");
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
                LoggingHelper.LogWarningActivity(ex);
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
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
