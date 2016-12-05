using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IApplicationLocker
    {
        int GetApplicationLock(int fundingDealId, List<ErrorCode> errorCodes);
        int ReleaseApplicationLock();
    }
}
