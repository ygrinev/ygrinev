using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IBuilderLegalDescriptionRepository : IRepository<tblBuilderLegalDescription>
    {
        BuilderLegalDescription InsertBuilderLegalDescription(BuilderLegalDescription builderLegalDescription, int propertyId);
        void UpdateBuilderLegalDescription(BuilderLegalDescription builderLegalDescription, int propertyId);
        tblBuilderLegalDescription FindBy(int tblBuilderLegalDescription);
    }
}