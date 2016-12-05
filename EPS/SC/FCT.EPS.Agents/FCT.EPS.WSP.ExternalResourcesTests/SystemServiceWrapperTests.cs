using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.ExternalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FCT.EPS.WSP.ExternalResources.Tests
{
    [TestClass()]
    public class SystemServiceWrapperTests
    {
        [TestMethod()]
        public void SendEmailTest()
        {
            SystemServiceWrapper mySystemServiceWrapper = new SystemServiceWrapper();

            mySystemServiceWrapper.SendEmail("pbinnell@fct.ca", null, "Subject Line", "Message Line", null, null);
        }

        [TestMethod()]
        public void SendEmailTest2()
        {
            SystemServiceWrapper target = new SystemServiceWrapper(); // TODO: Initialize to an appropriate value
            string emailAddress = "pbinnell@fct.ca"; // TODO: Initialize to an appropriate value
            string dealId = ""; // TODO: Initialize to an appropriate value
            string subject = "SendEmail Test for EPS report sender";
            string message = "Please find attached a test document used for this email test."; // TODO: Initialize to an appropriate value
            string filePath = "http://intratrain.prefirstcdn.com/WrkDir/20150501/09/22/7e2aa186-8cbd-45fd-9746-0bb951cc45e7/LLCAcknowledgementEnglish.PDF;"; // TODO: Initialize to an appropriate value
            //string filePath = @"DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141003004090 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003759 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003760 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003761 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003755 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003756 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003758 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003765 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003766 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003767 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003762 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003763 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003764 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003728 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003729 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003727 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003725 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003726 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003753 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003754 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003752 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003730 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141009003731 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141003004091 C149337;DMS://FCT Documents\1\4\2\7\6\0\1\1\7\8\0\Owner Policy (CTI) Package - 141003004093 C149337"; // TODO: Initialize to an appropriate value
            string userId = ""; // TODO: Initialize to an appropriate value
            string jobID = target.SendEmail(emailAddress, dealId, subject, message, filePath, userId);
            Assert.IsNotNull(jobID);
            Assert.IsTrue(Convert.ToInt32(jobID) != 0);
        }


        [TestMethod()]
        public void GetJobStatusTest()
        {
            SystemServiceWrapper target = new SystemServiceWrapper();
            string jobId = "2859371";
            string actual;
            try
            {
                actual = target.GetJobStatus(jobId);
            }
            catch
            {
                Assert.Fail("Not Supposed to be here");
            }
        }
    }
}
