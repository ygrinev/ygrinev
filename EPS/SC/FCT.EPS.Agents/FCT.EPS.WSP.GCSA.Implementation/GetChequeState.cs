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
using FCT.EPS.WSP.GCSA.BusinessLogic;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.GCSA.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace FCT.EPS.WSP.GCSA.Implementation
{
    public class ChequeStateAgent : MarshalByRefObject, IAgent, IDisposable
    {
        
        private Timer timer;
        private int milliseconds;
        private int recordsToGet;
        private string endpoint;
        private bool IsConfigFileRead = false;

        public ChequeStateAgent()
        {
            SolutionTraceClass.appTraceSwitch = new TraceSwitch(AgentConstants.Misc.LOGGING_APPLICATION_TITLE, "FCT.EPS.WSP.GCSA Defined in config file.");
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
                    this.milliseconds = Properties.Settings.Default.GetChequeStateIntervalInMilliSeconds;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.milliseconds = {0}", this.milliseconds));

                    this.recordsToGet = Properties.Settings.Default.HowManyRecordsToGet;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.recordsToGet = {0}", this.recordsToGet));

                    this.endpoint = Properties.Settings.Default.DefaultEndpoint;
                    SolutionTraceClass.WriteLineInfo(string.Format("this.endpoint = {0}", this.endpoint));
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

                BusinessLayer.ProcessChequeStatus(this.recordsToGet, this.endpoint);

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
                LoggingHelper.LogWarningActivity(ex);
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
