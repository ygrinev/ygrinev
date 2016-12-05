using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.GSMA.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCT.EPS.WSP.GSMA.Resources;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
namespace FCT.EPS.WSP.GSMA.BusinessLogic.Tests
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


            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create(), false);
            //ExceptionPolicyFactory exceptionFactory = new ExceptionPolicyFactory(configurationSource);
            //ExceptionPolicy.SetExceptionManager(exceptionFactory.CreateManager());
        }

        [TestMethod()]
        public void BusinessLayer_ProcessSwiftFilesStatusTest()
        {
            FileLocations SwiftFileLocations = new FileLocations();


            SwiftFileLocations.SwiftArchiveConverterErrorLocation = @"\\fspricdsg.prefirstcdn.com\EASYFUND\FCT\Archive\ACK_NAK_Error";
            SwiftFileLocations.SwiftConverterErrorLocation = @"\\fspricdsg.prefirstcdn.com\EASYFUND\FCT\Error\Converter";
            //SwiftFileLocations.SwiftAckNackLocation = @"\\fspricdsg.prefirstcdn.com\EASYFUND\FCT\Result";
            SwiftFileLocations.SwiftAckNackLocation = @"C:\wrkdir\SwiftAck";
            SwiftFileLocations.SwiftArchiveAckNackLocation = @"C:\wrkdir\Archive";
            SwiftFileLocations.SwiftArchiveAutoClientErrorLocation = @"\\fspricdsg.prefirstcdn.com\EASYFUND\FCT\Archive\ACK_NAK_Error";
            SwiftFileLocations.SwiftAutoClientErrorLocation = @"\\fspricdsg.prefirstcdn.com\EASYFUND\FCT\Error\AutoClient";
            //SwiftFileLocations.SwiftCreditLocation = @"\\fspricdsg.prefirstcdn.com\EASYFUND\FCT\Reception";
            SwiftFileLocations.SwiftCreditLocation = @"C:\wrkdir\EASYFUND\FCT\Reception";
            //SwiftFileLocations.SwiftDebitLocation = @"\\fspricdsg.prefirstcdn.com\EASYFUND\FCT\Reception";
            SwiftFileLocations.SwiftDebitLocation = SwiftFileLocations.SwiftCreditLocation;
            //SwiftFileLocations.SwiftArchiveCreditLocation = @"\\fspricdsg.prefirstcdn.com\EASYFUND\FCT\Archive\Reception";
            SwiftFileLocations.SwiftArchiveCreditLocation = @"C:\wrkdir\EASYFUND\FCT\Archive\Reception";
            //SwiftFileLocations.SwiftArchiveDebitLocation = @"\\fspricdsg.prefirstcdn.com\EASYFUND\FCT\Archive\Reception";
            SwiftFileLocations.SwiftArchiveDebitLocation = SwiftFileLocations.SwiftArchiveCreditLocation;


            BusinessLayer.ProcessSwiftFilesStatus(SwiftFileLocations);
        }
    }
}
