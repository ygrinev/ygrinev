using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.Services.LIM.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class MilestoneUpdated
    {
        internal bool ActiveUserSignRemoved { get; set; }
        internal bool PassiveUserSignRemoved { get; set; }
        
    }

    public class Payment
    {
        public DateTime PaymentDate { get; set; }
        public UserProfile LawyerProfile { get; set; }
        public int LawyerTrustAccountId { get; set; }
    }
}
