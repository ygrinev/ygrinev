using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblResourceText")]
    public partial class tblResourceText
    {
        public tblResourceText()
        {
            tblAnswerTypes = new HashSet<tblAnswerType>();
            tblQuestions = new HashSet<tblQuestion>();
        }

        [Key]
        public int ResourceTextID { get; set; }

        [StringLength(500)]
        public string ResourceTextEn { get; set; }

        [StringLength(500)]
        public string ResourceTextFr { get; set; }

        public DateTime LastModified { get; set; }

        public virtual ICollection<tblAnswerType> tblAnswerTypes { get; set; }

        public virtual ICollection<tblQuestion> tblQuestions { get; set; }
    }
}
