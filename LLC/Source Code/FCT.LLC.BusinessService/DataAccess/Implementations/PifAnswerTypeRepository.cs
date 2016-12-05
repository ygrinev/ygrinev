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
    public class PifAnswerTypeRepository : Repository<tblAnswerType>, IPifAnswerTypeRepository
    {
        #region Private Members
        private readonly IEntityMapper<tblAnswerType, PifAnswerType> _mapper;
        private readonly EFBusinessContext _context;
        #endregion

        #region Constructors
        public PifAnswerTypeRepository(EFBusinessContext context, IEntityMapper<tblAnswerType, PifAnswerType> mapper)
            : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public PifAnswerTypeRepository(DbContext context) : base(context)
        {
        }
        #endregion

        #region Public Methods

        public List<PifAnswerType> GetAnswerTypesByQuestionId(int questionId)
        {
            var answerTypes = new List<PifAnswerType>();

            if (questionId > 0)
            {
                var answerTypeEntities = _context.Database.SqlQuery<tblAnswerType>("dbo.sp_GetAnswerTypesForPIFQuestion @QuestionID",
                    new SqlParameter("@QuestionID", questionId)).ToList();

                if (answerTypeEntities.Any())
                {
                    foreach (var answerTypeEntity in answerTypeEntities)
                    {
                        var answerType = _mapper.MapToData(answerTypeEntity);

                        if (answerType != null)
                        {
                            answerTypes.Add(answerType);
                        }
                    }
                }
            }

            return answerTypes;
        }
        #endregion
    }
}
