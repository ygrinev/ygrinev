namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPaymentMethod")]
    public partial class tblPaymentMethod
    {
        public tblPaymentMethod()
        {
            tblPayeeInfo = new HashSet<tblPayeeInfo>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int PaymentMethodID { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethodName { get; set; }


        [Required]
        [StringLength(50)]
        public string PaymentMethodNameFr { get; set; }
        public bool SameDay { get; set; }

        public virtual ICollection<tblPayeeInfo> tblPayeeInfo { get; set; }
    }
}
