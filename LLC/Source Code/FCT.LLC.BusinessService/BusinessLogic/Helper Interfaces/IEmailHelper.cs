using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IEmailHelper
    {
        IDictionary<string, object> CreateTokens(FundingDeal fundingDeal, string actingFor);

        string GetEmailRecipientList(LawyerProfile user, string products);

        KeyValuePair<string, string>? GetEmailRecipientList(FundingDeal requestDeal, int otherDealId=0);

        void SendStandardNotification(FundingDeal fundingDeal, LawyerProfile lawyerProfile,
            string recipientActingFor, StandardNotificationKey notificationKey);
    }
}
