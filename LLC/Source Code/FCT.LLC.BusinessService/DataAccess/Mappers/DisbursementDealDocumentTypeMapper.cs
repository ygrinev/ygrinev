using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class DisbursementDealDocumentTypeMapper : IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType>
    {
        public DisbursementDealDocumentType MapToData(tblDisbursementDealDocumentType tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var data = new DisbursementDealDocumentType
                {
                    DealDocumentTypeId = tEntity.DealDocumentTypeID,
                    CreationDate = tEntity.PayoutLetterDate,
                    DisbursementDealDocumentTypeId = tEntity.DisbursementDealDocumentType,
                    DisbursementId = tEntity.DisbursementID
                };

                return data;
            }

            return null;
        }

        public tblDisbursementDealDocumentType MapToEntity(DisbursementDealDocumentType tData)
        {
            if (tData != null)
            {
                var entity = new tblDisbursementDealDocumentType
                {
                    DealDocumentTypeID = tData.DealDocumentTypeId,
                    PayoutLetterDate = tData.CreationDate,
                    DisbursementDealDocumentType = tData.DisbursementDealDocumentTypeId,
                    DisbursementID = tData.DisbursementId
                };

                return entity;
            }

            return null;
        }
    }
}
