using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityCaps.Data;
using QualityCaps.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace QualityCaps.Controllers
{
   [Authorize(Roles ="Admin, Member")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Orders.Include(i => i.User).AsNoTracking().ToListAsync());
            }
            else if (User.IsInRole("Member")) {
                return View(await _context.Orders.Where(i => i.User.Id == user.Id && i.User.UserName == User.Identity.Name)
                    .Include(i => i.User).AsNoTracking().ToListAsync());
            }
            return null;
        }



        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("City, Country, FirstName, LastName, Phone, PostalCode")] Order order)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                ShoppingCart cart = ShoppingCart.GetCart(this.HttpContext);
                List<CartItem> items = cart.GetCartItems(_context);
                List<OrderDetail> details = new List<OrderDetail>();

                foreach (CartItem item in items)
                {
                    OrderDetail detail = CreateOrderDetailForThisItem(item);
                    detail.Order = order;
                    details.Add(detail);
                    _context.Add(detail);
                }

                order.User = user;
                order.OrderDate = DateTime.Today;
                order.Status = Status.waitting;
                order.Total = ShoppingCart.GetCart(this.HttpContext).GetTotal(_context);
                var Subtotal =order.Total * (decimal)0.85;
                ViewData["SubTotalll"] = Subtotal;

                order.OrderDetails = details;
                _context.SaveChanges();               

                return RedirectToAction("Purchased", new RouteValueDictionary(new { action = "Purchased", id = order.OrderID }));
            }
            return View(order);
        }

        private OrderDetail CreateOrderDetailForThisItem(CartItem item)
        {
            OrderDetail detail = new OrderDetail();

            detail.Quantity = item.Count;
            detail.Cap = item.Cap;
            detail.UnitPrice = item.Cap.Price;

            return detail;
        }

        public async Task<IActionResult> Purchased(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            var details = _context.OrderDetails
                .Where(detail => detail.Order.OrderID == order.OrderID).Include(detail => detail.Cap).ToList();

            order.OrderDetails = details;
            ShoppingCart.GetCart(this.HttpContext).EmptyCart(_context);

            return View(order);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            var details = _context.OrderDetails
                .Where(detail => detail.Order.OrderID == order.OrderID).Include(detail => detail.Cap).ToList();

            order.OrderDetails = details;
            ShoppingCart.GetCart(this.HttpContext).EmptyCart(_context);
            ViewData["SubTotal"] = ShoppingCart.GetCart(this.HttpContext).GetSubTotal(_context);
            ViewData["GST"] = ShoppingCart.GetCart(this.HttpContext).GetGST(_context);

            return View(order);
        }

        public async Task<IActionResult> ChangeStatus(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

     //       var order = _context.Orders.SingleOrDefaultAsync(o => o.OrderID == Convert.ToInt32(id));
            var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(o => o.OrderID == Convert.ToInt32(id));

            if (order == null)
            {
                return NotFound();
            }

            if (order.Status.Equals(Status.shipped))
            {
                order.Status = Status.waitting;
            }
            else {
                order.Status = Status.shipped;
            }
            
            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            var details = _context.OrderDetails
                .Where(detail => detail.Order.OrderID == order.OrderID).Include(detail => detail.Cap).ToList();

            order.OrderDetails = details;
            ShoppingCart.GetCart(this.HttpContext).EmptyCart(_context);

            return View(order);
        }


        // POST: Orders1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,FirstName,LastName,City,PostalCode,Country,Status,Phone,Total,OrderDate")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }


        /*

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("City, Country, FirstName, LastName, Phone, PostalCode")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Fail to update the Order item, try again later.");
                        throw;
                    }
                }
                //   return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        */
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(i => i.User).AsNoTracking()
                .SingleOrDefaultAsync(m => m.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            var details = _context.OrderDetails.Where(detail => detail.Order.OrderID == order.OrderID)
                .Include(detail => detail.Cap).ToList();
            order.OrderDetails = details;

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderID == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
