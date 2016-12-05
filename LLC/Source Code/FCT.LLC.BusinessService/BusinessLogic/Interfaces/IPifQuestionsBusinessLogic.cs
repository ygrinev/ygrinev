using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic.Interfaces
{
    public interface IPifQuestionsBusinessLogic
    {
        PifQuestionCollection GetPifQuestions(int dealId, bool recalculateQuestions);
        List<tblQuestion> GettblQuestionsByDealId(int dealId, bool recalculateQuestions);
        bool SavePifAnswers(int dealId, PifAnswerCollection answers);
        bool SavetblAnswers(int dealId, FCT.LLC.Portal.DTOs.Dto.PifAnswerCollection answers);
    }
}
