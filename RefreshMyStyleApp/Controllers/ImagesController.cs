using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RefreshMyStyleApp.Data;
using RefreshMyStyleApp.Models;

namespace RefreshMyStyleApp.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ImagesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Images.Include(i => i.Likes).Include(i => i.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
           // ViewData["Id"] = new SelectList(_context.People, "Id", "Id");
            ViewData["ClothingCategory"] = ClothingCategory();
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,ImageTitle,FilePath,ClothingCategory,Color," +
                                                        "Size,Description,ToShare,ToGiveAway,Id")] Image image)
        {
           
            if (ModelState.IsValid)
            {
                Image imgInDb = new Image();
                imgInDb.ImageName = image.ImageName;
                imgInDb.ClothingCategory = image.ClothingCategory;
                imgInDb.Color = image.Color;
                imgInDb.Description = image.Description;
                imgInDb.Size = image.Size;

                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var person = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
                imgInDb.PersonId = person.Id;
                             
                _context.Images.Add(imgInDb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PostNewImage));
            }

            //ViewData["Id"] = new SelectList(_context.People, "Id", "Id", image.PersonId);

         
            return View(image);
        }

        public List<SelectListItem> ClothingCategory()
        {
            List<SelectListItem> category = new List<SelectListItem>();
            category.Add(new SelectListItem { Text = "Casual", Value = "Casual" });
            category.Add(new SelectListItem { Text = "Special Occasion", Value = "Special Occasion" });
            category.Add(new SelectListItem { Text = "Shoes", Value = "Shoes" });
            category.Add(new SelectListItem { Text = "Hats", Value = "Hats" });
            category.Add(new SelectListItem { Text = "Jewelry", Value = "Jewelry"});
            return category;
        }

        public IActionResult PostNewImage()
        {
         
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostNewImage([Bind("Id, ImageTitle")] List<IFormFile> files, Image newImage)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            var uniqueName = GetUniqueFileName(files[0].FileName);
            var uploads = Path.Combine(_env.WebRootPath, "images/items");
            var ImageName = Path.Combine(uploads, uniqueName);
            await files[0].CopyToAsync(new FileStream(ImageName, FileMode.Create));

            //Image newImage = new Image();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var person = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            newImage.PersonId = person.Id;

            newImage.ImageTitle = uniqueName;

            _context.Images.Update(newImage);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

            //return RedirectToAction(nameof(Index));
           // return View();
            //return CreatedAtAction("GetImage", new { id = image.ImageId }, image);
        }

        private string GetUniqueFileName(string fileName)
        {
            if (fileName.Length > 0)
            {
                fileName = Path.GetFileName(fileName);
                return Path.GetFileNameWithoutExtension(fileName)
                          + "_"
                          + Guid.NewGuid().ToString().Substring(0, 4)
                          + Path.GetExtension(fileName);
            }
            return string.Empty;

        }

        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.ApplicationUsers, "Id", "Id", image.PersonId);
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ImageId,ImageTitle,FilePath,ClothingCategory,Color,Size,Description,ToShare,ToGiveAway,Id")] Image image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.Id))
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
            ViewData["Id"] = new SelectList(_context.ApplicationUsers, "Id", "Id", image.PersonId);
            return View(image);
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var image = await _context.Images.FindAsync(id);
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int? id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
