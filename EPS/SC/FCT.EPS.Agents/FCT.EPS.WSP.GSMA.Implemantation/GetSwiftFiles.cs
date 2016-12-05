using FCT.EPS.WindowsServiceAgentInterface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FCT.EPS.WSP.GSMA.BusinessLogic;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.GSMA.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace FCT.EPS.WSP.GSMA.Implemantation
{
    public class SwiftFilesAgent : MarshalByRefObject, IAgent, IDisposable
    {
        
        private Timer timer;
        private int milliseconds;
        private bool IsConfigFileRead = false;
        private FileLocations SwiftFileLocations = new FileLocations();

        public SwiftFilesAgent()
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
                    this.milliseconds = Properties.Settings.Default.AgentIntervalInMilliSeconds;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.milliseconds = {0}", this.milliseconds));

                    this.SwiftFileLocations.SwiftAckNackLocation = Properties.Settings.Default.AckNackFileLocation;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.SwiftFileLocations.SwiftAckLocation = {0}", this.SwiftFileLocations.SwiftAckNackLocation));

                    this.SwiftFileLocations.SwiftAutoClientErrorLocation = Properties.Settings.Default.AutoClientErrorFileLocation;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.SwiftFileLocations.SwiftAutoClientErrorLocation = {0}", this.SwiftFileLocations.SwiftAutoClientErrorLocation));

                    this.SwiftFileLocations.SwiftConverterErrorLocation = Properties.Settings.Default.ConverterErrorFileLocation;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.SwiftFileLocations.SwiftConverterErrorLocation = {0}", this.SwiftFileLocations.SwiftConverterErrorLocation));

                    this.SwiftFileLocations.SwiftCreditLocation = Properties.Settings.Default.CreditFileLocation;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.SwiftFileLocations.SwiftCreditLocation = {0}", this.SwiftFileLocations.SwiftCreditLocation));

                    this.SwiftFileLocations.SwiftDebitLocation = Properties.Settings.Default.DebitFileLocation;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.SwiftFileLocations.SwiftDebitLocation = {0}", this.SwiftFileLocations.SwiftDebitLocation));

                    this.SwiftFileLocations.SwiftArchiveAckNackLocation = Properties.Settings.Default.ArchiveAckNackFileLocation;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.SwiftFileLocations.SwiftArchiveAckLocation = {0}", this.SwiftFileLocations.SwiftArchiveAckNackLocation));

                    this.SwiftFileLocations.SwiftArchiveAutoClientErrorLocation = Properties.Settings.Default.ArchiveAutoClientErrorFileLocation;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.SwiftFileLocations.SwiftArchiveAutoClientErrorLocation = {0}", this.SwiftFileLocations.SwiftArchiveAutoClientErrorLocation));

                    this.SwiftFileLocations.SwiftArchiveConverterErrorLocation = Properties.Settings.Default.ArchiveConverterErrorFileLocation;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.SwiftFileLocations.SwiftArchiveConverterErrorLocation = {0}", this.SwiftFileLocations.SwiftArchiveConverterErrorLocation));

                    this.SwiftFileLocations.SwiftArchiveCreditLocation = Properties.Settings.Default.ArchiveCreditFileLocation;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.SwiftFileLocations.SwiftArchiveCreditLocation = {0}", this.SwiftFileLocations.SwiftArchiveCreditLocation));

                    this.SwiftFileLocations.SwiftArchiveDebitLocation = Properties.Settings.Default.ArchiveDebitFileLocation;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.SwiftFileLocations.SwiftArchiveDebitLocation = {0}", this.SwiftFileLocations.SwiftArchiveDebitLocation));
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

                BusinessLayer.ProcessSwiftFilesStatus(this.SwiftFileLocations);

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
                SolutionTraceClass.WriteLineWarning("Exception was thrown message is -> " +ex.Message);
                LoggingHelper.LogErrorActivity(ex);
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
