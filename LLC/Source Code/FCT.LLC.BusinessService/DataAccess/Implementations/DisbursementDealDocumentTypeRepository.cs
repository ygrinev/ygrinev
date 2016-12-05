using System.Data.SqlClient;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess.Implementations
{
    public class DisbursementDealDocumentTypeRepository : Repository<tblDisbursementDealDocumentType>, IDisbursementDealDocumentTypeRepository
    {
        private readonly IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType> _disbursementDealDocumentTypeMapper;
        private readonly EFBusinessContext _context;

        public DisbursementDealDocumentTypeRepository(EFBusinessContext context, IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType> disbursementDealDocumentTypeMapper)
            : base(context)
        {
            _disbursementDealDocumentTypeMapper = disbursementDealDocumentTypeMapper;
            _context = context;
        }


        public int InsertDisbursementDealDocumentType(tblDisbursementDealDocumentType entity)
        {
            var disbursementDealDocumentType = 0;
            if (entity != null)
            {
                Insert(entity);
                _context.SaveChanges();

                disbursementDealDocumentType = entity.DisbursementDealDocumentType;
            }

            return disbursementDealDocumentType;
        }

        public DisbursementDealDocumentType GetByDisbursementIdDocumentTypeId(int dealId, int disbursementId, int documentTypeId)
        {
            DisbursementDealDocumentType disbursementDealDocumentType = null;
            const string query = "[dbo].[sp_tblDisbursementDealDocumentType_GetByDisbursementIDDealDocumentTypeId] @DealID, @DisbursementID, @DocumentTypeID";

            var result = _context.Database.SqlQuery<tblDisbursementDealDocumentType>(query,
                new SqlParameter("@DealID", dealId),
                new SqlParameter("@DisbursementID", disbursementId),
                new SqlParameter("@DocumentTypeID", documentTypeId)).SingleOrDefault();

            if (result != null)
            {
                disbursementDealDocumentType = _disbursementDealDocumentTypeMapper.MapToData(result);
            }

            return disbursementDealDocumentType;
        }

        public DisbursementDealDocumentType GetByDisbursementDealDocumentTypeId(int disbursementDealDocumentTypeId)
        {
            DisbursementDealDocumentType disbursementDealDocumentType = null;
            const string query = "[dbo].[sp_tblDisbursementDealDocumentType_GetByID] @DisbursementDealDocumentType";
            var result = _context.Database.SqlQuery<tblDisbursementDealDocumentType>(query,
                new SqlParameter("@DisbursementDealDocumentType", disbursementDealDocumentTypeId)).SingleOrDefault();

            if (result != null)
            {
                disbursementDealDocumentType = _disbursementDealDocumentTypeMapper.MapToData(result);
            }

            return disbursementDealDocumentType;
        }
    }
}
