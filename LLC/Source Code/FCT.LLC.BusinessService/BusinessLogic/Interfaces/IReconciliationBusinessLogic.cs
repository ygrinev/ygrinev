using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;


namespace FCT.LLC.BusinessService.BusinessLogic.Interfaces
{
    public interface IReconciliationBusinessLogic
    {
        GetReconciliationWorklistResponse GetReconciliationWorklist(GetReconciliationWorklistRequest request);
        void SaveReconciliation(SaveReconciliationRequest request);
    }
}
