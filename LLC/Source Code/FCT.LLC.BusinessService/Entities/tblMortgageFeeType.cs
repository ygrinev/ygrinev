using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblMortgageFeeType")]
    public partial class tblMortgageFeeType
    {
        public tblMortgageFeeType()
        {
            tblMortgageFees = new HashSet<tblMortgageFee>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FeeTypeID { get; set; }

        [StringLength(50)]
        public string FeeType { get; set; }

        [StringLength(10)]
        public string Recipient { get; set; }

        [StringLength(10)]
        public string Payer { get; set; }

        public virtual ICollection<tblMortgageFee> tblMortgageFees { get; set; }
    }
}
