using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess.Interfaces
{
    public interface IDisbursementDealDocumentTypeRepository : IRepository<tblDisbursementDealDocumentType>
    {
        int InsertDisbursementDealDocumentType(tblDisbursementDealDocumentType entity);
        DisbursementDealDocumentType GetByDisbursementIdDocumentTypeId(int dealId, int disbursementId, int documentTypeId);
        DisbursementDealDocumentType GetByDisbursementDealDocumentTypeId(int disbursementDealDocumentTypeId);
    }
}
