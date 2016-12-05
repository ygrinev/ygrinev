using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;
using UserType = FCT.LLC.Common.DataContracts.UserType;

namespace FCT.LLC.BusinessService.BusinessLogic
{
   public class CancelDealHelper:ICancelDealHelper
   {
       private readonly IEmailHelper _emailHelper;
       private readonly ILawyerRepository _lawyerRepository;
       private readonly IFundingDealRepository _fundingDealRepository;
       public CancelDealHelper(IEmailHelper emailHelper, ILawyerRepository lawyerRepository, IFundingDealRepository fundingDealRepository)
       {
           _emailHelper = emailHelper;
           _fundingDealRepository = fundingDealRepository;
           _lawyerRepository = lawyerRepository;
       }
       public void NotifyLawyer(FundingDeal fundingDeal, string recipient)
       {
           KeyValuePair<string, string>? mailingList = null;
           LawyerProfile recipientProfile = null;
           switch (recipient)
           {
               case LawyerActingFor.Vendor:
                   if (fundingDeal.ActingFor == LawyerActingFor.Vendor)
                   {
                       recipientProfile = _lawyerRepository.GetNotificationDetails(fundingDeal.Lawyer.LawyerID);
                       mailingList = _emailHelper.GetEmailRecipientList(fundingDeal);
                   }
                   else if (fundingDeal.ActingFor == LawyerActingFor.Purchaser)
                   {
                       int otherDealID = fundingDeal.DealStatus == DealStatus.Cancelled
                           ? _fundingDealRepository.GetOtherCancelledDealInScope(fundingDeal.DealID.GetValueOrDefault())
                           : _fundingDealRepository.GetOtherDealInScope(fundingDeal.DealID.GetValueOrDefault());

                       recipientProfile = _lawyerRepository.GetNotificationDetails(fundingDeal.OtherLawyer.LawyerID);
                       mailingList = _emailHelper.GetEmailRecipientList(fundingDeal, otherDealID);
                   }                   
                   break;

               case LawyerActingFor.Purchaser:
                   if (fundingDeal.ActingFor == LawyerActingFor.Vendor)
                   {
                       int otherDealID = fundingDeal.DealStatus == DealStatus.Cancelled
                          ? _fundingDealRepository.GetOtherCancelledDealInScope(fundingDeal.DealID.GetValueOrDefault())
                          : _fundingDealRepository.GetOtherDealInScope(fundingDeal.DealID.GetValueOrDefault());

                       recipientProfile = _lawyerRepository.GetNotificationDetails(fundingDeal.OtherLawyer.LawyerID);
                       mailingList = _emailHelper.GetEmailRecipientList(fundingDeal, otherDealID);
                   }
                   else if (fundingDeal.ActingFor == LawyerActingFor.Purchaser)
                   {
                       recipientProfile = _lawyerRepository.GetNotificationDetails(fundingDeal.Lawyer.LawyerID);
                       mailingList = _emailHelper.GetEmailRecipientList(fundingDeal);
                   }
                   break;

               case LawyerActingFor.Both:
               case LawyerActingFor.Mortgagor:
                   recipientProfile = _lawyerRepository.GetNotificationDetails(fundingDeal.Lawyer.LawyerID);
                       mailingList = _emailHelper.GetEmailRecipientList(fundingDeal);
                   break;
                 
           }
           if (!string.IsNullOrEmpty(mailingList.GetValueOrDefault().Value))
           {
               recipientProfile.Email = mailingList.GetValueOrDefault().Value;

               _emailHelper.SendStandardNotification(fundingDeal, recipientProfile, recipient,
                   StandardNotificationKey.EFDealCancelled);
           }
       }

      public static string GetRecipientActingFor(FundingDeal currentFundingDeal, Lookup otherDealDetails)
       {
           string recipientActingFor = string.Empty;
          if (otherDealDetails!=null)
          {
              var otherDealStatus = otherDealDetails.Value;
              if (otherDealStatus == DealStatus.SystemDraft || otherDealStatus == DealStatus.Declined)
              {
                  recipientActingFor = currentFundingDeal.ActingFor;
              }
              else
              {
                  switch (currentFundingDeal.ActingFor)
                  {
                      case LawyerActingFor.Vendor:
                          recipientActingFor = LawyerActingFor.Purchaser;
                          break;
                      case LawyerActingFor.Purchaser:
                          recipientActingFor = LawyerActingFor.Vendor;
                          break;
                  }
              }
             
          }
          else
          {
              recipientActingFor = currentFundingDeal.ActingFor; 
          }
        
           return recipientActingFor;
       }

       internal static bool NotificationRequired(FundingDeal deal, string usertype)
       {
           if (!IsLawyerOrClerkOrAssistant(usertype))
           {
               return true;
           }
           if ((deal.ActingFor==LawyerActingFor.Both || deal.ActingFor == LawyerActingFor.Mortgagor))
           {
               return false;
           }
           if (deal.ActingFor == LawyerActingFor.Purchaser || deal.ActingFor == LawyerActingFor.Vendor)
           {
               if (deal.DealStatus != DealStatus.Active && deal.DealStatus!=DealStatus.New)
               {
                   return false;
               }
           }
           return true;
       }

       internal static bool IsLawyerOrClerkOrAssistant(string userType)
       {
           bool IsLawyer = userType.Equals(UserType.Lawyer, StringComparison.CurrentCultureIgnoreCase)
               || userType.Equals(UserType.Clerk, StringComparison.CurrentCultureIgnoreCase)
               || userType.Equals(UserType.Assistant, StringComparison.CurrentCultureIgnoreCase);
           return IsLawyer;
       }

       internal static bool OtherLawyerNotificationRequired(Lookup otherDealDetails, string usertype)
       {
           if (otherDealDetails == null)
           {
               return false;
           }
           return Convert.ToInt32(otherDealDetails.Key) > 0 && !IsLawyerOrClerkOrAssistant(usertype) &&
                  otherDealDetails.Value != DealStatus.SystemDraft && otherDealDetails.Value != DealStatus.Declined;
       }
    }
}
