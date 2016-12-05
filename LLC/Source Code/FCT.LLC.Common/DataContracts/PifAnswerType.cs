using System.Collections.Generic;

namespace FCT.LLC.Common.DataContracts
{
    public class PifAnswerType
    {
        #region Public Properties
        public int AnswerTypeID { get; set; }
        public int QuestionID { get; set; }
        public int DisplaySequence { get; set; }
        public string DisplayControlType { get; set; }
        public int? ControlMaxSize { get; set; }
        public string ValidationExpression { get; set; }
        public string AnswerDataType { get; set; }
        public string EnglishText { get; set; }
        public string FrenchText { get; set; }
        public PifAnswer Answer { get; set; }
        #endregion

    }
}
