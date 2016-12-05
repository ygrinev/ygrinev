using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess.Interfaces
{
    public interface IPifAnswerTypeRepository : IRepository<tblAnswerType>
    {
        List<PifAnswerType> GetAnswerTypesByQuestionId(int questionId);
    }
}
