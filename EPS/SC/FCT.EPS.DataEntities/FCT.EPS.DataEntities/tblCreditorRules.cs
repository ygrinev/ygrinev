namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblCreditorRules")]
    public partial class tblCreditorRules
    {

        [Key]
        [ForeignKey("tblCreditorList")]
        public int PayeeInfoID { get; set; }

        public int AccountNumberExactLength { get; set; }
        public int AccountNumberMinLength { get; set; }	
        public int AccountNumberMaxLength { get; set; }	
        public string AccountNumberEditRules { get; set; }	
        public string AccountNumberDataTypeFormat { get; set; }
        public string AccountNumberValidStart { get; set; }	
        public string AccountNumberInValidStart { get; set; }
        public string AccountNumberValidEnd  { get; set; }
        public string AccountNumberInvalidEnd { get; set; }

        [Required, ForeignKey("PayeeInfoID")]
        public virtual tblCreditorList tblCreditorList { get; set; }		
    }
}
