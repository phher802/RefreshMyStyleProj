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
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RefreshMyStyleApp.Data;
using RefreshMyStyleApp.Hubs;
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
            var applicationDbContext = _context.Images.Include(i => i.Likes).Include(i => i.ApplicationUser);
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
                .Include(i => i.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }


        // GET: Images/Create
        public IActionResult Create(Image image, int imageId)
        {

            ViewData["ClothingCategory"] = ClothingCategory();
            ViewData["ItemStatus"] = ItemStatus();
            return View(image);
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Image image)
        {

            if (ModelState.IsValid)
            {

                _context.Images.Update(image);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ApplicationUsers");
            }

            return View(image);
        }

        public IActionResult PostNewImage()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostNewImage(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            var uniqueName = GetUniqueFileName(files[0].FileName);
            var uploads = Path.Combine(_env.WebRootPath, "images/items");
            var ImageName = Path.Combine(uploads, uniqueName);
            await files[0].CopyToAsync(new FileStream(ImageName, FileMode.Create));

            Image newImage = new Image();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            newImage.ApplicationUserId = applicationUser.Id;
            newImage.ImageTitle = uniqueName;
                           
            _context.Images.Add(newImage);
            _context.SaveChanges();
            return RedirectToAction("Create", newImage);
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

        public IActionResult AddLike()
        {
            var user1 = _context.ApplicationUsers.Where(a => a.IdentityUserId == this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Single();
            var image = _context.Images.Where(x => x.Id == user1.Id).SingleOrDefault();
            image.Likes = image.Likes.ToList();
            
            return View(image);
        }

        public IActionResult AddLike(int imageId, int id)
        {
            //get image
            //get appUser that liked image
            //add image and appUser to like table
           // var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationUser = _context.ApplicationUsers.Where(c => c.Id == id).FirstOrDefault();
            Image likeImage = _context.Images.Where(i => i.Id == imageId).SingleOrDefault();
            LikedItem likeInDb = new LikedItem();

            likeInDb.Id = likeImage.Id;
            likeInDb.ApplicationUserId = applicationUser.Id;
            likeInDb.IsLiked = true;
            likeInDb.DateLiked = DateTime.Now;

            _context.LikedItems.Update(likeInDb);
            _context.SaveChanges();
            return RedirectToAction("Index", "ApplicationUsers", likeInDb);

        }

        public List<LikedItem> GetLikes()
        {
            List<LikedItem> likes = _context.LikedItems.Where(l => l.Id > 0).ToList();
            return likes.OrderByDescending(l => l.ImageId).ThenBy(l => l.DateLiked).ToList();
        }
        public List<SelectListItem> ClothingCategory()
        {
            List<SelectListItem> category = new List<SelectListItem>();
            category.Add(new SelectListItem { Text = "Casual", Value = "Casual" });
            category.Add(new SelectListItem { Text = "Special Occasion", Value = "Special Occasion" });
            category.Add(new SelectListItem { Text = "Shoes", Value = "Shoes" });
            category.Add(new SelectListItem { Text = "Hats", Value = "Hats" });
            category.Add(new SelectListItem { Text = "Jewelry", Value = "Jewelry" });
            return category;
        }

        public List<SelectListItem> ItemStatus()
        {
            List<SelectListItem> itemStatus = new List<SelectListItem>();
            itemStatus.Add(new SelectListItem { Text = "Share", Value = "Share" });
            itemStatus.Add(new SelectListItem { Text = "Give", Value = "Give" });
            return itemStatus;
        }

        // GET: Images/Edit/5
        public async Task<IActionResult> EditImage(int? id)
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
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditImage(int? id, Image image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Image editImage = _context.Images.Find(id);
                editImage.ClothingCategory = image.ClothingCategory;
                editImage.Color = image.Color;
                editImage.Description = image.Description;
                editImage.Size = image.Size;
                editImage.ItemStatus = image.ItemStatus;

                _context.Images.Update(editImage);
                _context.SaveChanges();
                return RedirectToAction("Index" , "ApplicationUsers");
            }

            return View(image);
        }



        public List<ClaimedItem> GetClaims()
        {
            List<ClaimedItem> claimed = _context.ClaimedItems.Where(c => c.Id > 0).ToList();
            return claimed.OrderByDescending(c => c.ImageId).ThenBy(c => c.DateClaimed).ToList();
        }


        // GET: Images/Delete/5
        public IActionResult DeleteImage(int? id)
        {
            var deleteImage = _context.Images.Find(id);
            _context.Images.Remove(deleteImage);
            _context.SaveChanges();
            return RedirectToAction("Index", "ApplicationUsers");

           
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var image = _context.Images.Where(i => i.ApplicationUserId == applicationUser.Id).FirstOrDefault();
            //image = await _context.Images.FindAsync(id);

            var imagePath = Path.Combine(_env.WebRootPath, "images/items", image.ImageTitle);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

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
