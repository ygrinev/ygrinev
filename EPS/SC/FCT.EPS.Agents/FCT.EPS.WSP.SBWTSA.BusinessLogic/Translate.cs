using FCT.EPS.WSP.SBWTSA.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.SBWTSA.BusinessLogic
{
    class Translate
    {
        internal static Swift.MT101DataValues EPSDataToMT101DataValues(Grouping myPaymentObject, DataEntities.tblPaymentRequest myFirsttblPaymentRequest)
        {
            return new Swift.MT101DataValues()
            {
                BeneficiaryName = myFirsttblPaymentRequest.PayeeBankAccountHolderName,
                BeneficiaryStreet =
                    myFirsttblPaymentRequest.tblPaymentAddress_PayeeAddress.UnitNumber
                    + "," + myFirsttblPaymentRequest.tblPaymentAddress_PayeeAddress.StreetNumber
                    + "," + myFirsttblPaymentRequest.tblPaymentAddress_PayeeAddress.StreetAddress1
                    + "," + myFirsttblPaymentRequest.tblPaymentAddress_PayeeAddress.StreetAddress2,
                BeneficiaryCity = myFirsttblPaymentRequest.tblPaymentAddress_PayeeAddress.City,
                BeneficiaryProvince_State = myFirsttblPaymentRequest.tblPaymentAddress_PayeeAddress.ProvinceCode,
                BeneficiaryCountry = myFirsttblPaymentRequest.tblPaymentAddress_PayeeAddress.Country,
                BeneficiaryAccount = myFirsttblPaymentRequest.PayeeTransitNumber + myFirsttblPaymentRequest.PayeeAccountNumber,
                BeneficiaryBankName = myFirsttblPaymentRequest.PayeeBankName,
                BeneficiaryBankStreet =
                    myFirsttblPaymentRequest.tblPaymentAddress_PayeeBranchAddress.UnitNumber
                    + "," + myFirsttblPaymentRequest.tblPaymentAddress_PayeeBranchAddress.StreetNumber
                    + "," + myFirsttblPaymentRequest.tblPaymentAddress_PayeeBranchAddress.StreetAddress1
                    + "," + myFirsttblPaymentRequest.tblPaymentAddress_PayeeBranchAddress.StreetAddress2,
                BeneficiaryBankCity = myFirsttblPaymentRequest.tblPaymentAddress_PayeeBranchAddress.City,
                BeneficiaryBankProvince_State = myFirsttblPaymentRequest.tblPaymentAddress_PayeeBranchAddress.ProvinceCode,
                BeneficiaryBankCountry = myFirsttblPaymentRequest.tblPaymentAddress_PayeeBranchAddress.Country,
                BeneficiaryBankId = myFirsttblPaymentRequest.PayeeBankNumber,
                YourReference = myPaymentObject.PaymentTransactionID.ToString(),
                PaymentDueDate = myFirsttblPaymentRequest.PaymentRequestDate,
                PaymentCurrency = AgentConstants.Misc.PAYMENT_CURRENCY,
                PaymentAmount = myPaymentObject.PaymentAmount,
                SettlementAccount = myFirsttblPaymentRequest.tblSolutionSubscription.tblFCTAccount.TransitNumber + myFirsttblPaymentRequest.tblSolutionSubscription.tblFCTAccount.AccountNumber,
                BeneficiaryInstructions = myFirsttblPaymentRequest.PayeeReferenceNumber,
                ReasonforPayment = string.IsNullOrWhiteSpace(myFirsttblPaymentRequest.PayeeCanadianClearingCode) ? AgentConstants.Misc.CANADIAN_CLEARING_CODE_PREFIX + myFirsttblPaymentRequest.PayeeBankNumber + myFirsttblPaymentRequest.PayeeTransitNumber : myFirsttblPaymentRequest.PayeeCanadianClearingCode,

                //Needed for finance but not swift
                ContractType = AgentConstants.Misc.CONTRACT_TYPE,
                SettlementCurrency = AgentConstants.Misc.PAYMENT_CURRENCY,
                PayeeType = AgentConstants.Misc.PAYEE_TYPE,
                PaymentDestination = myFirsttblPaymentRequest.PayeeBankNumber == AgentConstants.Misc.TD_BANK_NUMBER ? AgentConstants.Misc.TD_PAYMENT_DESTINATION : AgentConstants.Misc.OTHER_PAYMENT_DESTINATION
            };
        }
    }
}
