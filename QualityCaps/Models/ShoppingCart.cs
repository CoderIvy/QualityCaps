using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QualityCaps.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QualityCaps.Data;

namespace QualityCaps.Models
{
    public class ShoppingCart
    {
        public string ShoppingCartID { get; set; }
        public const string CartSessionKey = "cartID";

        public static ShoppingCart GetCart(HttpContext context) {
            var cart = new ShoppingCart();
            cart.ShoppingCartID = cart.GetCartID(context);
            return cart;
        }

        /// <summary>
        /// add new cap to cart or increase the quantity
        /// </summary>
        /// <param name="cap"></param>
        /// <param name="db"></param>
        public void AddToCart(Cap cap, ApplicationDbContext db)
        {
            var cartItem = db.CartItems.SingleOrDefault(c => c.CartID == ShoppingCartID && c.Cap.CapID == cap.CapID);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Cap = cap,
                    CartID = ShoppingCartID,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.CartItems.Add(cartItem);
            }
            else {
                cartItem.Count++;
            }
            db.SaveChanges();
        }

        /// <summary>
        /// remove cap from cart or decrease the quantity of caps
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int RemoveFromCart(int id, ApplicationDbContext db) {
            var cartItem = db.CartItems.SingleOrDefault(cart => cart.CartID == ShoppingCartID && cart.Cap.CapID == id);
            int itemCount = 0;
            if (cartItem != null) {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else {
                    db.CartItems.Remove(cartItem);
                }
                db.SaveChanges();
            }
            return itemCount;
        }

        /// <summary>
        /// remove all items in cart
        /// </summary>
        /// <param name="db"></param>
        public void EmptyCart(ApplicationDbContext db) {
            var cartItems = db.CartItems.Where(cart => cart.CartID == ShoppingCartID);
            foreach (var cartItem in cartItems) {
                db.CartItems.Remove(cartItem);
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<CartItem> GetCartItems(ApplicationDbContext db) {
            List<CartItem> cartItems = db.CartItems.Include(i => i.Cap).Where(cartItem => cartItem.CartID == ShoppingCartID).ToList();

            return cartItems;
        }

        /// <summary>
        /// get total quantity
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public int GetCount(ApplicationDbContext db) {
            int? count = (from cartItems in db.CartItems
                          where cartItems.CartID == ShoppingCartID
                          select (int?)cartItems.Count).Sum();
            return count ?? 0;
        }

        /// <summary>
        /// get total price
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public decimal GetTotal(ApplicationDbContext db) {
            decimal? total = (from cartItems in db.CartItems
                              where cartItems.CartID == ShoppingCartID
                              select (int?)cartItems.Count * cartItems.Cap.Price).Sum();
            return total ?? decimal.Zero;
        }

        public decimal GetSubTotal(ApplicationDbContext db)
        {
            decimal? total = (from cartItems in db.CartItems
                              where cartItems.CartID == ShoppingCartID
                              select (int?)cartItems.Count * cartItems.Cap.Price).Sum();
            
            decimal? subTotal = total * ((decimal)0.85f); 
            return subTotal ?? decimal.Zero;
        }

        public decimal GetGST(ApplicationDbContext db)
        {
            decimal? total = (from cartItems in db.CartItems
                              where cartItems.CartID == ShoppingCartID
                              select (int?)cartItems.Count * cartItems.Cap.Price).Sum();

            decimal? subTotal = total * ((decimal)0.15f);
            return subTotal ?? decimal.Zero;
        }


        /// <summary>
        /// use session key to get cart id
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCartID(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) == null) {
                Guid tempCartID = Guid.NewGuid();
                context.Session.SetString(CartSessionKey, tempCartID.ToString());
            }
            return context.Session.GetString(CartSessionKey).ToString();
        }
    }
}
