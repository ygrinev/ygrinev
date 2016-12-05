using System.Data.SqlClient;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess.Implementations
{
    public class DealDocumentTypeRepository : Repository<tblDealDocumentType>, IDealDocumentTypeRepository
    {
        private readonly IEntityMapper<tblDealDocumentType, DealDocumentType> _dealDocumentTypeMapper;
        private readonly EFBusinessContext _context;

        public DealDocumentTypeRepository(EFBusinessContext context, IEntityMapper<tblDealDocumentType, DealDocumentType> dealDocumentTypeMapper) : base(context)
        {
            _dealDocumentTypeMapper = dealDocumentTypeMapper;
            _context = context;
        }

        public int InsertDealDocumentType(tblDealDocumentType entity)
        {
            var dealDocumentType = 0;
            if (entity != null)
            {
                Insert(entity);
                _context.SaveChanges();

                dealDocumentType = entity.DealDocumentTypeID;
            }

            return dealDocumentType;
        }

        public DealDocumentType GetByDealDocumentTypeId(int dealId, int documentTypeId, int languageId, bool isActive=true)
        {
            DealDocumentType dealDocumentType = null;
            const string query = "[dbo].[sp_tblDealDocumentType_GetByDealIDDocumentTypeID] @DealID, @DocumentTypeID, @IsActive";

            var result = _context.Database.SqlQuery<tblDealDocumentType>(query,
                new SqlParameter("@DealID", dealId),
                new SqlParameter("@DocumentTypeID", documentTypeId),
                new SqlParameter("@IsActive", isActive)).FirstOrDefault();

            if (result != null)
            {
                dealDocumentType = _dealDocumentTypeMapper.MapToData(result);
            }

            return dealDocumentType;
        }
    }
}
