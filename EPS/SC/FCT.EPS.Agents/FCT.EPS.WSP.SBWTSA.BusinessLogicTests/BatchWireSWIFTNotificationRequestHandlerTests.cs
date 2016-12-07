﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.SBWTSA.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
namespace FCT.EPS.WSP.SBWTSA.BusinessLogic.Tests
{
    [TestClass()]
    public class BatchWireSWIFTNotificationRequestHandlerTests
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
        public void CreateWireSWIFTBatchTest()
        {
            BatchWireSWIFTNotificationRequestHandler.CreateWireSWIFTBatch(300);
        }

        [TestMethod()]
        public void SubmitWireRequestToSWIFTTest()
        {
            BatchWireSWIFTNotificationRequestHandler.SubmitWireRequestToSWIFT(5, 10, @"c:\wrkdir");
        }
    }
}