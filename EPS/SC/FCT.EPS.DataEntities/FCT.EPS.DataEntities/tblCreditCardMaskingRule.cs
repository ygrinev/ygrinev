namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblCreditCardMaskingRule")]
    public partial class tblCreditCardMaskingRule
    {
        public tblCreditCardMaskingRule()
        {
            tblCreditorList = new HashSet<tblCreditorList>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaskingRuleID  { get; set; }
        public string Description { get; set; }

        public virtual ICollection<tblCreditorList> tblCreditorList { get; set; }	
    }
}
