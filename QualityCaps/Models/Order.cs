using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace QualityCaps.Models
{
    public enum Status
    {
        waitting, shipped
    }
    public class Order
    {
        public int OrderID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(30)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public Status Status { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }
        public decimal Total { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Order Date")]
        public System.DateTime OrderDate { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
        public ApplicationUser User { get; set; }
        
    }
}
