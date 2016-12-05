using System;
using System.Data.SqlClient;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess.Implementations
{
    public class DocumentTypeRepository : Repository<tblDocumentType>, IDocumentTypeRepository
    {
        private readonly IEntityMapper<tblDocumentType, DocumentType> _documentTypeMapper;
        private readonly EFBusinessContext _context;

        public DocumentTypeRepository(EFBusinessContext context, IEntityMapper<tblDocumentType, DocumentType> documentTypeMapper)
            : base(context)
        {
            _documentTypeMapper = documentTypeMapper;
            _context = context;
        }

        public DocumentType GetByName(string documentTypeName, string categoryName, string businessModel=null)
        {
            DocumentType documentType = null;
            const string query = "[dbo].[sp_tblDocumentType_GetByName] @DocumentTypeName, @CategoryName, @BusinessModel";

            tblDocumentType result = null;

            if (string.IsNullOrWhiteSpace(businessModel))
            {
                result = _context.Database.SqlQuery<tblDocumentType>(query,
                                new SqlParameter("@DocumentTypeName", documentTypeName),
                                new SqlParameter("@CategoryName", categoryName),
                                new SqlParameter("@BusinessModel", DBNull.Value)).SingleOrDefault();
            }
            else
            {
                result = _context.Database.SqlQuery<tblDocumentType>(query,
                                new SqlParameter("@DocumentTypeName", documentTypeName),
                                new SqlParameter("@CategoryName", categoryName),
                                new SqlParameter("@BusinessModel", businessModel)).SingleOrDefault();
            }

            if (result != null)
            {
                documentType = _documentTypeMapper.MapToData(result);
            }

            return documentType;
        }
    }
}
