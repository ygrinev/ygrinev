using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblAnswerType")]
    public partial class tblAnswerType
    {
        #region Public Properties
        [Key]
        public int AnswerTypeID { get; set; }

        public int QuestionID { get; set; }
        public int DisplaySequence { get; set; }
        public string DisplayControlType { get; set; }
        public int? ControlMaxSize { get; set; }
        public string ValidationExpression { get; set; }
        public string AnswerDataType { get; set; }
        public string EnglishText { get; set; }
        public string FrenchText { get; set; }

        public virtual tblAnswer Answer { get; set; }
        #endregion
    }
}
