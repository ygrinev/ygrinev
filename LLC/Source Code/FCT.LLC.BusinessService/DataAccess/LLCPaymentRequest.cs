using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.DataAccess
{
   public class LLCPaymentRequest
    {
      public int DisbursementID { get; set; }
      public int DealFundsAllocationID { get; set; }
      public string Message { get; set; }
      public int PaymentRequestID { get; set; }
      public DateTime RequestDate { get; set; }
    }
}
