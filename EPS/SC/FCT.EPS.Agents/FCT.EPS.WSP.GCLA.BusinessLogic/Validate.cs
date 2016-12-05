using FCT.EPS.FileSerializer.RBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.GCLA.BusinessLogic
{
    public class Validate
    {
        public static IList<string> ValidateData(RBCPayeeListBodyDataValues passedRBCPayeeListBodyDataValues)
        {
            List<string> myErrors = new List<string>();

            if (passedRBCPayeeListBodyDataValues.LanguageIndicator.ToUpper() != "F" && passedRBCPayeeListBodyDataValues.LanguageIndicator.ToUpper()  != "E")
                myErrors.Add("Language indicator is not a 'F' or 'E'.");
            if (passedRBCPayeeListBodyDataValues.CurrentRecord.ToUpper() != "Y" && passedRBCPayeeListBodyDataValues.CurrentRecord.ToUpper() != "N")
                myErrors.Add("Curent Record is not a 'Y' or 'N'.");
            if (passedRBCPayeeListBodyDataValues.CreditorType.ToUpper() != "CR" && passedRBCPayeeListBodyDataValues.CreditorType.ToUpper() != "VSA")
                myErrors.Add("Curent Record is not a 'Y' or 'N'.");

            return myErrors;
        }
    }
}
