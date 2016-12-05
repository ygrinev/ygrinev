using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class MsiParserMain
    {
        static void Main(string[] args)
        {
            const string rootFolder = @"C:\Users\sgeorge\Source\Workspaces\APD\Packaging";

            //const string rootFolder = @"C:\Users\sgeorge\Source\Workspaces\APD\Packaging\EPS\FCT.EPS.PaymentService";
            const string sqlFilename = @"MsiParser.sql";
            const string userID = @"firstcdn\bmathew";

            MsiParser msiParser = new MsiParser();
            List<string> msiFileNameList = FileNameParser.ReadMsiFileNames(rootFolder);
            List<ProductInfo> productInfoList =  msiParser.GetProductInfoFromMSIFile(msiFileNameList);
            SQLGenerator.GenerateSQLScript(productInfoList, sqlFilename, userID);
        }     
    }
}
