using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.DataEntities
{
    [Table("tblBPSReference")]
    public class tblBPSReference
    {
        [Key, ForeignKey("tblFCTAccount")]
        public int FCTAccountID { get; set; }

        public string ClientNumber { get; set; }
        public string TransmitID { get; set; }

        public int PayeeInfoID { get; set; }

        [Required, ForeignKey("FCTAccountID")]
        public virtual tblFCTAccount tblFCTAccount { get; set; }

        [Required, ForeignKey("PayeeInfoID")]
        public virtual tblPayeeInfo tblPayeeInfo { get; set; }

    }
}
