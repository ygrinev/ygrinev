using System.Collections.Generic;
using System.Linq;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class QuestionMapper : IEntityMapper<tblQuestion, PifQuestion>
    {
        #region Private Members

        private AnswerTypeMapper _answerTypeMapper;
        private readonly IEntityMapper<tblAnswer, PifAnswer> _answerMapper;

        public QuestionMapper(IEntityMapper<tblAnswer, PifAnswer> answerMapper)
        {
            _answerMapper = answerMapper;
        }

        #endregion

        #region Private Properties

        private AnswerTypeMapper AnswerTypeMapper
        {
            get
            {
                if (_answerTypeMapper == null)
                {
                    _answerTypeMapper = new AnswerTypeMapper(_answerMapper);
                }

                return _answerTypeMapper;
            }
        }
        #endregion

        #region Implementation of IEntityMapper
        public PifQuestion MapToData(tblQuestion tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var question = new PifQuestion();
                question.QuestionID = tEntity.QuestionID;
                question.DisplaySequence = tEntity.DisplaySequence;
                
                if (!string.IsNullOrWhiteSpace(tEntity.EnglishText))
                {
                    question.EnglishText = tEntity.EnglishText.Trim();
                }

                if (!string.IsNullOrWhiteSpace(tEntity.FrenchText))
                {
                    question.FrenchText = tEntity.FrenchText.Trim();
                }
                
                question.IsForOrderingTitleInsurance = tEntity.IsForOrderingTitleInsurance;
                question.IsFraudQuestion = tEntity.IsFraudQuestion;
                question.IsNationalExcludeQC = tEntity.IsNationalExcludeQC;
                question.IsFinalQuestion = tEntity.IsFinalQuestion;
                if (!string.IsNullOrWhiteSpace(tEntity.Province))
                {
                    question.Province = tEntity.Province.Trim();
                }

                if (tEntity.AnswerTypes.Any())
                {
                    question.AnswerTypes = new List<PifAnswerType>();
                    foreach (var answerTypeEntity in tEntity.AnswerTypes)
                    {
                        var answerType = AnswerTypeMapper.MapToData(answerTypeEntity);

                        if (answerType != null)
                        {
                            question.AnswerTypes.Add(answerType);
                        }
                    }
                }

                return question;
            }

            return null;
        }

        public tblQuestion MapToEntity(PifQuestion tData)
        {
            if (tData != null)
            {
                var question = new tblQuestion();
                question.QuestionID = tData.QuestionID;
                question.DisplaySequence = tData.DisplaySequence;
                
                if (!string.IsNullOrWhiteSpace(tData.EnglishText))
                {
                    question.EnglishText = tData.EnglishText.Trim();
                }

                if (!string.IsNullOrWhiteSpace(tData.FrenchText))
                {
                    question.FrenchText = tData.FrenchText.Trim();    
                }
                
                question.IsForOrderingTitleInsurance = tData.IsForOrderingTitleInsurance;
                question.IsFraudQuestion = tData.IsFraudQuestion;
                question.IsNationalExcludeQC = tData.IsNationalExcludeQC;
                question.IsFinalQuestion = tData.IsFinalQuestion;
                

                if (!string.IsNullOrWhiteSpace(tData.Province))
                {
                    question.Province = tData.Province.Trim();    
                }

                if (tData.AnswerTypes.Any())
                {
                    question.AnswerTypes = new List<tblAnswerType>();
                    foreach (var answerTypeData in tData.AnswerTypes)
                    {
                        var answerType = AnswerTypeMapper.MapToEntity(answerTypeData);

                        if (answerType != null)
                        {
                            question.AnswerTypes.Add(answerType);
                        }
                    }
                }

                return question;
            }

            return null;
        }
        #endregion
    }
}
