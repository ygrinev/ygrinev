using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess.Interfaces
{
    public interface IDocumentTypeRepository : IRepository<tblDocumentType>
    {
        DocumentType GetByName(string documentTypeName, string categoryName, string businessModel=null);
    }
}
