using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
   public static class SQLGenerator
    {
       static string sqlSatementFormat = "exec [dbo].[LogDummyDeployment] N'{0}', N'{1}', N'{2}', 0";
       public static void GenerateSQLScript(List<ProductInfo> productInfoList, string sqlFilename, string userID)
       {
           using (System.IO.StreamWriter file = new System.IO.StreamWriter(sqlFilename))
           {
               foreach (ProductInfo product in productInfoList)
               {
                       string sqlStmt = string.Format(sqlSatementFormat, product.Name, product.Code, userID);                   
                       file.WriteLine(sqlStmt);
                       file.WriteLine("GO");
                   
               }
           }
       }
    }
}
