using System.Collections.Generic;
using System.Linq;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public class PropertyMapper:IEntityMapper<tblProperty, Property>
    {
        private readonly IEntityMapper<tblPIN, Pin> _pinMapper;
        private readonly IEntityMapper<tblBuilderLegalDescription, BuilderLegalDescription> _builderLegalDescriptionMapper;

        public PropertyMapper(IEntityMapper<tblPIN, Pin> pinMapper, 
                                IEntityMapper<tblBuilderLegalDescription, BuilderLegalDescription> builderLegalDescriptionMapper)
        {
            _pinMapper = pinMapper;
            _builderLegalDescriptionMapper = builderLegalDescriptionMapper;
        }
        public Property MapToData(tblProperty tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var property = new Property()
                {
                    Address = tEntity.Address,
                    Address2 = tEntity.Address2,
                    City = tEntity.City,
                    Country = tEntity.Country,
                    PostalCode = tEntity.PostalCode,
                    PropertyID = tEntity.PropertyID,
                    Province = tEntity.Province,
                    StreetNumber = tEntity.StreetNumber,
                    UnitNumber = tEntity.UnitNumber,
                    Pins = new PinCollection(),
                    BuilderLegalDescription = _builderLegalDescriptionMapper.MapToData(tEntity.tblBuilderLegalDescriptions.FirstOrDefault())
                };
                property.Pins.AddRange(tEntity.tblPINs.OrderBy(p=>p.PINID).Select(p => _pinMapper.MapToData(p)));

                return property;
            }
           return null;
        }

        public tblProperty MapToEntity(Property tData)
        {
            var property = new tblProperty()
            {
                Address = tData.Address,
                Address2 = tData.Address2,
                City = tData.City,
                Country = tData.Country,
                PostalCode = tData.PostalCode,
                Province = tData.Province,
                StreetNumber = tData.StreetNumber,
                UnitNumber = tData.UnitNumber
            };
            if (tData.PropertyID.HasValue)
            {
                property.PropertyID =(int) tData.PropertyID;
            }
            return property;
        }
    }
}
