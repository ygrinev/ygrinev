using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class BuilderUnitLevelMapper : IEntityMapper<tblBuilderUnitLevel, BuilderUnitLevel>
    {
        public BuilderUnitLevel MapToData(tblBuilderUnitLevel tEntity, object parameters = null)
        {
            //if (tEntity == null) return null;
            var builderUnitLevel = new BuilderUnitLevel();
            builderUnitLevel.BuilderUnitLevelID = tEntity.BuilderUnitLevelId;
            builderUnitLevel.Level = tEntity.Level;
            builderUnitLevel.Unit = tEntity.Unit;
            return builderUnitLevel;
        }

        public tblBuilderUnitLevel MapToEntity(BuilderUnitLevel tData)
        {
            var tblBuilderUnitLevel = new tblBuilderUnitLevel();
            //if (!tData.BuilderUnitLevelID.HasValue) return null;
            if (tData.BuilderUnitLevelID == null)
                tblBuilderUnitLevel.BuilderUnitLevelId = 0;
            else
            {
                tblBuilderUnitLevel.BuilderUnitLevelId = (int)tData.BuilderUnitLevelID;
            }
            tblBuilderUnitLevel.Level = tData.Level;
            tblBuilderUnitLevel.Unit = tData.Unit;
            return tblBuilderUnitLevel;
        }
    }
}