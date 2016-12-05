using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    public partial class ImportedQuestion
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuestionID { get; set; }

        public int? AnswerID { get; set; }

        public int? DisplaySequence { get; set; }

        public int? UIControlTypeID { get; set; }

        [Key]
        [Column("Field Type", Order = 1)]
        [StringLength(255)]
        public string Field_Type { get; set; }

        [Column("English Label")]
        public string English_Label { get; set; }

        [Column("French Label")]
        public string French_Label { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string Purchase { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string Refinance { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(10)]
        public string AnswerDataType { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool IsActive { get; set; }

        [StringLength(100)]
        public string Provinces { get; set; }
    }
}
