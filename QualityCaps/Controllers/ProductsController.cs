using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityCaps.Data;
using QualityCaps.Models;

namespace QualityCaps.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? page, int? categoryId)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else {
                searchString = currentFilter;
            }

            //for search product by name
            ViewData["currentFilter"] = searchString;
            var caps = from c in _context.Caps
                       select c;

            int price;
            if (int.TryParse(searchString, out price)) {
                caps = caps.Where(c => c.Price.Equals(price));
            } else if(!String.IsNullOrEmpty(searchString)) {
                caps = caps.Where(c => c.CapName.Contains(searchString));
            }

            //for showing caps with different category id
            if (categoryId != null) {
                caps = caps.Where(c => c.CategoryID == categoryId);
            }

            int pageSize = 8;

            var categories = _context.Categories.ToList();
            ViewData["Categories"] = categories;    
                    
            return View(await PaginatedList<Cap>.CreatAsync(caps.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Caps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cap = await _context.Caps
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.CapID == id);
            if (cap == null)
            {
                return NotFound();
            }

            return View(cap);
        }

    }
}
