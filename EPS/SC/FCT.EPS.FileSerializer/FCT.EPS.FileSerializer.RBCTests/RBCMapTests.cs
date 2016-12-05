using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.FileSerializer.RBC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCT.EPS.FileSerializer.Common;
using System.Reflection;
using System.IO;
namespace FCT.EPS.FileSerializer.RBC.Tests
{
    [TestClass()]
    public class RBCMapTests
    {
        [TestMethod()]
        public void RBCMapTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetMapTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TestMap()
        {
            RBCRemittanceHeaderDataValues headerData = new RBCRemittanceHeaderDataValues()
            {
                RecordType = 01,
                ClientNumber = "3465",
                TransmitID = "FCT1245",
                TransmissionDate = DateTime.Now,
                SequenceNumber = 2
            };

            RBCRemittanceBodyDataValues bodyData = new RBCRemittanceBodyDataValues()
            {
                RecordType  = 5,
                AccountNumber = "6546513654987",
                Filler1 = "",
                PrimaryName = "Paul Binnell",
                CCIN = "90008218",
                PayeeName = "John Walker",
                CompanyID = "987fs",
                PayeeAddress1 = "300 Arch",
                PayeeAddress2 = "Here and there",
                PayeeCity = "Brampton",
                PayeeStateProvince = "Ontario",
                PayeePostalCode = "U8Y7U8",
                PayeeCountry = "CAN",
                PayeeAccountNumber = "457896548514",
                BalanceTransferAmount = (int)(548695.52M * 100),
                RequestedDate = DateTime.Now,
                PaymentReferenceNumber = "546sa5df4"
            };

            RBCRemittanceFooterDataValues footerData = new RBCRemittanceFooterDataValues()
            {
                RecordType = 1,
                ClientNumber = "8648",
                TransmitID = "fct47",
                TotalNumberOfBalanceTransfers = 3,
                TotalNumberOfFastCashRequests = 1,
                TotalRecords = 3
            };

            RBCRemittanceFileHeaderProcessor<RBCRemittanceHeaderDataValues> RBCHeader = new RBCRemittanceFileHeaderProcessor<RBCRemittanceHeaderDataValues>(headerData);
            RBCRemittanceFileBodyProcessor<RBCRemittanceBodyDataValues> RBCBody = new RBCRemittanceFileBodyProcessor<RBCRemittanceBodyDataValues>(bodyData);
            RBCRemittanceFileFooterProcessor<RBCRemittanceFooterDataValues> RBCFooter = new RBCRemittanceFileFooterProcessor<RBCRemittanceFooterDataValues>(footerData);




            StringBuilder test = new StringBuilder();
            test.AppendLine(RBCHeader.SerializeToStringFormat());
            test.AppendLine(RBCBody.SerializeToStringFormat());
            test.AppendLine(RBCFooter.SerializeToStringFormat());

            Console.WriteLine(test);

        }

        [TestMethod()]
        public void Test_RBCRejectedTranactions_Deserialize()
        {
            string fileName = @"C:\DOC\WORK\TASK\EPS-Merger for CREDIT CARD\RejectedErrorfile.txt";
            RBCRejectedTranactionsBodyDataValues datavalue = new RBCRejectedTranactionsBodyDataValues();

            RBCRejectedTranactionsFileBodyProcessor<RBCRejectedTranactionsBodyDataValues> process = new RBCRejectedTranactionsFileBodyProcessor<RBCRejectedTranactionsBodyDataValues>(datavalue);

            IEnumerable<string> text = System.IO.File.ReadLines(fileName);

            foreach (string line in text)
            {
                if(line.Length > 2 && line.Substring(0,2) == "05")
                {
                    process.DeserializeFromString(line, ref datavalue);
                }
                else
                {
                    datavalue.ErrorStrings.Add(line);
                }
            }
        }

        [TestMethod()]
        public void Test_RBCPayeeListBodyDataValues_Deserialize()
        {
            DateTime dt = DateTime.Now;
            //string fileName = @"C:\wrkdir\EASYFUND\CCLISTA-PROD.CTL";
            string fileName = @"CCLISTA";//Path.Combine(Environment.CurrentDirectory, )
            string pathOut = fileName + ".csv";
            StreamReader fr = null;
            StreamWriter fw = null;
            try
            {
                using (fr = new StreamReader(fileName))
                {
                    string pOut = pathOut;
                    if (File.Exists(pOut))
                    {
                        try
                        {
                            File.Delete(pOut);
                        }
                        catch (Exception) { }  // just leave it [it is not the test is about!] - the info will be appended though..
                    }
                    IList<RBCPayeeListBodyDataValues> myRBCPayeeListBodyDataValuesList = new List<RBCPayeeListBodyDataValues>();
                    using (fw = new StreamWriter(pOut, true))
                    {
                        string s;
                        string heads = FormatObjPropsToCsvHead(typeof(RBCPayeeListBodyDataValues));
                        fw.WriteLine(heads);
                        while (!string.IsNullOrEmpty(s = fr.ReadLine()))
                        {
                            RBCPayeeListBodyDataValues datavalue = new RBCPayeeListBodyDataValues();
                            RBCPayeeListFileBodyProcessor<RBCPayeeListBodyDataValues> process = new RBCPayeeListFileBodyProcessor<RBCPayeeListBodyDataValues>(datavalue);
                            process.DeserializeFromString(s, ref datavalue);
                            myRBCPayeeListBodyDataValuesList.Add(datavalue);
                            s = FormatObjToCsv(datavalue);
                            fw.WriteLine(s);
                        }
                    }
                }
            }
            finally
            {
                if (fr != null)
                    fr.Dispose();
                if (fw != null)
                    fw.Dispose();
            }
            Assert.IsTrue((DateTime.Now - dt).Seconds < 10);
        }
        private string FormatObjPropsToCsvHead(Type type)
        {
            return string.Join("",type.GetProperties().Select((info,idx) => (idx > 0 ? "," : "") + info.Name).Cast<string>().ToArray());
        }
        private string FormatObjPropsToCsvHead(object o)
        {
            return o == null ? null : FormatObjPropsToCsvHead(o.GetType());
        }
        private string FormatObjToCsv(object o)
        {
            if (o == null)
                return null;
            int totalLen = 4400;
            StringBuilder sb = new StringBuilder(totalLen);
            foreach (PropertyInfo info in o.GetType().GetProperties())
            {
                object v = info.GetValue(o);
                sb.Append((sb.Length > 0 ? "," : "") + "\"" + ConvertToStrIfStartsWith0((info.GetValue(o)??"").ToString().Trim().Replace("\0", "")) + "\""); // quotate segments
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

        public static void print(BaseDataValues datavalue)
        {
            foreach (PropertyInfo pi in datavalue.GetType().GetProperties())
            {
                Console.WriteLine(pi.Name + " -> " + pi.GetValue(datavalue));
            }
        }

    }
}
