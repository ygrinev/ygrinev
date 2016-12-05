using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class DocumentTypeMapper : IEntityMapper<tblDocumentType, DocumentType>
    {
        public DocumentType MapToData(tblDocumentType tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var data = new DocumentType
                {
                    DocumentTypeId = tEntity.DocumentTypeID,
                    DocumentCategoryId = tEntity.DocumentCategoryID,
                    DocumentTypeCodeId = tEntity.DocumentTypeCodeID,
                    IsSignatureRequired = tEntity.IsSignatureRequired,
                    IsUploadable = tEntity.IsUploadable,
                    IsGeneratable = tEntity.IsGeneratable,
                    Name = tEntity.Name,
                    IsSubmitable = tEntity.IsSubmitable,
                    IsOther = tEntity.IsOther,
                    IsDefaultType = tEntity.IsDefaultType,
                    LenderId = tEntity.LenderID,
                    IsEditable = tEntity.IsEditable,
                    IsCacheable = tEntity.IsCacheable,
                    VirusScan = tEntity.VirusScan,
                    IsSubmitableCondition = tEntity.IsSubmitableCondition,
                    IsArchivable = tEntity.IsArchivable,
                    IsUploadableCondition = tEntity.IsUploadableCondition,
                    IsArchivableCondition = tEntity.IsArchivableCondition,
                    IsPublishable = tEntity.IsPublishable,
                    IsPublishableCondition = tEntity.IsPublishableCondition,
                    BusinessModel = tEntity.BusinessModel
                };

                return data;
            }

            return null;
        }

        public tblDocumentType MapToEntity(DocumentType tData)
        {
            if (tData != null)
            {
                var entity = new tblDocumentType
                {
                    DocumentTypeID = tData.DocumentTypeId,
                    DocumentCategoryID = tData.DocumentCategoryId,
                    DocumentTypeCodeID = tData.DocumentTypeCodeId,
                    IsSignatureRequired = tData.IsSignatureRequired,
                    IsUploadable = tData.IsUploadable,
                    IsGeneratable = tData.IsGeneratable,
                    Name = tData.Name,
                    IsSubmitable = tData.IsSubmitable,
                    IsOther = tData.IsOther,
                    IsDefaultType = tData.IsDefaultType,
                    LenderID = tData.LenderId,
                    IsEditable = tData.IsEditable,
                    IsCacheable = tData.IsCacheable,
                    VirusScan = tData.VirusScan,
                    IsSubmitableCondition = tData.IsSubmitableCondition,
                    IsArchivable = tData.IsArchivable,
                    IsUploadableCondition = tData.IsUploadableCondition,
                    IsArchivableCondition = tData.IsArchivableCondition,
                    IsPublishable = tData.IsPublishable,
                    IsPublishableCondition = tData.IsPublishableCondition,
                    BusinessModel = tData.BusinessModel
                };
                return entity;
            }

            return null;
        }
    }
}
