using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
   public sealed class DisbursementSummaryMapper:IEntityMapper<tblDisbursementSummary, DisbursementSummary>
   {
       public DisbursementSummary MapToData(tblDisbursementSummary tEntity, object parameters = null)
       {
           DisbursementSummary data;
           if (tEntity != null)
           {
               data = new DisbursementSummary()
               {
                   RequiredDepositAmount = tEntity.DepositAmountRequired,
                   FundingDealID = tEntity.FundingDealID,
                   DisbursementSummaryID = tEntity.DisbursementSummaryID,
                   Version = tEntity.Version,
                   Comments = tEntity.Comments
               };
               return data;
           }
           if (parameters != null)
           {
               var milestones = parameters as vw_EFDisbursementSummary;
               if (milestones != null)
               {
                   data=new DisbursementSummary
                   {
                       RequiredDepositAmount = milestones.DepositAmountRequired.GetValueOrDefault(),
                       FundingDealID = milestones.FundingDealID,
                       DisbursementSummaryID = milestones.DisbursementSummaryID.GetValueOrDefault(),
                       DepositAmountReceived = milestones.DepositAmountReceived.GetValueOrDefault(),
                       WireDepositDetails = milestones.WireDepositDetails,
                       Version = milestones.Version,
                       Comments = milestones.PayoutComments,
                       FundingMilestone = new FundingMilestone()
                       {
                           Disbursed = milestones.Disbursed.GetValueOrDefault(),
                           Funded = milestones.Funded.GetValueOrDefault(),
                           SignedByPurchaser = milestones.SignedByPurchaser.GetValueOrDefault(),
                           SignedByVendor = milestones.SignedByVendor.GetValueOrDefault(),
                           InvitationAccepted = milestones.InvitationAccepted.GetValueOrDefault(),
                           InvitationSent = milestones.InvitationSent.GetValueOrDefault(),
                           PayoutSent = milestones.PayoutSent.GetValueOrDefault(),
                           SignedByPurchaserName = milestones.SignedByPurchaserName,
                           SignedByVendorName = milestones.SignedByVendorName
                       },

                   };
                   return data;
               }
           }
           return null;
       }

       public tblDisbursementSummary MapToEntity(DisbursementSummary tData)
       {
           if (tData != null)
           {
               var entity = new tblDisbursementSummary()
               {
                   DepositAmountRequired = tData.RequiredDepositAmount,
                   DisbursementSummaryID = tData.DisbursementSummaryID,
                   FundingDealID = tData.FundingDealID,
                   Version = tData.Version,
                   Comments = tData.Comments
               };
               return entity;
           }
           return null;
       }
   }
}
