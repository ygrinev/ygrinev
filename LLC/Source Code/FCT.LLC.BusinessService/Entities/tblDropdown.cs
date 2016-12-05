using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDropdown")]
    public partial class tblDropdown
    {
        public tblDropdown()
        {
            //tblLenders = new HashSet<tblLender>();
        }

        [Key]
        public int DropdownID { get; set; }

        [StringLength(1000)]
        public string DropdownValue { get; set; }

        [StringLength(1000)]
        public string DropdownText { get; set; }

        [StringLength(50)]
        public string FieldName { get; set; }

        [StringLength(50)]
        public string DbTableName { get; set; }

        public int? SortOrder { get; set; }

        [StringLength(1000)]
        public string DropDownTextFrench { get; set; }

        //public virtual ICollection<tblLender> tblLenders { get; set; }
    }
}
