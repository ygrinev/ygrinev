using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCT.EPS.WindowsServiceAgentInterface;
using System.Diagnostics.CodeAnalysis;
using FCT.EPS.NotificationMonitor.Resources;

namespace FCT.EPS.NotificationMonitor.Resources.Tests
{
    [ExcludeFromCodeCoverage,TestClass()]
    public class GenericPluginLoaderTests
    {
        [ExcludeFromCodeCoverage, TestCategory("Int"), TestMethod()]
        public void GenericPluginLoader_LoadPluginsTest()
        {
            ICollection<IAgent> Agents = null;
            Agents = GenericPluginLoader<IAgent>.LoadPlugins(@"..\..\..\FCT.EPS.WSP.GetChequeStateAgent\bin\debug\");
            Assert.IsNotNull(Agents);

            //***********************************************************************************************************************
            try
            {
                foreach(IAgent myAgent in Agents)
                {
                    myAgent.OnStart(null);
                }
            }
            catch(Exception ex)
            {
                Assert.Fail("Not Supposed to be here");
            }
        }
    }
}
