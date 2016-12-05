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
    public interface IReturnFundsHelper
    {
        void SendEmailNotification(FundingDeal fundingDeal, LawyerProfile lawyerProfile,
            string recipientActingFor, StandardNotificationKey notificationKey, decimal depositAmount);
    }
}
