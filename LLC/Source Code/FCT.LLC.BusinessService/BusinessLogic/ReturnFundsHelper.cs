using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Client;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class ReturnFundsHelper:IReturnFundsHelper
    {
        private readonly IEmailHelper _emailHelper;
        private readonly IFundingDealRepository _fundingDealRepository;
        public ReturnFundsHelper(IEmailHelper emailHelper, IFundingDealRepository fundingDealRepository)
        {
            _emailHelper = emailHelper;
            _fundingDealRepository = fundingDealRepository;
        }
        public void SendEmailNotification(FundingDeal fundingDeal, LawyerProfile lawyerProfile,
          string recipientActingFor, StandardNotificationKey notificationKey, decimal depositAmount)
        {
            var connection = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings[EmailHelper.DBContextName].ConnectionString
            };

            int otherDealId = _fundingDealRepository.GetOtherDealInScope(fundingDeal.DealID.GetValueOrDefault());

           var mailingList= _emailHelper.GetEmailRecipientList(fundingDeal, otherDealId);

            var tokens = _emailHelper.CreateTokens(fundingDeal, recipientActingFor);
            tokens.Add(EmailTemplateTokenList.DepositAmount, depositAmount.ToString("c"));

            EasyFundDispatcher.SendDealNotificationEmailToSpecificEmailId(connection,
                mailingList.GetValueOrDefault().Key, notificationKey, tokens,
                mailingList.GetValueOrDefault().Value);
        }
    }
}
