using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class BuilderUnitLevelRepository : Repository<tblBuilderUnitLevel>, IBuilderUnitLevelRepository
    {
        private IEntityMapper<tblBuilderUnitLevel, BuilderUnitLevel> _entityMapper;
        private EFBusinessContext _businessContext;

        public BuilderUnitLevelRepository(EFBusinessContext context, IEntityMapper<tblBuilderUnitLevel, BuilderUnitLevel> entityMapper)
            : base(context)
        {
            _entityMapper = entityMapper;
            _businessContext = context;
        }

        public void InsertBuilderUnitLevel(BuilderUnitLevel builderUnitLevel, int builderLegalDescriptionId)
        {
            tblBuilderUnitLevel tblBuilderUnitLevel = this.EntityMapper(builderUnitLevel, builderLegalDescriptionId);
            Insert(tblBuilderUnitLevel);
            _businessContext.SaveChanges();
        }

        public void UpdateBuilderUnitLevel(BuilderUnitLevel builderUnitLevel, int builderLegalDescriptionId)
        {
            tblBuilderUnitLevel tblBuilderUnitLevel = this.EntityMapper(builderUnitLevel, builderLegalDescriptionId);
            Update(tblBuilderUnitLevel);
            _businessContext.SaveChanges();

        }

        public BuilderUnitLevelCollection InsertBuilderUnitLevelRange(IEnumerable<BuilderUnitLevel> unitlevels, int builderLegalDescriptionId)
        {
            if (unitlevels != null)
            {
                var entities = unitlevels.Select(p => _entityMapper.MapToEntity(p));
                var results = new List<tblBuilderUnitLevel>();

                foreach (var entity in entities)
                {
                    entity.BuilderLegalDescriptionID = builderLegalDescriptionId;
                    if (entity.BuilderUnitLevelId > 0)
                    {
                        entity.BuilderUnitLevelId = entity.BuilderUnitLevelId;
                    }
                    results.Add(entity);
                }
                InsertRange(results);
                _businessContext.SaveChanges();

                var insertedUnitLevels = results.Select(r => _entityMapper.MapToData(r));
                var collection = new BuilderUnitLevelCollection();
                collection.AddRange(insertedUnitLevels);
                return collection;
            }
            return null;
        }

        public void UpdateBuilderUnitLevelRange(IEnumerable<BuilderUnitLevel> unitlevels, int builderLegalDescriptionId)
        {
            if (unitlevels != null)
            {
                var requestedUnitLevels = unitlevels.Select(p => _entityMapper.MapToEntity(p));
                var deletedUnitLevels = GetBuilderUnitLevels(builderLegalDescriptionId).Except(requestedUnitLevels, new UnitLevelEqualityComparer());
                DeleteRange(deletedUnitLevels);

                _businessContext.SaveChanges();

                _businessContext.Configuration.AutoDetectChangesEnabled = false;

                foreach (var unitlevel in unitlevels)
                {

                    if (unitlevel.BuilderUnitLevelID.HasValue)
                    {
                        var entity = _entityMapper.MapToEntity(unitlevel);
                        entity.BuilderLegalDescriptionID = builderLegalDescriptionId;
                        Update(entity);
                    }
                    else
                    {
                        var entity = _entityMapper.MapToEntity(unitlevel);
                        entity.BuilderLegalDescriptionID = builderLegalDescriptionId;
                        Insert(entity);
                    }
                }
                _businessContext.SaveChanges();
                _businessContext.Configuration.AutoDetectChangesEnabled = true;
            }

        }

        public IEnumerable<tblBuilderUnitLevel> GetBuilderUnitLevels(int builderLegalDescriptionId)
        {
            var results = GetAll().Where(v => v.BuilderLegalDescriptionID == builderLegalDescriptionId).AsEnumerable();
            return results;
        }
        public void DeleteBy(int builderUnitLevelId)
        {
            tblBuilderUnitLevel result =
              GetAll().FirstOrDefault(x => x.BuilderUnitLevelId == builderUnitLevelId);
            Delete(result);
            _businessContext.SaveChanges();
        }

        public tblBuilderUnitLevel FindBy(int tblBuilderUnitLevel)
        {
            tblBuilderUnitLevel result =
               GetAll().FirstOrDefault(x => x.BuilderUnitLevelId == tblBuilderUnitLevel);
            return result;
        }

        /// <summary>
        /// Mapping Builder Unit Level Object to Builder Unit Level Entity
        /// </summary>
        /// <param name="builderUnitLevel">Builder Unit Level Object</param>
        /// <param name="builderLegalDescriptionId">Builder Legal Description Id</param>
        /// <returns>Builder Unit Level Entity</returns>
        private tblBuilderUnitLevel EntityMapper(BuilderUnitLevel builderUnitLevel, int builderLegalDescriptionId)
        {
            tblBuilderUnitLevel tblBuilderUnitLevel = _entityMapper.MapToEntity(builderUnitLevel);
            tblBuilderUnitLevel.BuilderLegalDescriptionID = builderLegalDescriptionId;
            return tblBuilderUnitLevel;
        }


        private class UnitLevelEqualityComparer : IEqualityComparer<tblBuilderUnitLevel>
        {
            public bool Equals(tblBuilderUnitLevel x, tblBuilderUnitLevel y)
            {
                return x.BuilderUnitLevelId == y.BuilderUnitLevelId;
            }

            public int GetHashCode(tblBuilderUnitLevel obj)
            {
                return obj.BuilderUnitLevelId.GetHashCode();
            }
        }
    }

}