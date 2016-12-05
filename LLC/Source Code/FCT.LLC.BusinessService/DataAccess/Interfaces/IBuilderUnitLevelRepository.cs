using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IBuilderUnitLevelRepository : IRepository<tblBuilderUnitLevel>
    {
        void InsertBuilderUnitLevel(BuilderUnitLevel builderUnitLevel,int builderLegalDescriptionId);
        void UpdateBuilderUnitLevel(BuilderUnitLevel builderUnitLevel, int builderLegalDescriptionId);       
        tblBuilderUnitLevel FindBy(int tblBuilderUnitLevel);

        BuilderUnitLevelCollection InsertBuilderUnitLevelRange(IEnumerable<BuilderUnitLevel> unitlevels, int builderLegalDescriptionId);

        void UpdateBuilderUnitLevelRange(IEnumerable<BuilderUnitLevel> unitlevels, int builderLegalDescriptionId);
        //void DeleteAndInsertPins(int PropertyID, int FromPropertyID);

        IEnumerable<tblBuilderUnitLevel> GetBuilderUnitLevels(int builderLegalDescriptionId);
    }
}