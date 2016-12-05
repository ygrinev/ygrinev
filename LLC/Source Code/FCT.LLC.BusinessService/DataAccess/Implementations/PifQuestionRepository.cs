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
    public class PifQuestionRepository : Repository<tblQuestion>, IPifQuestionRepository
    {
        #region Private Members
        private readonly IEntityMapper<tblQuestion, PifQuestion> _mapper;
        private readonly EFBusinessContext _context;
        #endregion

        #region Constructors
        public PifQuestionRepository(EFBusinessContext context, IEntityMapper<tblQuestion, PifQuestion> mapper)
            : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public PifQuestionRepository(DbContext context) : base(context)
        {
        }
        #endregion

        #region Public Methods

        public List<PifQuestion> GetQuestionsByDealId(int dealId, bool recalculateQuestions)
        {
            var questions = new List<PifQuestion>();
            if (dealId > 0)
            {
                List<tblQuestion> questionEntities = GettblQuestionsByDealId(dealId, recalculateQuestions);

                if (questionEntities.Any())
                {
                    foreach (var questionEntity in questionEntities)
                    {
                        var question = _mapper.MapToData(questionEntity);

                        if (question != null)
                        {
                            questions.Add(question);
                        }
                    }
                }

                return questions;
            }
            return null;
        }

        public List<tblQuestion> GettblQuestionsByDealId(int dealId, bool recalculateQuestions)
        {
            List<tblQuestion> questionEntities = null;
            if (dealId > 0)
            {

                if (recalculateQuestions)
                {
                    questionEntities = _context.Database.SqlQuery<tblQuestion>("dbo.sp_CalculatePIFQuestions @DealID",
                                        new SqlParameter("@DealID", dealId)).ToList();
                }
                else
                {
                    questionEntities = _context.Database.SqlQuery<tblQuestion>("dbo.sp_GetPIFQuestions @DealID",
                        new SqlParameter("@DealID", dealId)).ToList();
                }
            }
            return questionEntities;
        }
        #endregion
    }
}
