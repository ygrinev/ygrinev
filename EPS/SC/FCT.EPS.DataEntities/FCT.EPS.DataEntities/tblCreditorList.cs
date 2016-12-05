namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblCreditorList")]
    public partial class tblCreditorList
    {

     
        [Key, ForeignKey("tblPayeeInfo")]
        public int PayeeInfoID { get; set; }

        public string CompanyID {get; set;}
        public string CompanyName { get; set; } 	
        public string Department { get; set; }
        public int? CCIN { get; set; } 	
        public string CompanyNameFr { get; set; } 
        public string CompanyShortNameEn { get; set; } 
        public string CompanyShortNameFr { get; set; } 
        public string LanguageIndicator  { get; set; } 
        public DateTime EffectiveDate { get; set; } 
        public string CreditorType { get; set; }

        [Required, ForeignKey("PayeeInfoID")]
        public virtual tblPayeeInfo tblPayeeInfo { get; set; }

        public virtual tblCreditorRules tblCreditorRules { get; set; }
        
    }
}
