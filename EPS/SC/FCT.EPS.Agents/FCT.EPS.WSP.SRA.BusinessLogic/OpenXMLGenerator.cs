using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.SRA.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FCT.EPS.WSP.SRA.BusinessLogic
{

    public class OpenXMLGenerator
    {

        //private IList<DataEntities.tblBatchPaymentReportInfo> ReportData;
        private string FileFormat;
        //private IList<DataEntities.tblPaymentReportFields> ReportFields;
        private int RowsBetweenHeaderAndData = 0;
        private IList<IEnumerable<ReportGenerator.MappedList>> myMappedData;


        public OpenXMLGenerator(IList<IEnumerable<ReportGenerator.MappedList>> passedMappedData, string passedFileFormat, int passedRowsBetweenHeaderAndData)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            //this.ReportData = passedReportData;
            this.FileFormat = passedFileFormat;
            //this.ReportFields = passedReportFields;
            this.myMappedData = passedMappedData;
            this.RowsBetweenHeaderAndData = passedRowsBetweenHeaderAndData;
            SolutionTraceClass.WriteLineVerbose("End");
        }

        public MemoryStream GenerateReport()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            MemoryStream myMemoryStream = new MemoryStream(10000);
            Package MyPackage = Package.Open(myMemoryStream, FileMode.Create);

            //Create an inmemory OPEN EXCEL document
            SpreadsheetDocument myopenXmlDocument = SpreadsheetDocument.Create(MyPackage, SpreadsheetDocumentType.Workbook);

            PrepareDocument(myopenXmlDocument);

            AddColumnHeadersToSheet(myopenXmlDocument, myMappedData, RowsBetweenHeaderAndData);

            AddDataToSheet(myopenXmlDocument, myMappedData);

            //myopenXmlDocument.WorkbookPart.Workbook.Save();
            myopenXmlDocument.Close();

            SolutionTraceClass.WriteLineVerbose("End");
            return myMemoryStream;
        }


        private void AddDataToSheet(SpreadsheetDocument myopenXmlDocument, IList<IEnumerable<ReportGenerator.MappedList>> myMappedData)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            SheetData mySheetData = myopenXmlDocument.WorkbookPart.WorksheetParts.First().Worksheet.GetFirstChild<SheetData>();

            int RowCounter = mySheetData.Elements<Row>().Count() + 1;
            foreach (IEnumerable<ReportGenerator.MappedList> myData in myMappedData)
            {
                Row row = new Row();
                int ColumnCounter = 1;
                foreach (ReportGenerator.MappedList myMappedList in myData)
                {
                    Cell cell = null;
                    try
                    {
                        string FormatString = myMappedList.MetaData.tblPaymentReportFieldFormat == null ? "" : myMappedList.MetaData.tblPaymentReportFieldFormat.FieldFormat;
                        cell = new Cell()
                        {
                            CellReference = Utils.ExcelColumnFromNumber(ColumnCounter) + RowCounter,
                            DataType = CellValues.String,
                            CellValue = new CellValue(Utils.ConvertAndFormat(FormatString, myMappedList.ValueData.FieldValue, myMappedList.MetaData.TemplateFieldType))
                        };
                    }
                    catch (Exception ex)
                    {
                        SolutionTraceClass.WriteLineWarning(String.Format("Exception while Creating a cell for the report. Message was->{0}", ex.Message));
                        LoggingHelper.LogWarningActivity("Exception while Creating a cell for the report.", ex);
                        cell = new Cell()
                        {
                            CellReference = Utils.ExcelColumnFromNumber(ColumnCounter) + RowCounter,
                            DataType = CellValues.String,
                            CellValue = new CellValue(myMappedList.ValueData.FieldValue)
                        };
                    }

                    row.Append(cell);
                    ColumnCounter++;
                }
                mySheetData.Append(row);
                RowCounter++;
            }


            myopenXmlDocument.WorkbookPart.Workbook.Save();
            SolutionTraceClass.WriteLineVerbose("End");
        }

        private void AddColumnHeadersToSheet(SpreadsheetDocument myopenXmlDocument, IList<IEnumerable<ReportGenerator.MappedList>> myMappedData, int RowsBetweenHeaderAndData)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            SheetData mySheetData = myopenXmlDocument.WorkbookPart.WorksheetParts.First().Worksheet.GetFirstChild<SheetData>();

            int RowCounter = 1;
            Row row = new Row();
            int ColumnCounter = 1;
            foreach (ReportGenerator.MappedList myMappedList in myMappedData.First())
            {
                Cell cell = null;
                try
                {
                    cell = new Cell()
                    {
                        CellReference = Utils.ExcelColumnFromNumber(ColumnCounter) + RowCounter,
                        DataType = CellValues.String,
                        CellValue = new CellValue(myMappedList.MetaData.tblPaymentReportLabels.FieldLabelName)
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

            for (int Counter = 1; Counter <= RowsBetweenHeaderAndData;Counter++ )
            {
                row = new Row();
                mySheetData.Append(row);
            }


            myopenXmlDocument.WorkbookPart.Workbook.Save();
            SolutionTraceClass.WriteLineVerbose("End");
        }

        private void PrepareDocument(SpreadsheetDocument myopenXmlDocument)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
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
