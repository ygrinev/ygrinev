using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblMortgageProduct")]
    public partial class tblMortgageProduct
    {
        public tblMortgageProduct()
        {
            tblMortgages = new HashSet<tblMortgage>();
        }

        [Key]
        public int MortgageProductID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public bool? Active { get; set; }

        public virtual ICollection<tblMortgage> tblMortgages { get; set; }
    }
}
