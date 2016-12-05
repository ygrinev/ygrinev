using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess.Interfaces
{
    public interface IDealDocumentTypeRepository : IRepository<tblDealDocumentType>
    {
        int InsertDealDocumentType(tblDealDocumentType entity);
        DealDocumentType GetByDealDocumentTypeId(int dealId, int documentTypeId, int languageId, bool isActive=true);
    }
}
