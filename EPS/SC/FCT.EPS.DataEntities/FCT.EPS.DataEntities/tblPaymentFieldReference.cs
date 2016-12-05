namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
  

    [Table("tblPaymentFieldReference")]
    public partial class tblPaymentFieldReference
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PayeeTypeID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PaymentFieldID { get; set; }

        [StringLength(50)]
        public string PayeeFieldLabelEn { get; set; }

        [StringLength(50)]
        public string PayeeFieldLabelFr { get; set; }

        public int? PayeeFieldLabelIndex { get; set; }

        public virtual tblPayeeType tblPayeeType { get; set; }

        public virtual tblPaymentFieldInfo tblPaymentFieldInfo { get; set; }
    }
}
