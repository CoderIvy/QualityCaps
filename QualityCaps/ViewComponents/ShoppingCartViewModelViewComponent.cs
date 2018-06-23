using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QualityCaps.Data;
using QualityCaps.Models;
using QualityCaps.Models.ShoppingCartViewModels;
using Microsoft.AspNetCore.Mvc;

namespace QualityCaps.ViewComponents
{
    public class ShoppingCartViewModelViewComponent :ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartViewModelViewComponent(ApplicationDbContext context) {
            _context = context;
        }

        public IViewComponentResult Invoke() {
            return View(ReturnCurrentCartViewModel());
        }

        private ShoppingCartViewModel ReturnCurrentCartViewModel()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(_context),
                CartTotal = cart.GetTotal(_context),
               
                SubTotal = cart.GetSubTotal(_context),
                GST = cart.GetGST(_context)
            };
            return viewModel;
        }
    }
}
