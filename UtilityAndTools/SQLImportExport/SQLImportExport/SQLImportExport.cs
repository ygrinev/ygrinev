using System;
using System.Linq;
using System.Data.SqlClient;

namespace SQLImportExport
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public  class SQLImportExport  {
       public SQLImportExport()
       {
        

#if EN_US_CULTURE
         System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
         System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
#endif
      }

      //private void btnCreateBasicWorkbook_Click(object sender, RoutedEventArgs e) {
      //   this.CreateBasicWorkbook("BasicWorkbook.xlsx", true);
      //}

      //private void btnCreate10000SharedStrings_Click(object sender, RoutedEventArgs e) {
      //   this.CreateStringWorkbook("SharedStrings10000.xlsx", 10000, true);

      //}
      //private void btnCreate10000Strings_Click(object sender, RoutedEventArgs e) {
      //   this.CreateStringWorkbook("Strings10000.xlsx", 10000, false);
      //}

      //private void btnCreateBasicWorkbookPredefinedStyles_Click(object sender, RoutedEventArgs e) {
      //   this.CreateBasicWorkbook("BasicWorkbookPredefinedStyles.xlsx", false);
      //}

      /// <summary>
      /// Creates a workbook with specified amount of strings
      /// </summary>
      /// <param name="workbookName">Name of the workbook</param>
      /// <param name="stringCount">Number of strings to add</param>
      /// <param name="useShared">Use shared strings?</param>
      /// <returns>True if succesful</returns>
       public bool CreateStringWorkbook(string workbookName)
       {
          // int stringCount =1000;
           bool useShared=false;
         DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadsheet;
         DocumentFormat.OpenXml.Spreadsheet.Worksheet worksheet;

         spreadsheet = Excel.CreateWorkbook(workbookName);
         if (spreadsheet == null) {
            return false;
         }

         Excel.AddBasicStyles(spreadsheet);
         //Excel.AddWorksheet(spreadsheet, "Strings");
         Excel.AddWorksheet(spreadsheet, Program.TABLENAME);
         worksheet = spreadsheet.WorkbookPart.WorksheetParts.First().Worksheet;

         //// Add shared strings
         //for (uint counter = 0; counter < stringCount; counter++) {
         //   Excel.SetCellValue(spreadsheet, worksheet, 1, counter + 1, "Some string", useShared, false);
         //}
         // Set column widths
//         Excel.SetColumnWidth(worksheet, 1, 15);

         string masterConnectionString = string.Empty;

         using (SqlConnection con = new SqlConnection(Program.CONNECTION))
         {
             con.Open();

             using (SqlCommand command = new SqlCommand(Program.SELECTQUERY, con))
             {
                 SqlDataReader reader = command.ExecuteReader();
                 uint counter = 1;
                 
                 while (reader.Read())
                 {
                     if (counter==1)
                     {
                         for (uint i = 0; i < reader.FieldCount; i++)
                         {
                             Excel.SetCellValue(spreadsheet, worksheet, i + 1, counter, reader.GetName((int)i), useShared, false);
                             Excel.SetColumnWidth(worksheet, (int)i + 1, 20);
                         }
                     }
                    counter++;
                   for (uint i = 0; i < reader.FieldCount; i++)
                   {
                       Excel.SetCellValue(spreadsheet, worksheet, i + 1, counter , GetString(reader[(int)i]), useShared, false);
                       Excel.SetColumnWidth(worksheet, (int)i + 1, 20);
                   }
                    

                 }
             }
         }




         worksheet.Save();
         spreadsheet.Close();

         System.Diagnostics.Process.Start(workbookName);
         return true;
      }

      /// <summary>
      /// Creates a basic workbook
      /// </summary>
      /// <param name="workbookName">Name of the workbook</param>
      /// <param name="createStylesInCode">Create the styles in code?</param>
      private void CreateBasicWorkbook(string workbookName, bool createStylesInCode) {
         DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadsheet;
         DocumentFormat.OpenXml.Spreadsheet.Worksheet worksheet;
         System.IO.StreamReader styleXmlReader;
         string styleXml;

         spreadsheet = Excel.CreateWorkbook(workbookName);
         if (spreadsheet == null) {
            return;
         }

         if (createStylesInCode) {
            Excel.AddBasicStyles(spreadsheet);
         } else {
            using (styleXmlReader = new System.IO.StreamReader("PredefinedStyles.xml")) {
               styleXml = styleXmlReader.ReadToEnd();
               Excel.AddPredefinedStyles(spreadsheet, styleXml);
            }
         }

        // Excel.AddSharedString(spreadsheet, "Shared string");
         Excel.AddWorksheet(spreadsheet, "Test 1");
      //   Excel.AddWorksheet(spreadsheet, "Test 2");
         worksheet = spreadsheet.WorkbookPart.WorksheetParts.First().Worksheet;

         //// Add shared strings
         //Excel.SetCellValue(spreadsheet, worksheet, 1, 1, "Shared string", true);
         //Excel.SetCellValue(spreadsheet, worksheet, 1, 2, "Shared string", true);
         //Excel.SetCellValue(spreadsheet, worksheet, 1, 3, "Shared string", true);

         //// Add a string
         //Excel.SetCellValue(spreadsheet, worksheet, 1, 5, "Number", false, false);
         //// Add a decimal number
         //Excel.SetCellValue(spreadsheet, worksheet, 2, 5, 1.23, null, true);

         //// Add a string
         //Excel.SetCellValue(spreadsheet, worksheet, 1, 6, "Integer", false, false);
         //// Add an integer number
         //Excel.SetCellValue(spreadsheet, worksheet, 2, 6, 1, null, true);

         //// Add a string
         //Excel.SetCellValue(spreadsheet, worksheet, 1, 7, "Currency", false, false);
         //// Add currency
         //Excel.SetCellValue(spreadsheet, worksheet, 2, 7, 1.23, 2, true);

         //// Add a string
         //Excel.SetCellValue(spreadsheet, worksheet, 1, 8, "Date", false, false);
         //// Add date
         //Excel.SetCellValue(spreadsheet, worksheet, 2, 8, System.DateTime.Now, 1, true);

         //// Add a string
         //Excel.SetCellValue(spreadsheet, worksheet, 1, 9, "Percentage", false, false);
         //// Add percentage
         //Excel.SetCellValue(spreadsheet, worksheet, 2, 9, 0.123, 3, true);

         //// Add a string
         //Excel.SetCellValue(spreadsheet, worksheet, 1, 10, "Boolean", false, false);
         //// Add percentage
         //Excel.SetCellValue(spreadsheet, worksheet, 2, 10, true, null, true);


         //// Set column widths
         //Excel.SetColumnWidth(worksheet, 1, 15);
         //Excel.SetColumnWidth(worksheet, 2, 20);

         worksheet.Save();
         spreadsheet.Close();

         System.Diagnostics.Process.Start(workbookName);
      }

      public static string GetString(object o)
      {
          if (o == DBNull.Value)
              return "";

          return o.ToString();
      }

      public void ReadWorkBook(string fileName)
      {
          Excel.ReadWorkBook(fileName);
      }

   }
}
