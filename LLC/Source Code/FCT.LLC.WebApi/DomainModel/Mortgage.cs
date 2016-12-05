using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomainModel
{
    public class Mortgage : AuditEntityBase
    {
        public Mortgage()
        {
        }


        /// <summary>Gets or sets the Id. </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public Guid Id { get; set; }

        public string MortgageNumber { get; set; }

        public decimal MortgageAmount { get; set; }

    }
}
