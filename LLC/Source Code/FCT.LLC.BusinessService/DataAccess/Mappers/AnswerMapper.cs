using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class AnswerMapper : IEntityMapper<tblAnswer, PifAnswer>
    {
        #region Implementation of IEntityMapper
        public PifAnswer MapToData(tblAnswer tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var answer = new PifAnswer();
                answer.AnswerID = tEntity.AnswerID;
                answer.AnswerTypeID = tEntity.AnswerTypeID;
                answer.QuestionID = tEntity.QuestionID;
                answer.AnswerData = tEntity.AnswerData.Trim();

                return answer;
            }

            return null;
        }

        public tblAnswer MapToEntity(PifAnswer tData)
        {
            if (tData != null)
            {
                var answer = new tblAnswer();
                answer.AnswerID = tData.AnswerID;
                answer.AnswerTypeID = tData.AnswerTypeID;
                answer.QuestionID = tData.QuestionID;
                answer.AnswerData = tData.AnswerData.Trim();

                return answer;
            }

            return null;
        }

        #endregion
    }
}
