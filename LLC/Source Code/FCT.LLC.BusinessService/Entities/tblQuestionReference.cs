using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblQuestionReference")]
    public partial class tblQuestionReference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuestionReferenceID { get; set; }

        public int QuestionID { get; set; }

        [StringLength(2)]
        public string Province { get; set; }

        public bool IsNationalExcludeQC { get; set; }

        [Required]
        [StringLength(50)]
        public string DealType { get; set; }

        [Column(TypeName = "money")]
        public decimal RegisteredAmountLimit { get; set; }

        public int IsActive { get; set; }

        public DateTime LastModified { get; set; }

        public virtual tblProvince tblProvince { get; set; }

        public virtual tblQuestion tblQuestion { get; set; }
    }
}
