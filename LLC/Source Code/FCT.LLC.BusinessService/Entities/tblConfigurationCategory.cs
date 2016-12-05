using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblConfigurationCategory")]
    public partial class tblConfigurationCategory
    {
        public tblConfigurationCategory()
        {
            tblConfigurationOptions = new HashSet<tblConfigurationOption>();
        }

        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public DateTime LastModified { get; set; }

        public virtual ICollection<tblConfigurationOption> tblConfigurationOptions { get; set; }
    }
}
