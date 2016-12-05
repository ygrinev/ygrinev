using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess.Interfaces
{
    public interface IPifQuestionRepository : IRepository<tblQuestion>
    {
        List<PifQuestion> GetQuestionsByDealId(int dealId, bool recalculateQuestions);
        List<tblQuestion> GettblQuestionsByDealId(int dealId, bool recalculateQuestions);
    }
}
