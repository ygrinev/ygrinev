using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess.Implementations
{
    public class PifAnswerRepository : Repository<tblAnswer>, IPifAnswerRepository
    {
        #region Private Members
        private readonly IEntityMapper<tblAnswer, PifAnswer> _mapper;
        private readonly EFBusinessContext _context;
        #endregion

        #region Constructors
        public PifAnswerRepository(EFBusinessContext context, IEntityMapper<tblAnswer, PifAnswer> mapper)
            : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public PifAnswerRepository(DbContext context) : base(context)
        {
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// When PIF answers are saved by the user, any previous answers must be deleted before the new ones can
        /// be saved.
        /// </summary>
        /// <param name="dealId">The ID of the Deal for which questions are toe be removed</param>
        /// <param name="questionIds">The ID's of the questions whose answers must be removed</param>
        /// <returns>Returns True upon success</returns>
        public void RemoveOldAnswers(int dealId, List<int> questionIds)
        {

            if ((dealId > 0) && (questionIds.Any()))
            {
                foreach (var questionId in questionIds)
                {
                    var recordsRemoved = _context.Database.ExecuteSqlCommand("dbo.sp_RemovePIFAnswer @DealID, @QuestionID",
                            new SqlParameter("@DealID", dealId),
                            new SqlParameter("@QuestionID", questionId));

                    _context.SaveChanges();
                }
            }
        }

        public PifAnswer GetPifAnswer(int answerId)
        {
            PifAnswer answer = null;
            if (answerId > 0)
            {
                var answerEntity = _context.Database.SqlQuery<tblAnswer>("dbo.sp_GetPIFAnswer @AnswerID",
                    new SqlParameter("@AnswerID", answerId)).FirstOrDefault();

                if (answerEntity != null)
                {
                    answer = _mapper.MapToData(answerEntity);
                }
            }

            return answer;
        }

        public PifAnswer GetPifAnswer(int dealId, int answerTypeId)
        {
            PifAnswer answer = null;
            if ((dealId > 0) && (answerTypeId > 0))
            {
                var answerEntity = _context.Database.SqlQuery<tblAnswer>("dbo.sp_GetPIFAnswerForAnswerType @DealID, @AnswerTypeID",
                    new SqlParameter("@DealID", dealId),
                    new SqlParameter("@AnswerTypeID", answerTypeId)).FirstOrDefault();

                if (answerEntity != null)
                {
                    answer = _mapper.MapToData(answerEntity);
                }
            }

            return answer;
        }

        public List<PifAnswer> GetPifAnswersForQuestion(int dealId, int questionId)
        {
            var answers = new List<PifAnswer>();
            if ((questionId > 0) && (dealId > 0))
            {
                var answerEntities = _context.Database.SqlQuery<tblAnswer>("dbo.sp_GetPIFAnswersForQuestion @DealID, @QuestionID",
                    new SqlParameter("@DealID", dealId),
                    new SqlParameter("@QuestionID", questionId)).ToList();

                if (answerEntities.Any())
                {
                    foreach (var answerEntity in answerEntities)
                    {
                        var answer = _mapper.MapToData(answerEntity);

                        if (answer != null)
                        {
                            answers.Add(answer);
                        }
                    }
                }
            }

            return answers;
        }

        public bool SaveAnswer(int dealId, PifAnswer answer)
        {
            var success = false;

            if (answer != null)
            {
                var entity = _mapper.MapToEntity(answer);

                if (entity != null)
                {
                    var result = _context.Database.SqlQuery<tblAnswer>("dbo.sp_SetPIFAnswer @DealID, @QuestionID, @AnswerTypeID, @AnswerData",
                        new SqlParameter("@DealID", dealId),
                        new SqlParameter("@QuestionID", answer.QuestionID),
                        new SqlParameter("@AnswerTypeID", answer.AnswerTypeID),
                        new SqlParameter("@AnswerData", answer.AnswerData)).FirstOrDefault();

                    _context.SaveChanges();

                    if ((result != null) && (result.AnswerID > 0))
                    {
                        success = true;
                    }
                }
            }

            return success;
        }

        #endregion
    }
}
