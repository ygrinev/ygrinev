using FCT.EPS.NotificationMonitor.Properties;
using FCT.EPS.NotificationMonitor.Resources;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.NotificationMonitor
{
    public partial class AgentController : ServiceBase
    {
        ICollection<FCT.EPS.WindowsServiceAgentInterface.IAgent> Agents = null;
        string ApplicationName = string.Empty;
        string Agentspath = string.Empty;

        public AgentController()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {
                InitializeComponent();

                DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
                IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
                LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
                Logger.SetLogWriter(logWriterFactory.Create());

                ApplicationName = Settings.Default.ApplicationName;
                Agentspath = Settings.Default.Agentspath ?? string.Empty;

                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.AssemblyResolve += new ResolveEventHandler(AdditionalFileAssemblyResolve);
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(string.Format("Error setting of the service. Message is '{0}'. Stack track is '{1}'.",ex.Message,ex.StackTrace));
                //Cannot log since something seems to have gone wrong with the setup of logging in the previous code.
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }

        protected override void OnStart(string[] args)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            //System.Threading.Thread.Sleep(10000);
            try
            {
                //Release all agents
                if (Agents != null)
                {
                    foreach (FCT.EPS.WindowsServiceAgentInterface.IAgent myAgent in Agents)
                    {
                        SolutionTraceClass.WriteLineVerbose("Stopping and Releasing Agent " + myAgent.Name);
                        myAgent.OnStop();
                        myAgent.Dispose();
                    }
                    Agents = null;
                }

                //Load agents
                Agentspath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Agentspath);
                SolutionTraceClass.WriteLineVerbose("Agentspath = '" + Agentspath + "'");
                Agents = GenericPluginLoader<FCT.EPS.WindowsServiceAgentInterface.IAgent>.LoadPlugins(Agentspath);

                //Start all agents
                if (Agents != null)
                {
                    foreach (FCT.EPS.WindowsServiceAgentInterface.IAgent myAgent in Agents)
                    {
                        SolutionTraceClass.WriteLineVerbose("Starting and Releasing Agent " + myAgent.Name);
                        //Start the agent
                        myAgent.OnStart(null);
                    }
                    Agents = null;
                }
            }
            catch(Exception ex)
            {
                SolutionTraceClass.WriteLineError(string.Format("Error in OnStart.  Exception of type '{0}' was thrown message was '{1}'",ex.GetType().AssemblyQualifiedName,ex.Message));
                Logging.LogCriticalActivity(ex, Constants.EventID.NOTIFICATION_MONITOR, TraceEventType.Critical, ApplicationName);
                throw;
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }

        protected override void OnStop()
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            //Stop all agents
            if (Agents != null)
            {
                foreach (FCT.EPS.WindowsServiceAgentInterface.IAgent myAgent in Agents)
                {
                    SolutionTraceClass.WriteLineVerbose("Stopping and disposing Agent " + myAgent.Name);
                    myAgent.OnStop();
                    myAgent.Dispose();
                }
                Agents = null;
            }


            SolutionTraceClass.WriteLineVerbose("End");
        }

        protected override void OnContinue()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            //Continue all agents
            if (Agents != null)
            {
                foreach (FCT.EPS.WindowsServiceAgentInterface.IAgent myAgent in Agents)
                {
                    SolutionTraceClass.WriteLineVerbose("Continueing Agent " + myAgent.Name);
                    myAgent.OnContinue();
                }
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }

        protected override void OnPause()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            //Pause all agents
            if (Agents != null)
            {
                foreach (FCT.EPS.WindowsServiceAgentInterface.IAgent myAgent in Agents)
                {
                    SolutionTraceClass.WriteLineVerbose("Pausing Agent " + myAgent.Name);
                    myAgent.OnPause();
                }
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }


        Assembly AdditionalFileAssemblyResolve(object sender, ResolveEventArgs args)
        {
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.

            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            //string applicationDirectory = Path.GetDirectoryName(executingAssembly.Location);

            string[] fields = args.Name.Split(',');
            string assemblyName = fields[0];
            string assemblyCulture;
            if (fields.Length < 2)
                assemblyCulture = null;
            else
                assemblyCulture = fields[2].Substring(fields[2].IndexOf('=') + 1);


            string assemblyFileName = assemblyName + ".dll";

            //file must be in the Agents directory
            string assemblyPath = Path.Combine(Agentspath, assemblyFileName);



            if (File.Exists(assemblyPath))
            {
                //Load the assembly from the specified path.                    
                Assembly loadingAssembly = Assembly.LoadFrom(assemblyPath);

                //Return the loaded assembly.
                return loadingAssembly;
            }
            else
            {
                return null;
            }

        }

    }
}
