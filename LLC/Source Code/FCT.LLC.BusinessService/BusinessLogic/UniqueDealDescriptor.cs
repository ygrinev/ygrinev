using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class UniqueDealDescriptor
    {
        public MortgagorCollection Mortgagors { get; set; }
        public VendorCollection Vendors { get; set; }
        public PropertyDescriptor Property { get; set; }
        public PinCollection Pins { get; set; }
        public DateTime? ClosingDate { get; set; }

        internal static UniqueDealDescriptor ToUniqueDeal(FundingDeal deal)
        {
            var propertyDesc = new PropertyDescriptor()
            {
                Address = deal.Property.Address,
                Address2 = deal.Property.Address2,
                City = deal.Property.City,
                Country = deal.Property.Country,
                PostalCode = deal.Property.PostalCode,
                PropertyID = deal.Property.PropertyID,
                Province = deal.Property.Province,
                StreetNumber = deal.Property.StreetNumber,
                UnitNumber = deal.Property.UnitNumber
            };
            var uniqueDeal = new UniqueDealDescriptor()
            {
                ClosingDate = deal.ClosingDate,
                Mortgagors = deal.Mortgagors,
                Property = propertyDesc,
                Vendors = deal.Vendors,
                Pins = deal.Property.Pins
            };
            return uniqueDeal;
        }

    }
}
