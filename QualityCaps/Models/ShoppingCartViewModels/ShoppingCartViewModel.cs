using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCaps.Models.ShoppingCartViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal CartTotal { get; set; }

   //     public decimal UnitSubTotal { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GST { get; set; }

    }
}
