using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.SBWTSA.BusinessLogic
{
    internal class Grouping
    {
        //public string PayeeBankAccountHolderName { get; set; }
        //public string PayeeTransitNumber { get; set; }
        //public string PayeeAccountNumber { get; set; }
        public int? PaymentTransactionID { get; set; }
        //public string PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
