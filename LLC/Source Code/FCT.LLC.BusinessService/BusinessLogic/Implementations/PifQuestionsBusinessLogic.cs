using System;
using System.Collections.Generic;
using System.Linq;
using FCT.LLC.BusinessService.BusinessLogic.Interfaces;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic.Implementations
{
    public class PifQuestionsBusinessLogic : IPifQuestionsBusinessLogic
    {
        #region Private Members

        private IPifQuestionRepository _pifQuestionRepository;
        private IPifAnswerTypeRepository _pifAnswerTypeRepository;
        private IPifAnswerRepository _answerRepository;
        private readonly IEntityMapper<tblAnswer, PifAnswer> _answerMapper;
        private readonly IEntityMapper<tblAnswerType, PifAnswerType> _answerTypeMapper;



        #endregion

        #region Constructors

        public PifQuestionsBusinessLogic(IPifQuestionRepository pifQuestionRepository, IPifAnswerTypeRepository pifAnswerTypeRepository, IPifAnswerRepository answerRepository, IEntityMapper<tblAnswer, PifAnswer> answerMapper, IEntityMapper<tblAnswerType, PifAnswerType> answerTypeMapper)
        {
            if (pifQuestionRepository == null) throw new ArgumentNullException("pifQuestionRepository");
            if (pifAnswerTypeRepository == null) throw new ArgumentNullException("pifAnswerTypeRepository");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");

            _pifQuestionRepository = pifQuestionRepository;
            _pifAnswerTypeRepository = pifAnswerTypeRepository;
            _answerRepository = answerRepository;
            _answerMapper = answerMapper;
            _answerTypeMapper = answerTypeMapper;
        }
        #endregion

        #region Implementation of IPifQuestionsBusinessLogic
        public PifQuestionCollection GetPifQuestions(int dealId, bool recalculateQuestions)
        {
            if (dealId <= 0) throw new ArgumentException("Invalid Deal ID");
            var pifQuestions = new PifQuestionCollection();

            var questions = _pifQuestionRepository.GetQuestionsByDealId(dealId, recalculateQuestions);
            
            if (questions.Any())
            {
                foreach (var question in questions)
                {
                    var answerTypes = _pifAnswerTypeRepository.GetAnswerTypesByQuestionId(question.QuestionID);

                    if (answerTypes.Any())
                    {
                        foreach (var answerType in answerTypes)
                        {
                            var answer = _answerRepository.GetPifAnswer(dealId, answerType.AnswerTypeID);

                            if (answer != null)
                            {
                                answerType.Answer = answer;
                            }
                        }

                        question.AnswerTypes = answerTypes;
                    }
                }

                pifQuestions.AddRange(questions);
            }

            return pifQuestions;
        }

        public List<tblQuestion> GettblQuestionsByDealId(int dealId, bool recalculateQuestions)
        {
            if (dealId <= 0) throw new ArgumentException("Invalid Deal ID");
            var pifQuestions = new List<tblQuestion>();

            var retrievedQuestions = _pifQuestionRepository.GettblQuestionsByDealId(dealId, recalculateQuestions);

            if (retrievedQuestions == null || !retrievedQuestions.Any()) return pifQuestions;
            foreach (var question in retrievedQuestions)
            {
                var answerTypes = _pifAnswerTypeRepository.GetAnswerTypesByQuestionId(question.QuestionID);
                if (answerTypes.Any())
                {
                    var tblAnswerTypeList = new List<tblAnswerType>();
                    foreach (var answerType in answerTypes)
                    {
                        var answer = _answerRepository.GetPifAnswer(dealId, answerType.AnswerTypeID);
                        if (answer != null)
                        {
                            answerType.Answer = answer;
                        }
                        
                        tblAnswerTypeList.Add(_answerTypeMapper.MapToEntity(answerType));
                    }

                    question.AnswerTypes = tblAnswerTypeList;
                }
            }
            pifQuestions.AddRange(retrievedQuestions);

            return pifQuestions;
        }

        public bool SavePifAnswers(int dealId, PifAnswerCollection answers)
        {
            if (answers == null) throw new ArgumentNullException("answers");
            var success = false;

            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                // Purge any existing answers for the question & deal
                var questionIds = answers.Select(a => a.QuestionID).Distinct().ToList();
                _answerRepository.RemoveOldAnswers(dealId, questionIds);

                foreach (var answer in answers)
                {
                    success = _answerRepository.SaveAnswer(dealId, answer);

                    if (!success)
                    {
                        break;
                    }
                }

                if (success)
                {
                    scope.Complete();
                }
                
            }

            return success;
        }
         
        public bool SavetblAnswers(int dealId, FCT.LLC.Portal.DTOs.Dto.PifAnswerCollection answers)
        {
            if (answers == null) throw new ArgumentNullException("answers");
            var success = false;

            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                // Purge any existing answers for the question & deal
                var questionIds = answers.Select(a => a.QuestionID).Distinct().ToList();
                _answerRepository.RemoveOldAnswers(dealId, questionIds);

                foreach (var answer in answers)
                {
                    PifAnswer tempAnswer = _answerMapper.MapToData(answer);
                    success = _answerRepository.SaveAnswer(dealId, tempAnswer);

                    if (!success)
                    {
                        break;
                    }
                }

                if (success)
                {
                    scope.Complete();
                }

            }

            return success;
        }
        #endregion
    }
}
