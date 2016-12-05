using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.NotificationMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [System.LoaderOptimization(LoaderOptimization.MultiDomain)]
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new AgentController() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
