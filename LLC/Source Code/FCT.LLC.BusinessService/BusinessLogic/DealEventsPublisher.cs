using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;


namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class DealEventsPublisher:IDealEventsPublisher
    {
        private const string ConnectionKeyName = "LLCDBconnection";

        private readonly IDealRepository _dealRepository;
        private readonly ILawyerRepository _lawyerRepository;
  
        public DealEventsPublisher(IDealRepository dealRepository, ILawyerRepository lawyerRepository)
        {
            _dealRepository = dealRepository;
            _lawyerRepository = lawyerRepository;
        }

        public bool AddDealEventForCancellation(int dealId, string userName, string userType)
        {
            if (!CancelDealHelper.IsLawyerOrClerkOrAssistant(userType))
            {
                return false;
            }

            bool bIsToWayLender = _dealRepository.IsTwoWayLender(dealId);

            if (bIsToWayLender)
            {
                int userId = 0;
                LawyerProfile lawyerProfile = _lawyerRepository.GetUserDetails(userName);
                if (null != lawyerProfile)
                {
                    userId = lawyerProfile.LawyerID;
                }
                var publisher = new DealEventPublishing.Client.Publisher(ConnectionKeyName);

                // using (new TransactionScope(TransactionScopeOption.Suppress))
                {
                    return publisher.PublishRequestCancellation(dealId, userId);
                }

            }

            return false;
        }


        public bool SendDealBusinessModel(int dealId, int userId)
        {
            bool bIsToWayLender = _dealRepository.IsTwoWayLender(dealId);

            if (bIsToWayLender)
            {
                var publisher = new DealEventPublishing.Client.Publisher(ConnectionKeyName);

                // using (new TransactionScope(TransactionScopeOption.Suppress))
                {
                    return publisher.PublishDealBusinessModel(dealId, userId);
                }

            }

            return false;
        }


        public bool PublishConfirmClosing(int dealId, int userId)
        {
            bool bIsToWayLender = _dealRepository.IsTwoWayLender(dealId);

            if (bIsToWayLender)
            {
                var publisher = new DealEventPublishing.Client.Publisher(ConnectionKeyName);

                // using (new TransactionScope(TransactionScopeOption.Suppress))
                {
                    return publisher.PublishConfirmClosing(dealId, userId);
                }

            }

            return false;
        }
    }
}
