using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using FCT.EPS.NotificationMonitor.Resources;
namespace FCT.EPS.WSP.Resources.Tests
{
    [TestClass(), ExcludeFromCodeCoverage]
    public class SolutionTraceClassTests
    {
        [ExcludeFromCodeCoverage, TestCategory("Int"), TestMethod()]
        public void SolutionTraceClass_WriteLineErrorTest()
        {
            SolutionTraceClass.WriteLineError("WriteLineError");
        }

        [ExcludeFromCodeCoverage, TestCategory("Int"), TestMethod()]
        public void SolutionTraceClass_WriteLineWarningTest()
        {
            SolutionTraceClass.WriteLineWarning("WriteLineWarning");
        }

        [ExcludeFromCodeCoverage, TestCategory("Int"), TestMethod()]
        public void SolutionTraceClass_WriteLineInfoTest()
        {
            SolutionTraceClass.WriteLineInfo("WriteLineInfo");
        }

        [ExcludeFromCodeCoverage, TestCategory("Int"), TestMethod()]
        public void SolutionTraceClass_WriteLineVerboseTest()
        {
            SolutionTraceClass.WriteLineVerbose("WriteLineVerbose");
        }
    }
}
