using FCT.EPS.DataEntities;
using FCT.EPS.FileSerializer.RBC;
using FCT.EPS.WSP.DataAccess;
using FCT.EPS.WSP.GCLA.BusinessLogic;
using FCT.EPS.WSP.GCLA.Resources;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FCT.EPS.WSP.GCLA.BusinessLogicTests
{
    [TestClass()]
    public class BusinessLogicTests
    {
        private TimeSpan pollStart = new TimeSpan(0, 0, 6);
        private UnitOfWork unitOfWork;
        [ExcludeFromCodeCoverage, TestInitialize()]
        public void Initialize()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create(), false);
            unitOfWork = new UnitOfWork();
        }
        [TestMethod()]
        public void UnitOfWork_GetListCreditors()
        {
            List<tblCreditorList> myList = unitOfWork.TblCreditorListRepository.Get().ToList();

            Assert.IsNotNull(myList);
            Assert.IsNotNull(myList[0].tblCreditorRules);
        }
        [TestMethod()]
        public void File_ProvideCCListsForProcessing()
        {
            TimeSpan pollStart = new TimeSpan(6, 0, 0);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                if (!BusinessLayer.LockProcess(this.pollStart)) // in process or the process was completed this date
                    return;

                BusinessLayer busLayer = new BusinessLayer();
                using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
            //if (!BusinessLayer.LockProcess(new TimeSpan(6,0,0))) // in process or the process was completed this date
            //{
            //    Transaction.Current.Rollback();
            //    scope.Dispose();
            //    return;
            //}

                    DateTime now = DateTime.Now;
                    //string errFile = Path.Combine(Properties.Settings.Default.pathToReport, "Erroneous Corporate Creditors on " + now.Year.ToString("0000") + "-" + now.Month.ToString("00") + "-" + now.Day.ToString("00") + ".txt");
                    Assert.IsTrue(busLayer.MoveFile(Properties.Settings.Default.pathToCCListFile, Properties.Settings.Default.pathToArcCCListFile)
                        //&& busLayer.ParseAndStageCCList() 
                        && busLayer.ProcessCCListRBC()
                        //&& busLayer.SaveCCUpdateList()
                        //&& busLayer.ProcessCCErrList(Properties.Settings.Default.pathToReport, "ygrinev@fct.ca", Properties.Settings.Default.mailBodyErr, "RBC CC List has Error(s) - " + now.ToString())
                        //&& busLayer.ProcessCCNewList("ygrinev@fct.ca", "The following new corporate creditors were added on {0:ddd-MM-yyyy}: \r\n\t{1}\r\n\r\n\tPlease action accordingly.", "RBC CC List has New Creditor(s) - {0:ddd-MM-yyyy}",
                        //    Properties.Settings.Default.pathToReport, "{0:ddd-MM-yyyy}")
                        );
                    scope2.Complete();
                }
                if (busLayer.isFileMoved && busLayer.isStagingInfoMerged)
                    scope.Complete();
                else
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }
        }
        [TestMethod()]
        public void CheckTimerTickLogging()
        {
            LoggingHelper.LogAuditingActivity("TimerTick");
            Exception ex = new Exception("This is a test exception");
            LoggingHelper.LogErrorActivity(ex);
        }

        [TestMethod()]
        public void File_WriteReadEncoding()
        {
            const string sChrs = "É�";
            const string path0 = @"C:\DOC\WORK\TASK\EPS-Merger for CREDIT CARD\TEST\Encoding0.txt";
            const string path = @"C:\DOC\WORK\TASK\EPS-Merger for CREDIT CARD\TEST\Encoding_out.csv";
            int len1 = sChrs.Length;
            int len2 = 0;
            using (StreamReader fr = new StreamReader(path0, Encoding.Default, true))
            {
                string s = fr.ReadLine();
                int len = s.Length;
            }
            if (File.Exists(path)) File.Delete(path);
            using (StreamWriter fw = new StreamWriter(path, true, Encoding.Default))
            {
                fw.WriteLine(sChrs); // FormatLineToCsv(
            }
            using (StreamReader fr = new StreamReader(path, true))
            {
                len2 = fr.ReadLine().Length; // FormatLineToCsv(
            }
            Assert.IsTrue(len1 == len2);
        }

        [TestMethod()]
        public void TestStringToObject()
        {
            string s = "009741SHERWOOD BMR                                                          P.O. Box 216                       CHARLOTTETOWN      PEC1A 7K4Patsy Mills                        00009025666619990308          NNNNNNNN                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      2012-09-17-15.54.00.96669300000000                                   SHERWOOD BMR   SHERWOOD BMR   E2007-07-16YCR  ";
            RBCPayeeListBodyDataValues datavalue = new RBCPayeeListBodyDataValues();
            RBCPayeeListFileBodyProcessor<RBCPayeeListBodyDataValues> process = new RBCPayeeListFileBodyProcessor<RBCPayeeListBodyDataValues>(datavalue);
            process.DeserializeFromString(s, ref datavalue);


        }
        [TestMethod()]
        public void TestTriggerFiredDirectlyFromEFX() // tblRBCCreditorListStaging as an example, trigger: tblStaging_trCCList
        {
            // OUT-OFF-TRANSACTION STUFF
            string oldAddr = "";
            try
            {
                // IN-TRANSACTION STUFF
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    var stageRepoListOf2 = unitOfWork.TblRBCCreditorListStagingRepository.Get(r=>r.CompanyID == "000026" || r.CompanyID == "000043").ToList();
                    var item0 = stageRepoListOf2.FirstOrDefault(el=>el.CompanyID == "000043");
                    var item1 = stageRepoListOf2.FirstOrDefault(el=>el.CompanyID == "000026");
                    // legal UPDATE - change address
                    bool isOldAPattern = item1.Address.Length > 2 && item0.Address.Substring(0, 3) == "###";
                    oldAddr = item1.Address;
                    item0.Address = item1.Address.Length > 2 && item0.Address.Substring(0,3) == "@@@" ? ("111" + item0.Address.Substring(3)) : "@@@" + item0.Address.Substring(Math.Min(item0.Address.Length, 3));
                    item1.Address = isOldAPattern ? ("777" + item1.Address.Substring(3)) : "###" + item0.Address.Substring(Math.Min(item1.Address.Length, 3));
                    // illegal update - change PK of the 1st row to the existing ID
                    //item0.CompanyID = "000026";
                    unitOfWork.Save();
                    scope.Complete();
                }
            }
            catch (Exception)
            {
               // Transaction.Current.Rollback();
            }

            // VERIFICATION STUFF
            string newAddr = unitOfWork.TblAddressRepository.Get(r=>r.AddressID == 2).FirstOrDefault().StreetAddress1;
            Assert.IsTrue(newAddr != oldAddr);
        }
        [TestMethod()]
        public void TestRollbackOfTriggerFiredDirectlyFromEFX() // tblRBCCreditorListStaging as an example, trigger: tblStaging_trCCList
        {
            // OUT-OFF-TRANSACTION STUFF
            string oldAddr = "";
            try
            {
                // IN-TRANSACTION STUFF
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    var stageRepoListOf2 = unitOfWork.TblRBCCreditorListStagingRepository.Get(r=>r.CompanyID == "000026" || r.CompanyID == "000043").ToList();
                    var item0 = stageRepoListOf2.FirstOrDefault(el=>el.CompanyID == "000043");
                    var item1 = stageRepoListOf2.FirstOrDefault(el=>el.CompanyID == "000026");
                    // legal UPDATE - change address
                    bool isOldAPattern = item1.Address.Length > 2 && item0.Address.Substring(0, 3) == "###";
                    oldAddr = item1.Address;
                    item0.Address = item1.Address.Length > 2 && item0.Address.Substring(0,3) == "@@@" ? ("111" + item0.Address.Substring(3)) : "@@@" + item0.Address.Substring(Math.Min(item0.Address.Length, 3));
                    item1.Address = isOldAPattern ? ("777" + item1.Address.Substring(3)) : "###" + item0.Address.Substring(Math.Min(item1.Address.Length, 3));
                    // illegal update - change PK of the 1st row to the existing ID
                    item0.CompanyID = "000026";
                    unitOfWork.Save();
                    scope.Complete();
                }
            }
            catch (Exception)
            {
               // Transaction.Current.Rollback();
            }

            // VERIFICATION STUFF
            string newAddr = unitOfWork.TblAddressRepository.Get(r=>r.AddressID == 2).FirstOrDefault().StreetAddress1;
            Assert.IsTrue(newAddr == oldAddr);
        }
    }
}
