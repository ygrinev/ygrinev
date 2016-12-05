using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class AnswerTypeMapper : IEntityMapper<tblAnswerType, PifAnswerType>
    {
        private readonly IEntityMapper<tblAnswer, PifAnswer> _answerMapper;

        public AnswerTypeMapper(IEntityMapper<tblAnswer, PifAnswer> answerMapper)
        {
            _answerMapper = answerMapper;
        }

        #region Implementation of IEntityMapper
        public PifAnswerType MapToData(tblAnswerType tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var answerType = new PifAnswerType();
                answerType.AnswerTypeID = tEntity.AnswerTypeID;
                answerType.QuestionID = tEntity.QuestionID;

                if (!string.IsNullOrWhiteSpace(tEntity.AnswerDataType))
                {
                    answerType.AnswerDataType = tEntity.AnswerDataType.Trim();    
                }

                if (!string.IsNullOrWhiteSpace(tEntity.DisplayControlType))
                {
                    answerType.DisplayControlType = tEntity.DisplayControlType.Trim();    
                }
                
                answerType.ControlMaxSize = tEntity.ControlMaxSize;

                if (!string.IsNullOrWhiteSpace(tEntity.ValidationExpression))
                {
                    answerType.ValidationExpression = tEntity.ValidationExpression.Trim();    
                }
                
                answerType.DisplaySequence = tEntity.DisplaySequence;

                if (!string.IsNullOrWhiteSpace(tEntity.EnglishText))
                {
                    answerType.EnglishText = tEntity.EnglishText.Trim();    
                }

                if (!string.IsNullOrWhiteSpace(tEntity.FrenchText))
                {
                    answerType.FrenchText = tEntity.FrenchText.Trim();    
                }



                return answerType;
            }

            return null;
        }

        public tblAnswerType MapToEntity(PifAnswerType tData)
        {
            if (tData != null)
            {
                var answerType = new tblAnswerType();

                answerType.AnswerTypeID = tData.AnswerTypeID;
                answerType.QuestionID = tData.QuestionID;

                if (!string.IsNullOrWhiteSpace(tData.AnswerDataType))
                {
                    answerType.AnswerDataType = tData.AnswerDataType.Trim();    
                }

                if (!string.IsNullOrWhiteSpace(tData.DisplayControlType))
                {
                    answerType.DisplayControlType = tData.DisplayControlType.Trim();    
                }
                
                answerType.ControlMaxSize = tData.ControlMaxSize;

                if (!string.IsNullOrWhiteSpace(tData.ValidationExpression))
                {
                    answerType.ValidationExpression = tData.ValidationExpression.Trim();    
                }
                
                answerType.DisplaySequence = tData.DisplaySequence;

                if (!string.IsNullOrWhiteSpace(tData.EnglishText))
                {
                    answerType.EnglishText = tData.EnglishText.Trim();    
                }

                if (!string.IsNullOrWhiteSpace(tData.FrenchText))
                {
                    answerType.FrenchText = tData.FrenchText.Trim();    
                }

                if (tData.Answer != null) answerType.Answer = _answerMapper.MapToEntity(tData.Answer);
                return answerType;
            }

            return null;
        }
        #endregion
    }
}
