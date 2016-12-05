using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FCT.EPS.WSP.GCLA.Resources;
using FCT.EPS.WSP.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.GCLA.BusinessLogic
{
    public class OpenXMLGenerator
    {
        public class ReportCell
        {
            public EnumValue<CellValues> DataType { get; set; }
            public CellValue CellValue { get; set; }
        }

        private Package MyPackage = null;
        private SpreadsheetDocument myopenXmlDocument = null;
        int RowCounter = 1;

        public MemoryStream Document
        { get; private set; }

        public void Close()
        {
            myopenXmlDocument.Close();
        }

        public OpenXMLGenerator()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            InitDocument();
            SolutionTraceClass.WriteLineVerbose("End");
        }


        public void AddRowToSheet(params ReportCell[] ReportHeaderCell)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            SheetData mySheetData = myopenXmlDocument.WorkbookPart.WorksheetParts.First().Worksheet.GetFirstChild<SheetData>();


            
            Row row = new Row();
            int ColumnCounter = 1;
            foreach (ReportCell myReportCell in ReportHeaderCell)
            {
                Cell cell = null;
                try
                {
                    cell = new Cell()
                    {
                        CellReference = Utils.ExcelColumnFromNumber(ColumnCounter) + RowCounter,
                        DataType =myReportCell.DataType,
                        CellValue = myReportCell.CellValue
                    };
                }
                catch (Exception ex)
                {
                    SolutionTraceClass.WriteLineWarning(String.Format("Exception while Creating a cell for the report. Message was->{0}", ex.Message));
                    LoggingHelper.LogWarningActivity("Exception while Creating a cell for the report.", ex);
                    throw;
                }

                row.Append(cell);
                ColumnCounter++;
            }
            mySheetData.Append(row);
            RowCounter++;

            myopenXmlDocument.WorkbookPart.Workbook.Save();
            SolutionTraceClass.WriteLineVerbose("End");
        }

        private void InitDocument()
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            Document = new MemoryStream(10000);
            MyPackage = Package.Open(Document, FileMode.Create);
            myopenXmlDocument = SpreadsheetDocument.Create(MyPackage, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            WorkbookPart myWorkbookPart = myopenXmlDocument.AddWorkbookPart();
            myWorkbookPart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart myworksheetPart = myWorkbookPart.AddNewPart<WorksheetPart>();
            myworksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            Sheets sheets = myopenXmlDocument.WorkbookPart.Workbook.
            AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet()
            {
                Id = myopenXmlDocument.WorkbookPart.GetIdOfPart(myworksheetPart),
                SheetId = 1,
                Name = "Sheet1"
            };
            sheets.Append(sheet);
            SolutionTraceClass.WriteLineVerbose("End");
        }

    }
}
