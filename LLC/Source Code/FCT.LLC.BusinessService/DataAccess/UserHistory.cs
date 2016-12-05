using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class UserHistory
    {
        public string Activity { get; set; }
        public string LawyerName { get; set; }
        public int DealId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
