using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.GCLA.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
namespace FCT.EPS.WSP.GCLA.BusinessLogic.Tests
{
    [TestClass()]
    public class OpenXMLGeneratorTests
    {
        [TestMethod()]
        public void OpenXMLGeneratorTest()
        {
            OpenXMLGenerator myOpenXMLGenerator = new OpenXMLGenerator();
            myOpenXMLGenerator.Close();

            //Create the file
            string FileArchiveLocation = @"C:\wrkdir\EASYFUND\FCT\Report";
            DateTime myDateTime = DateTime.Now;
            string FileName = String.Format("RBC CC List Update – New Corporate Creditors on {0:yyyy-MM-dd}.xlsx", myDateTime);
            String archiveFileName = System.IO.Path.Combine(new string[] { FileArchiveLocation, myDateTime.Year.ToString("00"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), FileName });
            new FileInfo(archiveFileName).Directory.Create();
            File.WriteAllBytes(archiveFileName, myOpenXMLGenerator.Document.ToArray());

        }

        [TestMethod()]
        public void AddRowToSheetTest()
        {
            OpenXMLGenerator myOpenXMLGenerator = new OpenXMLGenerator();

            myOpenXMLGenerator.AddRowToSheet(new FCT.EPS.WSP.GCLA.BusinessLogic.OpenXMLGenerator.ReportCell() { CellValue = new CellValue("Test1"), DataType = CellValues.String });

            myOpenXMLGenerator.Close();

            //Create the file
            string FileArchiveLocation = @"C:\wrkdir\EASYFUND\FCT\Report";
            DateTime myDateTime = DateTime.Now;
            string FileName = String.Format("RBC CC List Update – New Corporate Creditors on {0:yyyy-MM-dd}.xlsx", myDateTime);
            String archiveFileName = System.IO.Path.Combine(new string[] { FileArchiveLocation, myDateTime.Year.ToString("00"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), FileName });
            new FileInfo(archiveFileName).Directory.Create();
            File.WriteAllBytes(archiveFileName, myOpenXMLGenerator.Document.ToArray());

        }

        [TestMethod()]
        public void AddRowToSheet2Test()
        {
            OpenXMLGenerator myOpenXMLGenerator = new OpenXMLGenerator();

            myOpenXMLGenerator.AddRowToSheet(new FCT.EPS.WSP.GCLA.BusinessLogic.OpenXMLGenerator.ReportCell() { CellValue = new CellValue("Test10"), DataType = CellValues.String }, new FCT.EPS.WSP.GCLA.BusinessLogic.OpenXMLGenerator.ReportCell() { CellValue = new CellValue("Test11"), DataType = CellValues.String });
            myOpenXMLGenerator.AddRowToSheet();
            myOpenXMLGenerator.AddRowToSheet();
            myOpenXMLGenerator.AddRowToSheet(new FCT.EPS.WSP.GCLA.BusinessLogic.OpenXMLGenerator.ReportCell() { CellValue = new CellValue("Test20"), DataType = CellValues.String });

            myOpenXMLGenerator.Close();

            //Create the file
            string FileArchiveLocation = @"C:\wrkdir\EASYFUND\FCT\Report";
            DateTime myDateTime = DateTime.Now;
            string FileName = String.Format("RBC CC List Update – New Corporate Creditors on {0:yyyy-MM-dd}.xlsx", myDateTime);
            String archiveFileName = System.IO.Path.Combine(new string[] { FileArchiveLocation, myDateTime.Year.ToString("00"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), FileName });
            new FileInfo(archiveFileName).Directory.Create();
            File.WriteAllBytes(archiveFileName, myOpenXMLGenerator.Document.ToArray());

        }
    }
}
