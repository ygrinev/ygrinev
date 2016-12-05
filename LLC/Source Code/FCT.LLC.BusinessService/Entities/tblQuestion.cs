using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblQuestion")]
    public partial class tblQuestion
    {
        #region Public Properties
        [Key]
        public int QuestionID { get; set; }
        public int DisplaySequence { get; set; }
        public bool IsFraudQuestion { get; set; }
        public bool IsForOrderingTitleInsurance { get; set; }
        public bool IsFinalQuestion { get; set; }
        public bool IsNationalExcludeQC { get; set; }
        public string EnglishText { get; set; }
        public string FrenchText { get; set; }
        public string Province { get; set; }
        
        public virtual ICollection<tblAnswerType> AnswerTypes { get; set; }
        #endregion

        #region Constructors

        public tblQuestion()
        {
            AnswerTypes = new List<tblAnswerType>();
        }
        #endregion
    }
}
