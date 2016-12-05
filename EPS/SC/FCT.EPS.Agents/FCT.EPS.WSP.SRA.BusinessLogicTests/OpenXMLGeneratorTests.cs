using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.SRA.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FCT.EPS.WSP.SRA.BusinessLogic.Tests
{
    [TestClass()]
    public class OpenXMLGeneratorTests
    {
        [TestMethod()]
        public void GenerateReportTest()
        {
            OpenXMLGenerator myOpenXMLGenerator = new OpenXMLGenerator(new List<IEnumerable<ReportGenerator.MappedList>>(), "XLS",5);

            myOpenXMLGenerator.GenerateReport();
        }

    }
}
