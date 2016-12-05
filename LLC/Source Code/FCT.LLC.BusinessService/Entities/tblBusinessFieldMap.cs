using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblBusinessFieldMap")]
    public partial class tblBusinessFieldMap
    {
        public tblBusinessFieldMap()
        {
            tblLawyerSavedAmendments = new HashSet<tblLawyerSavedAmendment>();
            tblRejectLenderAmendments = new HashSet<tblRejectLenderAmendment>();
        }

        public int BusinessFieldID { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InternalComponentID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(500)]
        public string ComponentFieldName { get; set; }

        public virtual tblBusinessField tblBusinessField { get; set; }

        public virtual tblInternalComponent tblInternalComponent { get; set; }

        public virtual ICollection<tblLawyerSavedAmendment> tblLawyerSavedAmendments { get; set; }

        public virtual ICollection<tblRejectLenderAmendment> tblRejectLenderAmendments { get; set; }
    }
}
