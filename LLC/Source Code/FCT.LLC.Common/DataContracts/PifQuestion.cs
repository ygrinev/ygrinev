using System.Collections.Generic;

namespace FCT.LLC.Common.DataContracts
{
    public class PifQuestion
    {
        #region Public Properties
        public int QuestionID { get; set; }
        public int DisplaySequence { get; set; }
        public bool IsFraudQuestion { get; set; }
        public bool IsForOrderingTitleInsurance { get; set; }
        public bool IsFinalQuestion { get; set; }
        public bool IsNationalExcludeQC { get; set; }
        public string EnglishText { get; set; }
        public string FrenchText { get; set; }
        public string Province { get; set; }
        public List<PifAnswerType> AnswerTypes { get; set; }
        #endregion

        #region Constructors

        public PifQuestion()
        {
            AnswerTypes = new List<PifAnswerType>();
        }
        #endregion
    }
}
