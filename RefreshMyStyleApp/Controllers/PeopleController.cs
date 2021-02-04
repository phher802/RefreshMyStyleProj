using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PeopleController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: People
        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var person = _context.People.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            if (person == null)
            {
                return RedirectToAction(nameof(Create));
            }
        

            return View(person);


        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            Person person = new Person();
            return View(person);

        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FName,LName,PhoneNumber,ImageName")] Person person)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                person.IdentityUserId = userId;
                _context.Add(person);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(person);

        }

        public IActionResult Uploadfiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles([Bind("Id, ImageName")] List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            //foreach (var formFile in files)
            //{
            //var uniqueName = GetUniqueFileName(formFile.FileName);
            //var uploads = Path.Combine(_env.WebRootPath, "images/profileImages");
            //var ImageName = Path.Combine(uploads, uniqueName);
            //await formFile.CopyToAsync(new FileStream(ImageName, FileMode.Create));
            //}
            var uniqueName = GetUniqueFileName(files[0].FileName);
            var uploads = Path.Combine(_env.WebRootPath, "images/profileImages");
            var ImageName = Path.Combine(uploads, uniqueName);
            await files[0].CopyToAsync(new FileStream(ImageName, FileMode.Create));


            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var person = _context.People.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            person.ImageName = uniqueName;


            _context.Update(person);      
            _context.SaveChanges();
            //_context.Entry(person).Property("ImageName").IsModified = true;
            return RedirectToAction(nameof(Index));
            //return View("Index");
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

        // GET: ClothingEnthusiasts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: ClothingEnthusiasts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FName,LName,PhoneNumber,ProfileImageId,ImageId,EventId,FriendsListId")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeopleExists(person.Id))
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
            return View(person);
        }

        // GET: ClothingEnthusiasts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: ClothingEnthusiasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.FindAsync(id);
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeopleExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
