using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    public class MsiParser
    {
        static Type InstallerType = Type.GetTypeFromProgID("WindowsInstaller.Installer");
        static dynamic installer = Activator.CreateInstance(InstallerType);
        static string propertyProductName = "ProductName";
        static string propertyProductCode = "ProductCode";
        dynamic database, view, result;

        public ProductInfo GetProductInfoFromMSIFile(string msiFileName)
        {
            ProductInfo productInfo = new ProductInfo();
            database = installer.OpenDatabase(msiFileName, 0);
            view = database.OpenView("SELECT Value FROM Property WHERE Property='" + propertyProductName + "'");
            view.Execute();
            result = view.Fetch;
            productInfo.Name = result.StringData(1);

            view = database.OpenView("SELECT Value FROM Property WHERE Property='" + propertyProductCode + "'");
            view.Execute();
            result = view.Fetch;
            string rawCode = result.StringData(1);

            string pattern = "[-|{|}]";
            string replacement = "";
            Regex rgx = new Regex(pattern);
            productInfo.Code = rgx.Replace(rawCode, replacement);

            return productInfo;
        }

        public List<ProductInfo> GetProductInfoFromMSIFile(List<string> msiFileNameList)
        {
            ProductInfo productInfo = null;
            Dictionary<string, ProductInfo> productInfoDict = new Dictionary<string, ProductInfo>();
            foreach (string msiFileName in msiFileNameList)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(msiFileName))
                    {
                        productInfo = GetProductInfoFromMSIFile(msiFileName);
                        if (productInfo != null &&
                            !productInfoDict.ContainsKey(productInfo.Code)                            
                            )
                        {
                            productInfoDict.Add(productInfo.Code, productInfo);
                        }
                        else
                        {
                            //skip the record
                            //added just for testing
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error :" + ex.Message);                
                } //do nothing move to next file ??
            }


            return productInfoDict.Values.ToList<ProductInfo>();
        }
    }
}
