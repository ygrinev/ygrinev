using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.SETBA.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.ExternalResources;
namespace FCT.EPS.WSP.SETBA.BusinessLogic.Tests
{
    [TestClass()]
    public class SendElectronicToBankHandlerTests
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
        public void SubmitElectronicRequestToBankTest()
        {
            SendElectronicToBankHandler mySendElectronicToBankHandler = new SendElectronicToBankHandler();
            mySendElectronicToBankHandler.CreateElectronicRequest(5, @"c:\wrkdir",18000,new DateTime(2015,08,07,9,00,00), "RoutingHeader");
        }

        [TestMethod()]
        public void SubmitElectronicFilesTest()
        {
            SendElectronicToBankHandler mySendElectronicToBankHandler = new SendElectronicToBankHandler();
            mySendElectronicToBankHandler.SubmitElectronicFiles(5, @"c:\wrkdir", @"c:\wrkdir2", @"c:\wrkdir2", new EmailItems() { EmailAddress = "imukherjee@fct.com", EmailSubject = "RBC BPS Payment Details report ready", EmailBody = "The RBC BPS Payment Details report {0} is located in {1} for your review." }, new EmailItems() { EmailAddress = "imukherjee@fct.com", EmailSubject = "RBC Electronic Payment sent", EmailBody = "The RBC electronic Payment file has been sent to RBC.  A copy of the file is located at {0}" });
        }

        [TestMethod()]
        public void CreateElectronicTransactionsTest()
        {
            SendElectronicToBankHandler mySendElectronicToBankHandler = new SendElectronicToBankHandler();
            mySendElectronicToBankHandler.CreateElectronicTransactions();
        }

        [TestMethod()]
        public void SendEmailTest()
        {
            SendElectronicToBankHandler mySendElectronicToBankHandler = new SendElectronicToBankHandler();
            //Must make this method public in order to test it.
            //string jobid = mySendElectronicToBankHandler.SendEmail("pbinnell@fct.ca", "RBC BPS Payment Details report ready", "The RBC BPS Payment Details report {0} is located in {1} for your review.");
            SystemServiceWrapper mySystemServiceWrapper = new SystemServiceWrapper();
            //mySystemServiceWrapper.GetJobStatus(jobid);
            
        }

    }
}
