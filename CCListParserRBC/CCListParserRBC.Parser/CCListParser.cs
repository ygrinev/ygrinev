using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCListParserRBC.Parser
{
    public class CCListParser
    {
        private string _pathIn, _pathOut;
        const string heads = "CompanyID,CompanyName,Department,Address,City,Province,Postal Code,ContactName"+ // 7
            ",ContactPhone,AccountNumberExactLength,AccountNumberMinLength,AccountNumberMaxLength,EditRulesForAccountNumbers"+ //12
            ",DataTypeFormatForAccountNumbers,ValidStartForAccountNumbers,InvalidStartForAccountNumbers"+ // 15
            ",ValidEndForAccountNumbers,InvalidEndForAccountNumbers,Timestamp,CCIN,CompanyFrenchName,CoEngShortName"+ // 21
            ",CoFrShortName,LangIndicator,EffectiveDate,CurrentRecord,CreditorType"; // 26
        static int[] format = { 6, 35, 35, 35, 19, 2, 7, 35, 14, 2, 2, 2, 10, 30, 1000, 1000, 1000, 1000, 26, 8, 35, 15, 15, 1, 10, 1, 4};
        int totalLen = format.Sum();
        public CCListParser(string pathIn, string pathOut)
        {
            StreamReader fr = null;
            StreamWriter fw = null;
            try
            {
                string pathArr = pathIn ?? _pathIn;
                foreach (string sFullPath in pathArr.Split(','))
                {
                    String filePatt = System.IO.Path.GetFileName(sFullPath);
                    String path = System.IO.Path.GetDirectoryName(sFullPath);
                    foreach (string sFile in Directory.GetFiles(path, filePatt))
                    {
                        using (fr = new StreamReader(sFile, Encoding.Default, true))
                        {
                            string pOut = sFile + ".csv";
                            string pError = sFile + ".Errors.txt";
                            if (File.Exists(pOut))
                            {
                                File.Delete(pOut);
                            }
                            List<string> errors = new List<string>();
                            using (fw = new StreamWriter(pOut, true, Encoding.Default))
                            {
                                string s;
                                fw.WriteLine(heads);
                                int counter = 0;
                                while (!string.IsNullOrEmpty(s = fr.ReadLine()))
                                {
                                    counter++;
                                    if(s.Length != 4349)
                                    {
                                        errors.Add("***** Line #" + counter + "[CompanyID=" + s.Substring(0,6) + "] has length=" + s.Length + ", must be " + format.Sum()); // 4349
                                        continue;
                                    }
                                    string strIn = FormatLineToCsv(s);
                                    //if (s.Contains("RENO ASSISTANCE"))
                                    //{
                                    //    string sEncoded = s.Substring(s.Length - 40);
                                    //}
                                    fw.WriteLine(strIn);
                                }
                            }
                            if(errors.Count() > 0)
                            {
                                if (File.Exists(pError))
                                {
                                    File.Delete(pError);
                                }
                                using(fw = new StreamWriter(pError, true, Encoding.Default))
                                {
                                    foreach(string error in errors)
                                    {
                                        fw.WriteLine(error);
                                    }
                                }
                            }
                        }
                    }
                }
                _pathIn = pathIn;
                _pathOut = pathOut;
            }
            catch (Exception x)
            {
                string s = x.Message;
            }
            finally
            {
                if(fr != null)
                    fr.Dispose();
                if(fw != null)
                    fw.Dispose();
            }
        }
        private string FormatLineToCsv(string s)
        {
            if(string.IsNullOrEmpty(s) || s.Length < totalLen)
                return s;
            StringBuilder sb = new StringBuilder(totalLen + format.Length*3);
            int pos = 0;
            foreach (int n in format)
            {
                sb.Append((pos > 0 ? "," : "") + "\"" + ConvertToStrIfStartsWith0(s.Substring(pos, n).Trim().Replace("\0","")) + "\""); // quotate segments
                pos += n;
            }
            return sb.ToString();
        }
        private string ConvertToStrIfStartsWith0(string s)
        {
            string sret = s;
            if (!string.IsNullOrEmpty(sret) && sret[0] == '0') 
                sret = "=\"\"" + s + "\"\"";
            return sret;
        }
    }
}
