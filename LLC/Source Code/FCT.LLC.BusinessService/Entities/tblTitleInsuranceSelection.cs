using System.ComponentModel.DataAnnotations;

namespace FCT.LLC.BusinessService.Entities
{
    public partial class tblTitleInsuranceSelection
    {
        [Key]
        public int SelectionID { get; set; }

        [StringLength(127)]
        public string InsurerName { get; set; }

        [StringLength(255)]
        public string InsurerLink { get; set; }

        [StringLength(255)]
        public string InsurerLinkFrench { get; set; }

        public int? Sequence { get; set; }

        [StringLength(2)]
        public string InsurerProvince { get; set; }

        public virtual tblProvince tblProvince { get; set; }
    }
}
