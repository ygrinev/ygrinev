using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IPayoutLetterWorklistRepository : IRepository<vw_PayoutLetterWorklist>
    {
       
 
        List<vw_PayoutLetterWorklist> GetPayoutLetterWorkList(string ChequeBatchNumber, PayoutLetterWorklistOrderBySpecificationCollection payoutLetterWorklistOrderBySpecificationCollection, int pageIndex, int pageSize, UserContext userContext, out int rows);
    }
}
