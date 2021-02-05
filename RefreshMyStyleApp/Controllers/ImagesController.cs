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
            var applicationDbContext = _context.Images.Include(i => i.LikedList).Include(i => i.Person);
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
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.People, "Id", "Id");
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,ImageTitle,FilePath,ClothingCategory,Color,Size,Description,ToShare,ToGiveAway,Id")] Image image)
        {
            if (ModelState.IsValid)
            {
                _context.Add(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.People, "Id", "Id", image.Id);
            return View(image);
        }

        public IActionResult PostImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostImage([Bind("Id, ImageTitle")] List<IFormFile> files)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var person = _context.People.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var img = _context.Images.Where(i => i.ImageId == person.Id).FirstOrDefault();

            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePaths = Path.GetTempFileName();

            foreach ( var formfile in files)
            {
                var uniqueName = GetUniqueFileName(files[0].FileName);
                var uploads = Path.Combine(_env.WebRootPath, "images/items");
                var imgTitle = Path.Combine(uploads, uniqueName);
                await files[0].CopyToAsync(new FileStream(imgTitle, FileMode.Create));
                img.ImageTitle = uniqueName;
                
            }

            _context.Update(img);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return View();
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
            ViewData["Id"] = new SelectList(_context.People, "Id", "Id", image.Id);
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ImageId,ImageTitle,FilePath,ClothingCategory,Color,Size,Description,ToShare,ToGiveAway,Id")] Image image)
        {
            if (id != image.ImageId)
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
                    if (!ImageExists(image.ImageId))
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
            ViewData["Id"] = new SelectList(_context.People, "Id", "Id", image.Id);
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
                .FirstOrDefaultAsync(m => m.ImageId == id);
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
            return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
