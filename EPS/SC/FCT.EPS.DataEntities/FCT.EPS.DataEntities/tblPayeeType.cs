namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPayeeType")]
    public partial class tblPayeeType
    {
        public tblPayeeType()
        {
            tblPayeeReference = new HashSet<tblPayeeReference>();
            tblPaymentFieldReference = new HashSet<tblPaymentFieldReference>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int PayeeTypeID { get; set; }

        [StringLength(50)]
        public string PayeeTypeName { get; set; }

        [StringLength(50)]
        public string PayeeTypeNameFr { get; set; }
        
        public int PayeeTypeDisplayIndex { get; set; }

        public virtual ICollection<tblPayeeReference> tblPayeeReference { get; set; }

        public virtual ICollection<tblPaymentFieldReference> tblPaymentFieldReference { get; set; }
    }
}
