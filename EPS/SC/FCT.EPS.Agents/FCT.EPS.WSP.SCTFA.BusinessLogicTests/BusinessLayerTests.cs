using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.SCTFA.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
namespace FCT.EPS.WSP.SCTFA.BusinessLogic.Tests
{
    [TestClass()]
    public class BusinessLayerTests
    {
        [ExcludeFromCodeCoverage, TestInitialize()]
        public void Initialize()
        {
            //EPSDBContext myEPSPaymentDBContext = new EPSDBContext();
            //myEPSPaymentDBContext.Database.Delete();
            //myEPSPaymentDBContext.Database.Create();
            //myEPSPaymentDBContext.Database.ExecuteSqlCommand("CREATE PROCEDURE [dbo].[spGetFCTFeePaymentRequest] @PaymentRequestID int = NULL,  @NumberOfRecords int = 5 AS BEGIN SET NOCOUNT ON; DECLARE @RecordsToProcess  TABLE ( [PaymentRequestID] INT PRIMARY KEY ); INSERT INTO @RecordsToProcess SELECT TOP (@NumberOfRecords) tblPaymentRequest.PaymentRequestID FROM tblPaymentRequest WITH (READPAST,UPDLOCK) join tblFCTFeeSummaryRequest ON tblFCTFeeSummaryRequest.PaymentRequestID = tblPaymentRequest.PaymentRequestID WHERE  tblPaymentRequest.PaymentRequestID > COALESCE(@PaymentRequestID,0) AND PaymentStatusID = 1 AND PaymentMethod = 'FCTFee' AND PaymentTransactionID IS NOT NULL UPDATE tblPaymentRequest SET PaymentStatusID = 2 WHERE PaymentRequestID IN ( SELECT PaymentRequestID FROM @RecordsToProcess ) SELECT [PaymentRequestID],[SubscriptionID],[FCTReferenceNumber],[FCTURNShort],[DisbursementRequestID],[PaymentAmount],[PaymentMethod],[PaymentRequestDate],[PaymentRequestType],[PayeeName],[PayeeBankName],[PayeeBankNumber],[PayeeTransitNumber],[PayeeAccountNumber],[PayeeSWIFTBIC],[PayeeCanadianClearingCode],[RequestUsername],[RequestClientIP],[PaymentStatusID],[PayeeReferenceNumber],[PayeeBranchAddressID],[PayeeAddressID],[PaymentTransactionID],[LastModifiedDate] FROM tblPaymentRequest WHERE PaymentRequestID IN(SELECT PaymentRequestID FROM @RecordsToProcess) END");


            //DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create(), false);
            //ExceptionPolicyFactory exceptionFactory = new ExceptionPolicyFactory(configurationSource);
            //ExceptionPolicy.SetExceptionManager(exceptionFactory.CreateManager());
        }

        [TestMethod()]
        public void SendChequesToFinanceTest()
        {
            BusinessLayer.StartProcess(5, "EPSNetTcpEndpoint", 5);
        }

        //[TestMethod()]
        //public void TestStringCut()
        //{

        //    string[] TestStrings = new string[] { "this only", "this,is,a,test, of, the, standard, string, lenght, with, commas, daa, daa, daa, doo, doo", "LastName1ChequePaymentMethodLastName1Sam,LastName2ChequePaymentMethodLastNameSam" };

        //    foreach (string Temp in TestStrings)
        //    {
        //        string TestString = Temp;
        //        string AddressLine2 = string.Empty;
        //        string AddressLine3 = string.Empty;
        //        int firstLineCutoff = TestString.LastIndexOf(',', TestString.Length > 39 ? 40 : TestString.Length - 1);
        //        if (firstLineCutoff == -1)
        //        {
        //            AddressLine2 = TestString;
        //        }
        //        else
        //        {
        //            AddressLine2 = TestString.Substring(0, firstLineCutoff);
        //            TestString = TestString.Remove(0, firstLineCutoff);
        //            if (TestString.Substring(0, 1) == ",")
        //            {
        //                TestString = TestString.Substring(1, TestString.Length - 1).Trim();
        //            }
        //            int secondLineCutoff = TestString.LastIndexOf(',', TestString.Length > 39 ? 40 : TestString.Length - 1);
        //            if (secondLineCutoff == -1)
        //            {
        //                AddressLine3 = TestString;
        //            }
        //            else
        //            {
        //                AddressLine3 = TestString.Substring(0, secondLineCutoff);
        //            }
                    
        //        }
        //    }
        //}
    }
}
