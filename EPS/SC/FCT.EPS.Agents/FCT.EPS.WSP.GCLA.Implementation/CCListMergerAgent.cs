using FCT.EPS.FileSerializer.RBC;
using FCT.EPS.WindowsServiceAgentInterface;
using FCT.EPS.WSP.GCLA.BusinessLogic;
using FCT.EPS.WSP.GCLA.Resources;
using FCT.EPS.WSP.Resources;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Transactions;

namespace FCT.EPS.WSP.GCLA.Implementation
{
    public class CCListMergerAgent : MarshalByRefObject, IAgent, IDisposable
    {
        private Timer timer;
        private bool _disposed = false;
        private int milliseconds;
        private int dateShift;
        private int maxRetryNumber;
        private int retryInterval;
        private TimeSpan pollStart;
        private string pathToCCListFile;
        private string pathToArcCCListFile;
        private string mailTo;
        private string mailSubjectNew;
        private string mailSubjectErr;
        private string mailSubjectDel;
        private string mailBodyNew;
        private string mailBodyErr;
        private string mailBodyDel;
        private bool inTestMode;
        private string mailDateFormat;
        private string pathToReport;
        public CCListMergerAgent()
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
            try
            {
                this.dateShift = Properties.Settings.Default.dateShift;
                SolutionTraceClass.WriteLineInfo(string.Format("this.dateShift = {0}", this.dateShift));
                this.pollStart = Properties.Settings.Default.pollStart;
                SolutionTraceClass.WriteLineInfo(string.Format("this.pollStart = {0}", this.pollStart));
                this.maxRetryNumber = Properties.Settings.Default.maxRetryNumber;
                SolutionTraceClass.WriteLineInfo(string.Format("this.maxRetryNumber = {0}", this.maxRetryNumber));
                this.retryInterval = Properties.Settings.Default.retryInterval;
                SolutionTraceClass.WriteLineInfo(string.Format("this.retryInterval = {0}", this.retryInterval));
                this.milliseconds = Properties.Settings.Default.GetElectronicPollingIntervalInMilliSeconds;
                SolutionTraceClass.WriteLineInfo(string.Format("this.milliseconds = {0}", this.milliseconds));

                this.pathToCCListFile = Properties.Settings.Default.pathToCCListFile;
                SolutionTraceClass.WriteLineInfo(string.Format("this.pathToCCListFile = {0}", this.pathToCCListFile));
                this.pathToArcCCListFile = Properties.Settings.Default.pathToArcCCListFile;
                SolutionTraceClass.WriteLineInfo(string.Format("this.pathToArcCCListFile = {0}", this.pathToArcCCListFile));
                this.mailTo = Properties.Settings.Default.mailTo;
                SolutionTraceClass.WriteLineInfo(string.Format("this.mailTo = {0}", this.mailTo));
                this.mailSubjectNew = Properties.Settings.Default.mailSubjectNew;
                SolutionTraceClass.WriteLineInfo(string.Format("this.mailSubjectNew = {0}", this.mailSubjectNew));
                this.mailSubjectErr = Properties.Settings.Default.mailSubjectErr;
                SolutionTraceClass.WriteLineInfo(string.Format("this.mailSubjectErr = {0}", this.mailSubjectErr));
                this.mailSubjectDel = Properties.Settings.Default.mailSubjectDel;
                SolutionTraceClass.WriteLineInfo(string.Format("this.mailSubjectDel = {0}", this.mailSubjectDel));
                this.mailBodyNew = Properties.Settings.Default.mailBodyNew;
                SolutionTraceClass.WriteLineInfo(string.Format("this.mailBodyNew = {0}", this.mailBodyNew));
                this.mailBodyErr = Properties.Settings.Default.mailBodyErr;
                SolutionTraceClass.WriteLineInfo(string.Format("this.mailBodyErr = {0}", this.mailBodyErr));
                this.mailBodyDel = Properties.Settings.Default.mailBodyDel;
                SolutionTraceClass.WriteLineInfo(string.Format("this.mailBodyDel = {0}", this.mailBodyDel));
                this.mailDateFormat = Properties.Settings.Default.mailDateFormat;
                SolutionTraceClass.WriteLineInfo(string.Format("this.mailDateFormat = {0}", this.mailDateFormat));
                this.pathToReport = Properties.Settings.Default.pathToReport;
                SolutionTraceClass.WriteLineInfo(string.Format("this.pathToReport = {0}", this.pathToReport));
                
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Exception was thrown. ->" + ex.Message);
                LoggingHelper.LogErrorActivity(ex);
                throw;
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

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    if (!BusinessLayer.LockProcess(this.pollStart)) // in process or the process was completed this date
                        return;

                    BusinessLayer oBus = new BusinessLayer();
                    using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        //if (BusinessLayer.isCCListFileRead || DateTime.Now.TimeOfDay < this.pollStart)
                        //{
                        //    if (BusinessLayer.isCCListFileRead && DateTime.Now.TimeOfDay < this.pollStart)
                        //        BusinessLayer.isCCListFileRead = false; // 
                        //    SolutionTraceClass.WriteLineVerbose("Skipped Process End");
                        //    return;
                        //}

                        
                        inTestMode = oBus.isTestMode;
                        // Check if check mode is enabled
                        for (int i = 0; i < maxRetryNumber; i++)
                        {
                            if (oBus.MoveFile(pathToCCListFile, pathToArcCCListFile)
                            && oBus.ProcessCCListRBC()
                            && oBus.SaveCCUpdateList()
                            && oBus.ProcessCCErrList(pathToReport, mailTo, mailBodyErr, mailSubjectErr)
                            && oBus.ProcessCCNewList(mailTo, mailBodyNew, mailSubjectNew, pathToReport, mailDateFormat)
                               )
                            {
                                break;
                            }
                            Thread.Sleep(retryInterval);
                        }
                        scope2.Complete();
                    }
                    if (oBus.isFileMoved && oBus.isStagingInfoMerged)
                    {
                        scope.Complete();
                    }
                }
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
            this.timer = new Timer(this.TimerTick, null, 30000, (inTestMode ? 60000 : this.milliseconds));

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
