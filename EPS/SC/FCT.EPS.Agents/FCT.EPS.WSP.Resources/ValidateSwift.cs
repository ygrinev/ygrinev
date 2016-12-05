using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.Resources
{
    public class ValidateSwift
    {
        static public bool IsValid(Swift.MT101DataValues passedSwiftMessage, out string invalidFields)
        {
            bool isValid = true;
            var missingFieldsList = new List<string>();

            if (string.IsNullOrWhiteSpace(passedSwiftMessage.BeneficiaryName))
                missingFieldsList.Add("Beneficiary Name");

            if (string.IsNullOrWhiteSpace(passedSwiftMessage.BeneficiaryStreet))
                missingFieldsList.Add("Beneficiary Street");

            if (string.IsNullOrWhiteSpace(passedSwiftMessage.BeneficiaryCity))
                missingFieldsList.Add("Beneficiary City");

            if (string.IsNullOrWhiteSpace(passedSwiftMessage.BeneficiaryProvince_State))
                missingFieldsList.Add("Beneficiary Province/State");

            if (string.IsNullOrWhiteSpace(passedSwiftMessage.BeneficiaryCountry))
                missingFieldsList.Add("Beneficiary Country");

            if (string.IsNullOrWhiteSpace(passedSwiftMessage.BeneficiaryAccount))
                missingFieldsList.Add("Beneficiary Account");

            if (string.IsNullOrWhiteSpace(passedSwiftMessage.YourReference))
                missingFieldsList.Add("Your Reference");

            if(string.IsNullOrWhiteSpace(passedSwiftMessage.ReasonforPayment))
                missingFieldsList.Add("Reason for Payment");

            //if (DateTime.IsNullOrWhiteSpace(passedSwiftMessage.PaymentDueDate))
            //    missingFieldsList.Add("Payment Due Date");

            if (string.IsNullOrWhiteSpace(passedSwiftMessage.PaymentCurrency))
                missingFieldsList.Add("Payment Currency");

            if (passedSwiftMessage.PaymentAmount <= 0)
                missingFieldsList.Add("Payment Amount");

            isValid = missingFieldsList.Count == 0;

            invalidFields = isValid ? string.Empty : string.Join(",", missingFieldsList);

            return isValid;
        }
    }
}
