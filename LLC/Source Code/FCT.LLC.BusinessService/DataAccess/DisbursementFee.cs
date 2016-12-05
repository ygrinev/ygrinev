using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DisbursementFee
    {
        public Fee PurchaserFee { get; set; }
        public Fee VendorFee { get; set; }
    }
}
