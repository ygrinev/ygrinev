using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class FundedDealMapper:IEntityMapper<tblFundingDeal, FundedDeal>
    {
        public FundedDeal MapToData(tblFundingDeal tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var data = new FundedDeal()
                {
                    DealScopeId = tEntity.DealScopeID,
                    FundingDealId = tEntity.FundingDealID,
                    AssignedTo = tEntity.AssignedTo,
                    Milestone =
                    {
                        PayoutSent = tEntity.PayoutSent,
                        Disbursed = tEntity.Disbursed,
                        InvitationAccepted = tEntity.InvitationAccepted,
                        InvitationSent = tEntity.InvitationSent,
                        Funded = tEntity.Funded,
                        SignedByPurchaser = tEntity.SignedByPurchaser,
                        SignedByVendor = tEntity.SignedByVendor,
                        SignedByPurchaserName = tEntity.SignedByPurchaserName,
                        SignedByVendorName = tEntity.SignedByVendorName
                    },

                };
                return data;
            }
           return null;
        }

        public tblFundingDeal MapToEntity(FundedDeal tData)
        {
            if (tData != null)
            {
                var entity = new tblFundingDeal()
                {
                    PayoutSent = tData.Milestone.PayoutSent,
                    DealScopeID = tData.DealScopeId,
                    Disbursed = tData.Milestone.Disbursed,
                    Funded = tData.Milestone.Funded,
                    FundingDealID = tData.FundingDealId,
                    InvitationAccepted = tData.Milestone.InvitationAccepted,
                    InvitationSent = tData.Milestone.InvitationSent,
                    SignedByPurchaser = tData.Milestone.SignedByPurchaser,
                    SignedByVendor = tData.Milestone.SignedByVendor,
                    AssignedTo = tData.AssignedTo,
                    SignedByVendorName = tData.Milestone.SignedByVendorName,
                    SignedByPurchaserName = tData.Milestone.SignedByPurchaserName
                };
                return entity;
            }
            return null;
        }
    }
}
