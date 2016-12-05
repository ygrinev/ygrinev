using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic.Interfaces
{
    public interface IPayoutLetterBusinessLogic
    {
        void CreatePayoutLetter(CreatePayoutLetterRequest request);
        GetPayoutLetterDateResponse GetPayoutLetterDate(GetPayoutLetterDateRequest request);
        GetPayoutLettersWorklistResponse GetPayoutLettersWorklist(GetPayoutLettersWorklistRequest request);

        void AssignFundingDeal(AssignFundingDealRequest request);

        void SavePayoutSentDate(SavePayoutSentDateRequest request);

    }
}
