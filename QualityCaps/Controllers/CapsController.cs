using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using QualityCaps.Data;
using QualityCaps.Models;

namespace QualityCaps.Controllers
{
    public class CapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Caps
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //for search product by name
            ViewData["currentFilter"] = searchString;
            var caps = from c in _context.Caps
                       select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                caps = caps.Where(c => c.CapName.Contains(searchString));
            }

            int pageSize = 100;

            //  return View(await caps.AsNoTracking().ToListAsync());
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

        // GET: Caps/Create
        public IActionResult Create()
        {
            PopulateSuppliersDropDownList();
            PopulateCategoriesDropDownList();
            return View();
        }

        // POST: Caps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CapID,CapName,Description,SupplierID,Price,CategoryID,Image")] Cap cap, IFormFile uploadImg)
        {
            if (uploadImg == null || uploadImg.Length == 0)
                return Content("file not selected");
            var fileName = cap.CategoryID+"-" + DateTime.Now.Ticks.ToString()+".jpg";
                
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\images\\upload",fileName);           

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await uploadImg.CopyToAsync(stream);
            }

            try
            {
                cap.Image = "\\chenl85\\asp_assignment\\images\\upload\\"+fileName;
               
                if (ModelState.IsValid)
                {
                    _context.Add(cap);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException) {
                ModelState.AddModelError("", "Fail to create new cap item, try again later.");
            }            
            return View(cap);
        }

        // GET: Caps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PopulateSuppliersDropDownList();
            PopulateCategoriesDropDownList();

            var cap = await _context.Caps.SingleOrDefaultAsync(m => m.CapID == id);
            if (cap == null)
            {
                return NotFound();
            }
            
            return View(cap);
        }

        // POST: Caps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CapID,CapName,Description,SupplierID,Price,CategoryID,Image")] Cap cap, IFormFile uploadImg)
        {
            if (id != cap.CapID)
            {
                return NotFound();
            }

            if (uploadImg == null || uploadImg.Length == 0)
                return Content("file not selected");

            var fileName = cap.CategoryID + "-" + DateTime.Now.Ticks.ToString() + ".jpg";

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\images\\upload", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await uploadImg.CopyToAsync(stream);
            }
            
                cap.Image = "\\chenl85\\asp_assignment\\images\\upload\\" + fileName;

                if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cap);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CapExists(cap.CapID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Fail to update the cap item, try again later.");
                        throw;
                    }
                }
                // return RedirectToAction(nameof(Index));
            }
            return View(cap);
        }

        // GET: Caps/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangeError = false)
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

            if (saveChangeError.GetValueOrDefault()) {
                ViewData["ErrorMessage"] = "Delete failed. Please try agian later";
            }

            return View(cap);
        }

        // POST: Caps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cap = await _context.Caps
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.CapID == id);

            if (cap == null) {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Caps.Remove(cap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException) {
                ModelState.AddModelError("", "Fail to delete the cap item, try again later.");
                return RedirectToAction(nameof(Delete), new { id = id, saveChangeError = true });
            }
           
        }

        private bool CapExists(int id)
        {
            return _context.Caps.Any(e => e.CapID == id);
        }

        /// <summary>
        /// get suppliers list for create new caps page
        /// </summary>
        /// <param name="selectSupplier"></param>
        private void PopulateSuppliersDropDownList(object selectSupplier = null) {
            var suppliersQuery = from s in _context.Suppliers
                                 orderby s.SupplierName
                                 select s;
            ViewBag.SupplierID = new SelectList(suppliersQuery.AsNoTracking(), "SupplierID", "SupplierName", selectSupplier);
        }

        /// <summary>
        /// get categories list for create new caps page
        /// </summary>
        /// <param name="selectSupplier"></param>
        private void PopulateCategoriesDropDownList(object selectCategory = null)
        {
            var categoriesQuery = from c in _context.Categories
                                 orderby c.CategoryName
                                 select c;
            ViewBag.CategoryID = new SelectList(categoriesQuery.AsNoTracking(), "CategoryID", "CategoryName", selectCategory);
        }

        //for upload image 

        private Dictionary<string, string> mimeTypes = new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };

        private Dictionary<string, string> imageMimeTypes = new Dictionary<string, string>
            {
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
            };

        
      
        private bool IsImageFile(string filename)
        {
            var ext = Path.GetExtension(filename).ToLowerInvariant();
            return imageMimeTypes.ContainsKey(ext);
        }
    }
}
