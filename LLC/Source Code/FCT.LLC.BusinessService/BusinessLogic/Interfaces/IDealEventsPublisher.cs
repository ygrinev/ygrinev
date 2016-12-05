using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IDealEventsPublisher
    {
        bool SendDealBusinessModel(int dealId, int userId);

        bool PublishConfirmClosing(int dealId, int userId);

        bool AddDealEventForCancellation(int dealId, string userName, string userType);
    }
}
