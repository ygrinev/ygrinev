using System.Linq;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class BuilderLegalDescriptionMapper : IEntityMapper<tblBuilderLegalDescription, BuilderLegalDescription>
    {
        private readonly IEntityMapper<tblBuilderUnitLevel, BuilderUnitLevel> _builderUnitLevelEntityMapper;
        public BuilderLegalDescriptionMapper(IEntityMapper<tblBuilderUnitLevel, BuilderUnitLevel> builderUnitLevelEntityMapper)
        {
            _builderUnitLevelEntityMapper = builderUnitLevelEntityMapper;
        }

        public BuilderLegalDescription MapToData(tblBuilderLegalDescription tEntity, object parameters = null)
        {
            if (tEntity == null) return null;
            var builderLegalDescription = new BuilderLegalDescription
            {
                BuilderLegalDescriptionID = tEntity.BuilderLegalDescriptionID,
                BuilderLot = tEntity.BuilderLot,
                BuilderProjectReference = tEntity.BuilderProjectReference,
                Lot = tEntity.Lot,
                Plan = tEntity.Plan,
                BuilderUnitsLevels = new BuilderUnitLevelCollection()
            };

            if (tEntity.tblBuilderUnitLevels != null)
                builderLegalDescription.BuilderUnitsLevels.
                    AddRange(tEntity.tblBuilderUnitLevels.
                        OrderBy(x => x.BuilderUnitLevelId).Select(z => _builderUnitLevelEntityMapper.MapToData(z)));
            return builderLegalDescription;
        }

        public tblBuilderLegalDescription MapToEntity(BuilderLegalDescription tData)
        {
            if (tData == null) return null;
            
            var builderLegalDescription = new tblBuilderLegalDescription
            {
                Lot = tData.Lot,
                Plan = tData.Plan,
                BuilderLot = tData.BuilderLot,
                BuilderProjectReference = tData.BuilderProjectReference
                //tblProperty = new tblProperty()
            };

            if (tData.BuilderLegalDescriptionID.HasValue)
            {
                builderLegalDescription.BuilderLegalDescriptionID = (int)tData.BuilderLegalDescriptionID;
            }

            //foreach (BuilderUnitLevel unitLevel in tData.BuilderUnitsLevels)
            //{
            //    builderLegalDescription.tblBuilderUnitLevels.Add(_builderUnitLevelEntityMapper.MapToEntity(unitLevel));
            //}

            return builderLegalDescription;
        }
    }
}