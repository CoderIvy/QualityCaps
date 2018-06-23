using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QualityCaps.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public string SupplierName { get; set; }

        [Required]
        [MaxLength(11)]
        [MinLength(6)]
        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public ICollection<Cap> Caps { get; set; }
  

    }
}
