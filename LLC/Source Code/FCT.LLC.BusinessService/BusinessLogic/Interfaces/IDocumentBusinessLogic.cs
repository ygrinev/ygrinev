using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Portal.DTOs.Dto;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IDocumentBusinessLogic
    {
        DealDocuments GetDealDocs(tblDeal deal, int langID);
    }
}
