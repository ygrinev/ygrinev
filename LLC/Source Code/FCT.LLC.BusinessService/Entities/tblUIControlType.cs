using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblUIControlType")]
    public partial class tblUIControlType
    {
        public tblUIControlType()
        {
            tblAnswerTypes = new HashSet<tblAnswerType>();
        }

        [Key]
        public int UIControlTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string ControlTypeName { get; set; }

        public int? ControlMaxSize { get; set; }

        [StringLength(200)]
        public string ValidationExpression { get; set; }

        public DateTime LastModified { get; set; }

        public virtual ICollection<tblAnswerType> tblAnswerTypes { get; set; }
    }
}
