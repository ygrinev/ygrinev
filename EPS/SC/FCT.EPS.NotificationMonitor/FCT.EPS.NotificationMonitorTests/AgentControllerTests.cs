using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.NotificationMonitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FCT.EPS.NotificationMonitor.Tests
{
    [TestClass()]
    public class AgentControllerTests
    {
        [TestMethod()]
        public void AgentController_AgentControllerTest()
        {
            AgentController myAgentController = new AgentController();
        }


        [TestMethod()]
        public void AgentController_OnStartTest()
        {
            AgentController myAgentController = new AgentController();
            PrivateObject PrivateAgentController = new PrivateObject(myAgentController);

            PrivateAgentController.Invoke("OnStart",new Object[]{null});

        }
        
    }
}
