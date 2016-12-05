using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;
using FCT.Services.LIM.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IVendorLawyerHelper
    {
        void UpdateVendorLawyerDisbursement(Lawyer vendorLawyer, int dealId, UserContext userContext, string currentActingFor,
            string oldActingFor);

        bool AssignActiveExistingTrustAccount(Lawyer vendorLawyer, Disbursement disbursement);

        void ClearVendorLawyerDisbursement(IEnumerable<Disbursement> disbursements);

        DisbursementCollection AdjustVendorLawyerDisbursement(DisbursementCollection disbursements, Fee fee,
            string feeSplit);

        UserProfile GetUserProfile(string userName);

        void SyncAnyLawyerActingForChanges(FundingDeal currentDeal, string oldActingFor);


    }
}
