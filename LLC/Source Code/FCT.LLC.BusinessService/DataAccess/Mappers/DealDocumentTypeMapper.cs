using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class DealDocumentTypeMapper : IEntityMapper<tblDealDocumentType, DealDocumentType>
    {
        public DealDocumentType MapToData(tblDealDocumentType tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var data = new DealDocumentType
                {
                    DealDocumentTypeId = tEntity.DealDocumentTypeID,
                    DocumentTypeId = tEntity.DocumentTypeID,
                    DealId = tEntity.DealID,
                    DisplayNameSuffix = tEntity.DisplayNameSuffix,
                    IsActive = tEntity.IsActive
                };

                return data;
            }

            return null;
        }

        public tblDealDocumentType MapToEntity(DealDocumentType tData)
        {
            if (tData != null)
            {
                var entity = new tblDealDocumentType
                {
                    DealDocumentTypeID = tData.DealDocumentTypeId,
                    DocumentTypeID = tData.DocumentTypeId,
                    DealID = tData.DealId,
                    DisplayNameSuffix = tData.DisplayNameSuffix,
                    IsActive = tData.IsActive
                };

                return entity;
            }
            return null;
        }
    }
}
