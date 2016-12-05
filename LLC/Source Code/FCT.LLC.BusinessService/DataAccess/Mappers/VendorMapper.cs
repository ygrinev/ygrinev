using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class VendorMapper : IEntityMapper<tblVendor, Vendor>
    {
        public Vendor MapToData(tblVendor tEntity, object parameters = null)
        {
            var vendor = new Vendor()
            {
                CompanyName = tEntity.CompanyName,
                FirstName = tEntity.FirstName,
                LastName = tEntity.LastName,
                MiddleName = tEntity.MiddleName,
                VendorType = tEntity.VendorType,
                VendorID = tEntity.VendorID
            };
            return vendor;
        }

        public tblVendor MapToEntity(Vendor tData)
        {
            var vendor = new tblVendor()
            {
                CompanyName = tData.CompanyName,
                FirstName = tData.FirstName,
                LastName = tData.LastName,
                MiddleName = tData.MiddleName,
                VendorType = tData.VendorType
            };
            if (tData.VendorID.HasValue)
            {
                vendor.VendorID =(int) tData.VendorID;
            }
            return vendor;
        }
    }
}
