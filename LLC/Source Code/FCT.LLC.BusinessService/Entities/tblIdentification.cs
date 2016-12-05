using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblIdentification")]
    public partial class tblIdentification
    {
        public tblIdentification()
        {
            //tblMortgagors = new HashSet<tblMortgagor>();
            tblPersons = new HashSet<tblPerson>();
        }

        [Key]
        public int IdentificationID { get; set; }

        [StringLength(50)]
        public string IdentificationType { get; set; }

        [StringLength(50)]
        public string IdentificationNumber { get; set; }

        [StringLength(50)]
        public string PlaceOfIssue { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Birthdate { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        public string EntityType { get; set; }

        [StringLength(50)]
        public string RegistrationNumber { get; set; }

        [StringLength(100)]
        public string RecordType { get; set; }

        [StringLength(100)]
        public string ElectronicRecordSource { get; set; }

        //public virtual ICollection<tblMortgagor> tblMortgagors { get; set; }
        
        public virtual ICollection<tblPerson> tblPersons { get; set; }

    }
}
