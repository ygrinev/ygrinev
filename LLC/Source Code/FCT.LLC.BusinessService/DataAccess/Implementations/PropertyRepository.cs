using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;
using System.Data.SqlClient;

namespace FCT.LLC.BusinessService.DataAccess
{
   public class PropertyRepository:Repository<tblProperty>,IPropertyRepository
   {
       private readonly IEntityMapper<tblProperty, Property> _propertyMapper;
       private readonly EFBusinessContext _context;
       public PropertyRepository(EFBusinessContext context, IEntityMapper<tblProperty, Property> propertyMapper) : base(context)
       {
           _propertyMapper = propertyMapper;
           _context = context;
       }

       public Property InsertProperty(Property property, int dealID)
       {
           try
           {
               var tEntity = _propertyMapper.MapToEntity(property);
               tEntity.DealID = dealID;
               Insert(tEntity);
               _context.SaveChanges();

               var result = _propertyMapper.MapToData(tEntity);
               return result;
           }
           catch (DbEntityValidationException dbEx)
           {
               TraceExceptionInDebugMode(dbEx);
           }
           return null;
       }

       public void UpdateProperty(Property property, int dealID)
       {
           try
           {
               var tEntity = _propertyMapper.MapToEntity(property);
               tEntity.DealID = dealID;
               Update(tEntity);
               _context.SaveChanges();
           }
           catch (DbEntityValidationException dbEx)
           {
               TraceExceptionInDebugMode(dbEx);
           }

       }

       public void UpdateProperty(tblProperty tEntity)
       {
           try
           {
               tEntity.tblDeal = null;
               tEntity.tblPINs = null;
               Update(tEntity);
               _context.SaveChanges();
           }
           catch (DbEntityValidationException dbEx)
           {
               TraceExceptionInDebugMode(dbEx);
           }
       }

       public int UpdatePropertyForOtherDeal(Property property, int dealID)
       {
           var existingProperty = GetAll().SingleOrDefault(p => p.DealID == dealID);
           if (existingProperty != null)
           {
               var dbPropertyId = existingProperty.PropertyID;
               var requestedProperty = _propertyMapper.MapToEntity(property);
               existingProperty = requestedProperty;
               existingProperty.PropertyID = dbPropertyId;
               existingProperty.DealID = dealID;
               Update(existingProperty);
               _context.SaveChanges();
               return existingProperty.PropertyID;
           }
           return 0;
       }

       public Property GetPropertyByScope(int fundingDealId)
       {
           if (fundingDealId > 0)
           {
               Property property = _context.Database.SqlQuery<Property>("dbo.usp_getPropertyByScope @fundingDealId",
                   new SqlParameter("@fundingDealId", fundingDealId)).LastOrDefault();
               return property;
           }
           return null;
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
