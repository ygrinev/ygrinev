using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using FCT.LLC.BusinessService.BusinessLogic.FCTURNReference;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Client;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;


namespace FCT.LLC.BusinessService.BusinessLogic
{
    internal class FundingBusinessLogicHelper
    {
        private const string SourceSystemNumber = "SourceSystemNumber";

        private const string DBContextName = "EFBusinessContext";

        internal static void SendNotificationEmail(string party, string fctURN, string lawyerlanguage, string mailingList,
            FundingDeal requestDeal)
        {
            var tokens =EmailHelper.CreateEmailTokens(party, FCTURNHelper.FormatShortFCTURN(fctURN), requestDeal);
            var connection = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings[DBContextName].ConnectionString
            };

            EasyFundDispatcher.SendDealNotificationEmailToSpecificEmailId(connection,
                lawyerlanguage, StandardNotificationKey.EFLawyerToLawyer, tokens, mailingList);
        }

    

        internal static string GetFCTURN()
        {
            using (var client = new UniqueReferenceNumberSoapClient())
            {
                long sourceSystemNumber = Convert.ToInt64(ConfigurationManager.AppSettings[SourceSystemNumber]);
                return client.GetUniqueReferenceNumber(sourceSystemNumber);
            }
        }

        internal static string GetShortFCTURN(string fctURNLong)
        {

            if (string.IsNullOrEmpty(fctURNLong)) return string.Empty;

            string fctURNShort = fctURNLong;

            int first5digits = Convert.ToInt16(fctURNLong.Substring(0, 5)); //Grab first 5 digits
            string remainingDigits = fctURNLong.Substring(5).TrimStart('0'); // Removes leading 0's from the remaining digits ie., after 5th character

            fctURNShort = first5digits + remainingDigits; // Format first 5 digits and concatenate the remainig with -

            return fctURNShort;
        }

        internal static bool IsValidDealForUpdate(string dealstatus)
        {
            if (dealstatus.ToUpper() == DealStatus.Cancelled || dealstatus.ToUpper() == DealStatus.CancelRequest ||
                dealstatus.ToUpper() == DealStatus.Declined)
            {
                return false;
            }
            return true;
        }

        internal static bool ReassignedToSingleLawyer(string oldActingFor, string newActingFor)
        {
            if (newActingFor != oldActingFor)
            {
                if (newActingFor == LawyerActingFor.Both || newActingFor == LawyerActingFor.Mortgagor)
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool MortgagorSyncRequired(string actingFor, string dealStatus)
        {
            if (actingFor == LawyerActingFor.Vendor)
            {
                return true;
            }
            if (actingFor == LawyerActingFor.Purchaser && dealStatus == DealStatus.SystemDraft)
            {
                return true;
            }
            return false;
        }

        internal static bool ComboDealStatusChangeRequired(FundingDeal requestDeal, FundedDeal milestonesForDeal,
            string otherDealStatus)
        {
            return (requestDeal.BusinessModel == BusinessModel.LLCCOMBO || requestDeal.BusinessModel == BusinessModel.MMSCOMBO) &&
                   otherDealStatus != requestDeal.OtherLawyerDealStatus &&
                   (milestonesForDeal.Milestone.InvitationAccepted <= DateTime.MinValue ||
                    milestonesForDeal.Milestone.InvitationAccepted == null);
        }

        internal static void LogHistoryForNewDeal(FundingDeal requestDeal, List<UserHistory> dealhistories, int otherDealId)
        {
            string otherLawyerName = string.Format("{0} {1}", requestDeal.OtherLawyer.FirstName,
                requestDeal.OtherLawyer.LastName);
            string lawyerName = string.Format("{0} {1}", requestDeal.Lawyer.FirstName,
                requestDeal.Lawyer.LastName);
            dealhistories.Add(new UserHistory()
            {
                DealId = requestDeal.DealID.GetValueOrDefault(),
                Activity = DealActivity.EFCreatedInviteSent,
                LawyerName = otherLawyerName
            });
            dealhistories.Add(new UserHistory()
            {
                DealId = otherDealId,
                Activity = DealActivity.EFInviteReceived,
                LawyerName = lawyerName
            });
        }

        internal static bool IsLawyerActingForSwapped(string currentActingFor, string oldActingFor)
        {
            if ((currentActingFor == LawyerActingFor.Purchaser || currentActingFor == LawyerActingFor.Vendor) &&
                (oldActingFor == LawyerActingFor.Purchaser || oldActingFor == LawyerActingFor.Vendor) &&
                currentActingFor != oldActingFor)
            {
                return true;
            }
            return false;
        }

    }
}
