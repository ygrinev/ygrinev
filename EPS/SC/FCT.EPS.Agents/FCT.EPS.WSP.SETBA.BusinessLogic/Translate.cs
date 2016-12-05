using FCT.EPS.DataEntities;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.SETBA.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.SETBA.BusinessLogic
{
    public class FooterValues
    {
        public int RecordType { get; set; }
        public string ClientNumber { get; set; }
        public string TransmitID { get; set; }
        public int TotalNumberOfBalanceTransfers { get; set; }
        public int TotalNumberOfFastCashRequests { get; set; }
        public int TotalRecords { get; set; }
    }

    public class HeaderValues
    {
        public int RecordType { get; set; }
        public string ClientNumber { get; set; }
        public string TransmitID { get; set; }
        public DateTime TransmissionDate { get; set; }
        public int SequenceNumber { get; set; }
    }

    internal class Translate
    {
        internal static FileSerializer.RBC.RBCRemittanceBodyDataValues CreateRBCMessageBodyRecord(DataEntities.tblPaymentRequest currenttblPaymentRequest, DataEntities.tblPayeeInfo payeeInfo)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            return new FileSerializer.RBC.RBCRemittanceBodyDataValues()
            {
                RecordType = AgentConstants.Misc.RBC_BODY_RECORD_TYPE,
                AccountNumber = currenttblPaymentRequest.PayeeReferenceNumber,
                PrimaryName = "FCT",
                CCIN = payeeInfo.tblCreditorList.CCIN.ToString(),
                PayeeName = currenttblPaymentRequest.PayeeName,
                CompanyID = payeeInfo.tblCreditorList.CompanyID,
                PayeeAddress1 = currenttblPaymentRequest.tblPaymentAddress_PayeeAddress.StreetAddress1,
                PayeeCity = currenttblPaymentRequest.tblPaymentAddress_PayeeAddress.City,
                PayeeStateProvince = currenttblPaymentRequest.tblPaymentAddress_PayeeAddress.ProvinceCode,
                PayeePostalCode = currenttblPaymentRequest.tblPaymentAddress_PayeeAddress.PostalCode,
                PayeeCountry = currenttblPaymentRequest.tblPaymentAddress_PayeeAddress.Country,
                PayeeAccountNumber = string.IsNullOrEmpty(currenttblPaymentRequest.TokenNumber) ? currenttblPaymentRequest.PayeeReferenceNumber : TokenizerSerializer.DeTokenize(currenttblPaymentRequest.TokenNumber),
                BalanceTransferAmount = (int)(currenttblPaymentRequest.PaymentAmount*100),
                RequestedDate = currenttblPaymentRequest.PaymentRequestDate,
                PaymentReferenceNumber = currenttblPaymentRequest.PaymentTransactionID.ToString()
            };
        }
        internal static FileSerializer.RBC.RBCRemittanceFooterDataValues CreateRBCMessageFooterRecord(FooterValues passedFooterValues)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            SolutionTraceClass.WriteLineVerbose("End");
            return new FileSerializer.RBC.RBCRemittanceFooterDataValues()
            {
                RecordType = passedFooterValues.RecordType,
                ClientNumber = passedFooterValues.ClientNumber,
                TransmitID = passedFooterValues.TransmitID,
                TotalNumberOfBalanceTransfers = passedFooterValues.TotalNumberOfBalanceTransfers,
                TotalNumberOfFastCashRequests = passedFooterValues.TotalNumberOfFastCashRequests,
                TotalRecords = passedFooterValues.TotalRecords
            };
        }
        internal static FileSerializer.RBC.RBCRemittanceHeaderDataValues CreateRBCMessageHeaderRecord(HeaderValues passedHeaderValues)
        {
            SolutionTraceClass.WriteLineVerbose("Start/End");
            return new FileSerializer.RBC.RBCRemittanceHeaderDataValues()
            {
                RecordType = passedHeaderValues.RecordType,
                ClientNumber = passedHeaderValues.ClientNumber,
                TransmitID = passedHeaderValues.TransmitID,
                TransmissionDate = passedHeaderValues.TransmissionDate,
                SequenceNumber = passedHeaderValues.SequenceNumber
            };
        }


        internal static tblPaymentRequest CreateBPStblPaymentRequest(tblPayeeInfo mytblPayeeInfo, decimal passedAmount,string passedSubscriptionID, IList<tblPaymentRequest> passedSuccesfullRecords)
        {
            return new tblPaymentRequest()
            {
                SubscriptionID = passedSubscriptionID,
                FCTReferenceNumber = "BPSSwift",
                FCTURNShort = "",
                DisbursementRequestID = "BPSSwift",
                PaymentAmount = passedAmount,
                PaymentMethod = "Wire",
                PaymentRequestDate = DateTime.Now,
                PaymentRequestType = "Single",
                PayeeName = mytblPayeeInfo.PayeeName,
                PayeeBankAccountHolderName = mytblPayeeInfo.BankAccountHolderName,
                PayeeBankName = mytblPayeeInfo.tblPayeeAccount.BankName,
                PayeeBankNumber = mytblPayeeInfo.tblPayeeAccount.BankNumber,
                PayeeTransitNumber = mytblPayeeInfo.tblPayeeAccount.TransitNumber,
                PayeeAccountNumber = mytblPayeeInfo.tblPayeeAccount.AccountNumber,
                PayeeSWIFTBIC = mytblPayeeInfo.tblPayeeAccount.BankSWIFTCode,
                PayeeCanadianClearingCode = mytblPayeeInfo.tblPayeeAccount.CanadianClearingCode,
                RequestUsername = Environment.UserName,
                RequestClientIP = Utils.GetLocalIPs(),
                PayeeReferenceNumber = null,
                tblPaymentAddress_PayeeBranchAddress = CreatetblPaymentAddressFromtblAddress(mytblPayeeInfo.tblPayeeAccount.tblAddress),
                tblPaymentAddress_PayeeAddress =CreatetblPaymentAddressFromtblAddress( mytblPayeeInfo.tblAddress),
                PaymentTransactionID = null,
                PayeeContact = mytblPayeeInfo.PayeeContact,
                PayeeContactPhoneNumber = mytblPayeeInfo.PayeeContactPhoneNumber,
                PayeeContactEmailAddress = mytblPayeeInfo.PayeeEmail,
                LastModifiedDate = DateTime.Now,
                PayeeInfoID = mytblPayeeInfo.PayeeInfoID,
                DebtorName = "",
                TokenNumber = "",
                BPSWithWirePayment = null,
                tblChildPaymentRequest = passedSuccesfullRecords
            };
        }

        private static tblPaymentAddress CreatetblPaymentAddressFromtblAddress(tblAddress passedtblAddress)
        {
            return new tblPaymentAddress()
            {
                City = passedtblAddress.City,
                Country = passedtblAddress.Country,
                PostalCode = passedtblAddress.PostalCode,
                Province = passedtblAddress.Province,
                ProvinceCode = passedtblAddress.ProvinceCode,
                StreetAddress1 = passedtblAddress.StreetAddress1,
                StreetAddress2 = passedtblAddress.StreetAddress2,
                StreetNumber = passedtblAddress.StreetNumber,
                UnitNumber = passedtblAddress.UnitNumber
            };
        }
    }
}
