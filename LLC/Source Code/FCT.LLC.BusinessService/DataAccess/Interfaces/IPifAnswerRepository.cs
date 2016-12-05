using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess.Interfaces
{
    public interface IPifAnswerRepository : IRepository<tblAnswer>
    {
        void RemoveOldAnswers(int dealId, List<int> questionIds);
        PifAnswer GetPifAnswer(int answerId);
        PifAnswer GetPifAnswer(int dealId, int answerTypeId);
        List<PifAnswer> GetPifAnswersForQuestion(int dealId, int questionId);
        bool SaveAnswer(int dealId, PifAnswer answer);
    }
}
