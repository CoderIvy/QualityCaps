using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QualityCaps.Data;
using QualityCaps.Models;

namespace QualityCaps.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult Index() {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            return View(cart);
        }

        public ActionResult AddToCart(int id) {
            var addedCap = _context.Caps.Single(cap => cap.CapID == id);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedCap, _context);

            ViewData["TotalQuantity"] = cart.GetCount(_context);

            return RedirectToAction("Index", "Products");
        }


        public ActionResult RemoveFromCart(int id) {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int itemCount = cart.RemoveFromCart(id, _context);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public ActionResult ClearCart() {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.EmptyCart(_context);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public ActionResult GetSubTotal()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.GetSubTotal(_context);      
            return RedirectToAction("Index", "Products");
            //   return Redirect(Request.Headers["Referer"].ToString());
        }
        
    }
}