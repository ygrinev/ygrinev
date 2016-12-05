namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPaymentFieldInfo")]
    public partial class tblPaymentFieldInfo
    {
        public tblPaymentFieldInfo()
        {
            tblPaymentFieldReference = new HashSet<tblPaymentFieldReference>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int PaymentFieldID { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentFieldDefinition { get; set; }

        [Required]
        [StringLength(50)]
        public string FieldLabelEn { get; set; }

        [StringLength(50)]
        public string FieldLabelFr { get; set; }

        public int FieldLabelIndex { get; set; }

        public virtual ICollection<tblPaymentFieldReference> tblPaymentFieldReference { get; set; }
    }
}
