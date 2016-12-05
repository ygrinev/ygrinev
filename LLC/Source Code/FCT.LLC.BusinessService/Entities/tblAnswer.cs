using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblAnswer")]
    public partial class tblAnswer
    {
        [Key]
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerTypeID { get; set; }
        public string AnswerData { get; set; }
    }
}
