using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RefreshMyStyleApp.Data;
using RefreshMyStyleApp.Models;

namespace RefreshMyStyleApp.Controllers
{
    public class ClothingEnthusiastsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClothingEnthusiastsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClothingEnthusiasts
        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.ClothingEnthusiast.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            if (user == null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(user);

            //var applicationDbContext = _context.ClothingEnthusiast.Include(c => c.Event).Include(c => c.FriendsList).Include(c => c.Image).Include(c => c.ProfileImage);
            //return View(await applicationDbContext.ToListAsync());

        }

        // GET: ClothingEnthusiasts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingEnthusiast = await _context.ClothingEnthusiast
                .Include(c => c.Event)
                .Include(c => c.FriendsList)
                .Include(c => c.Image)
                .Include(c => c.ProfileImage)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (clothingEnthusiast == null)
            {
                return NotFound();
            }

            return View(clothingEnthusiast);
        }

        [HttpPost]
        public IActionResult UpoadImage()
        {
            foreach(var file in Request.Form.Files)
            {
                Image img = new Image();
                img.ImageTitle = file.FileName;

                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                img.ImageData = ms.ToArray();

                ms.Close();
                ms.Dispose();

                _context.Images.Add(img);
                _context.SaveChanges(); 

            }

            ViewBag.Message = "Image(s) stored in database!";
            return View("Index");
        
        }
         
        [HttpPost]
        public IActionResult RetreiveImage()
        {
            Image img = _context.Images.OrderByDescending(i => i.ImageId).SingleOrDefault();
            string imageBase64Data = Convert.ToBase64String(img.ImageData);
            string imageDataURL = string.Format("data:image/jpg;base64, {0}", imageBase64Data);

            ViewBag.ImageTitle = img.ImageTitle;
            ViewBag.ImageDataURL = imageDataURL;
            return View("Index");
        }


        // GET: ClothingEnthusiasts/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Set<Event>(), "EventId", "EventId");
            ViewData["FriendsListId"] = new SelectList(_context.Set<FriendsList>(), "FriendsListId", "FriendsListId");
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId");
            ViewData["ProfileImageId"] = new SelectList(_context.profileImages, "ProfileImageId", "ProfileImageId");
            return View();



        }

        // POST: ClothingEnthusiasts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("UserId,FName,LName,PhoneNumber,ProfileImageId,ImageId,EventId,FriendsListId")] ClothingEnthusiast clothingEnthusiast)
        {
            if (clothingEnthusiast != null)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                clothingEnthusiast.IdentityUserId = userId;
                _context.Add(clothingEnthusiast);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(clothingEnthusiast);

            //if (ModelState.IsValid)
            //{
            //    _context.Add(clothingEnthusiast);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["EventId"] = new SelectList(_context.Set<Event>(), "EventId", "EventId", clothingEnthusiast.EventId);
            //ViewData["FriendsListId"] = new SelectList(_context.Set<FriendsList>(), "FriendsListId", "FriendsListId", clothingEnthusiast.FriendsListId);
            //ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId", clothingEnthusiast.ImageId);
            //ViewData["ProfileImageId"] = new SelectList(_context.profileImages, "ProfileImageId", "ProfileImageId", clothingEnthusiast.ProfileImageId);

        }

        // GET: ClothingEnthusiasts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingEnthusiast = await _context.ClothingEnthusiast.FindAsync(id);
            if (clothingEnthusiast == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Set<Event>(), "EventId", "EventId", clothingEnthusiast.EventId);
            ViewData["FriendsListId"] = new SelectList(_context.Set<FriendsList>(), "FriendsListId", "FriendsListId", clothingEnthusiast.FriendsListId);
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId", clothingEnthusiast.ImageId);
            ViewData["ProfileImageId"] = new SelectList(_context.profileImages, "ProfileImageId", "ProfileImageId", clothingEnthusiast.ProfileImageId);
            return View(clothingEnthusiast);
        }

        // POST: ClothingEnthusiasts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FName,LName,PhoneNumber,ProfileImageId,ImageId,EventId,FriendsListId")] ClothingEnthusiast clothingEnthusiast)
        {
            if (id != clothingEnthusiast.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clothingEnthusiast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClothingEnthusiastExists(clothingEnthusiast.UserId))
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
            ViewData["EventId"] = new SelectList(_context.Set<Event>(), "EventId", "EventId", clothingEnthusiast.EventId);
            ViewData["FriendsListId"] = new SelectList(_context.Set<FriendsList>(), "FriendsListId", "FriendsListId", clothingEnthusiast.FriendsListId);
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId", clothingEnthusiast.ImageId);
            ViewData["ProfileImageId"] = new SelectList(_context.profileImages, "ProfileImageId", "ProfileImageId", clothingEnthusiast.ProfileImageId);
            return View(clothingEnthusiast);
        }

        // GET: ClothingEnthusiasts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingEnthusiast = await _context.ClothingEnthusiast
                .Include(c => c.Event)
                .Include(c => c.FriendsList)
                .Include(c => c.Image)
                .Include(c => c.ProfileImage)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (clothingEnthusiast == null)
            {
                return NotFound();
            }

            return View(clothingEnthusiast);
        }

        // POST: ClothingEnthusiasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clothingEnthusiast = await _context.ClothingEnthusiast.FindAsync(id);
            _context.ClothingEnthusiast.Remove(clothingEnthusiast);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClothingEnthusiastExists(int id)
        {
            return _context.ClothingEnthusiast.Any(e => e.UserId == id);
        }
    }
}
