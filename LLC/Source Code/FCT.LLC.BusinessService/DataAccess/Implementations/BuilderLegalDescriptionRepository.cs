using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
//using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class BuilderLegalDescriptionRepository : Repository<tblBuilderLegalDescription>, IBuilderLegalDescriptionRepository
    {
        

        private EFBusinessContext _context;
        private IEntityMapper<tblBuilderLegalDescription, BuilderLegalDescription> _builderLegalDescriptionMapper;
        private IEntityMapper<tblBuilderUnitLevel, BuilderUnitLevel> _builderUnitLevelMapper;
        public BuilderLegalDescriptionRepository(EFBusinessContext context, 
            IEntityMapper<tblBuilderLegalDescription, BuilderLegalDescription> builderLegalDescriptionMapper,
            IEntityMapper<tblBuilderUnitLevel, BuilderUnitLevel> builderUnitLevelMapper
            )
            : base(context)
        {
            _context = context;
            _builderLegalDescriptionMapper = builderLegalDescriptionMapper;
            _builderUnitLevelMapper = builderUnitLevelMapper;
        }

       
        //public BuilderLegalDescription InsertBuilderLegalDescription(BuilderLegalDescription builderLegalDescription, int propertyId)
        //{
        //    tblBuilderLegalDescription tblBuilderLegalDescription = this.EntityMapper(builderLegalDescription, propertyId);
        //    _context.tblBuilderLegalDescriptions.Add(tblBuilderLegalDescription);
        //    _context.SaveChanges();
        //    return _builderLegalDescriptionMapper.MapToData(tblBuilderLegalDescription);
        //}

        public BuilderLegalDescription InsertBuilderLegalDescription(BuilderLegalDescription builderLegalDescription, int propertyId)
        {
            try
            {
                var tEntity = _builderLegalDescriptionMapper.MapToEntity(builderLegalDescription);
                tEntity.PropertyID = propertyId;
                Insert(tEntity);
                _context.SaveChanges();

                var result = _builderLegalDescriptionMapper.MapToData(tEntity);
                return result;
            }
            catch (DbEntityValidationException dbEx)
            {
                TraceExceptionInDebugMode(dbEx);
            }
            return null;
        }

        public void UpdateBuilderLegalDescription(BuilderLegalDescription builderLegalDescription, int propertyId)
        {
            try
            {
                var tEntity = _builderLegalDescriptionMapper.MapToEntity(builderLegalDescription);
                tEntity.PropertyID = propertyId;

                //if (builderLegalDescription.BuilderLegalDescriptionID != null && builderLegalDescription.BuilderLegalDescriptionID > 0)
                //{
                //    foreach (tblBuilderUnitLevel unitLevel in tEntity.tblBuilderUnitLevels)
                //    {
                //        unitLevel.BuilderLegalDescriptionID = builderLegalDescription.BuilderLegalDescriptionID ?? default(int);
                //    }
                //}
               
                Update(tEntity);
                _context.SaveChanges();

               
            }
            catch (DbEntityValidationException dbEx)
            {
                TraceExceptionInDebugMode(dbEx);
            }

        }

        
        public tblBuilderLegalDescription FindBy(int tblBuilderLegalDescription)
        {
            tblBuilderLegalDescription result =
              GetAll().FirstOrDefault(x => x.BuilderLegalDescriptionID == tblBuilderLegalDescription);
            return result;
        }

        public void DeleteBy(int tblBuilderLegalDescriptionId)
        {
            tblBuilderLegalDescription result =
              GetAll().FirstOrDefault(x => x.BuilderLegalDescriptionID == tblBuilderLegalDescriptionId);
            Delete(result);
            _context.SaveChanges();
        }

        /// <summary>
        /// Mapping Builder Unit Level Object to Builder Unit Level Entity
        /// </summary>
        /// <param name="builderLegalDescription">Builder Unit Level Object</param>
        /// <param name="propertyId"></param>
        private tblBuilderLegalDescription EntityMapper(BuilderLegalDescription builderLegalDescription, int propertyId)
        {
            tblBuilderLegalDescription tblBuilderLegalDescription = _builderLegalDescriptionMapper.MapToEntity(builderLegalDescription);
            //tblBuilderLegalDescription.PropertyID = propertyId;
            return tblBuilderLegalDescription;
        }

        [Conditional("DEBUG")]
        private static void TraceExceptionInDebugMode(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                        validationError.ErrorMessage);
                }
            }
        }
    }
}