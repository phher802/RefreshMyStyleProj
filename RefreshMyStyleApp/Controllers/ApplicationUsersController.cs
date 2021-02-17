using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using ClientNotifications;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RefreshMyStyleApp.Data;
using RefreshMyStyleApp.Hubs;
using RefreshMyStyleApp.Models;

using RefreshMyStyleApp.ViewModels;

namespace RefreshMyStyleApp.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<NotificationHub> _notiHubContext;


        public ApplicationUsersController(ApplicationDbContext context, IWebHostEnvironment env, IHubContext<NotificationHub> notificationHubContext)
        {
            _context = context;
            _env = env;
            _notiHubContext = notificationHubContext;
        }

        // GET: People
        public IActionResult Index()
        {
            ApplicationUser applicationUserLoggedIn = new ApplicationUser();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            applicationUserLoggedIn = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            if (applicationUserLoggedIn == null)
            {
                return RedirectToAction(nameof(Create));
            }
            applicationUserLoggedIn.FullName = applicationUserLoggedIn.FName + " " + applicationUserLoggedIn.LName;
            _context.ApplicationUsers.Update(applicationUserLoggedIn);
            _context.SaveChanges();

            ApplicationUserImageViewModel applicationUserImageViewModel = new ApplicationUserImageViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Images = _context.Images.Where(i => i.ApplicationUserId == applicationUserLoggedIn.Id).ToList(),
                Friends = _context.Friends.Where(f => f.Id == applicationUserLoggedIn.Id).ToList(),
                Likes = _context.Likes.Where(x => x.ImageId == applicationUserLoggedIn.Id).ToList(),
            };
            return View(applicationUserImageViewModel);
        }


        // GET: People/Details/5
        public IActionResult Details(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUserLoggedIn = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            //// var appUsers = _context.ApplicationUsers.Where(x => x.Id == id).FirstOrDefault();
            ApplicationUser appUserdetails = _context.ApplicationUsers.Find(id);

            ApplicationUserImageViewModel personViewModel = new ApplicationUserImageViewModel
            {
                //ApplicationUser = _context.ApplicationUsers.Where(c => c.Id == appUserLoggedIn.Id).SingleOrDefault(), 
                AppUserNotLoggedIn = _context.ApplicationUsers.Find(id),
                Images = _context.Images.Where(i => i.ApplicationUserId == id).ToList(),
                //ApplicationUsers = _context.ApplicationUsers.Where(c => c.Id == appUsers.Id).ToList(),
                //SearchUsers = _context.ApplicationUsers.Where(x => x.Id == appUserNotLoggedIn.Id).ToList(),
            };

            return View(personViewModel);
        }


        // GET: People/Create
        public IActionResult Create()
        {
            ApplicationUser applicationUser = new ApplicationUser();
            return View(applicationUser);

        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                applicationUser.IdentityUserId = userId;

                _context.ApplicationUsers.Add(applicationUser);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(applicationUser);

        }

        public IActionResult SearchUsers()
        {
            var user1 = _context.ApplicationUsers.Where(a => a.IdentityUserId == this.User.FindFirstValue(ClaimTypes.NameIdentifier)).Single();
            user1.SearchResults = _context.ApplicationUsers.ToList();

            return View(user1);
        }

        [HttpPost]
        public IActionResult SearchUsers(string searchString)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userLoggedIn = _context.ApplicationUsers.Where(f => f.IdentityUserId == userId).FirstOrDefault();

            var searchUsers = from m in _context.ApplicationUsers select m;
            //var searchUsers = _context.ApplicationUsers.Where(x => x.IdentityUserId != userId).FirstOrDefault();

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchString != userLoggedIn.FName || searchString != userLoggedIn.LName)
                {
                    searchUsers = searchUsers.Where(x => x.FName.Contains(searchString.ToLower()) ||
                                      x.LName.Contains(searchString.ToLower()));                                    
                }
            }         
            userLoggedIn.SearchResults = searchUsers.ToList();
             return View(userLoggedIn);           
        }

  
        //GET
        public IActionResult SearchImages()
        {
            return View();
        }

        //POST
        public IActionResult SearchImages(string searchString)
        {       
            var image = from i in _context.Images select i;

            if (!String.IsNullOrEmpty(searchString))
            {              
                image = image.Where(x => x.ImageName.Contains(searchString.ToLower()));
                
            }
            return View(image.ToList());
        }

        ////GET
        //public IActionResult AddFriendRequest()
        //{
        //    return View();
        //}

        //POST
        public IActionResult AddFriendRequest(ApplicationUser user, ApplicationUser friendUser)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            user = _context.ApplicationUsers.Where(f => f.IdentityUserId == userId).FirstOrDefault();
            //friendUser = _context.ApplicationUsers.Select(x => x.Id);

            var friendRequest = new Friend()
            {
                RequestedBy = user,
                RequestedTo = friendUser,
                RequestTime = DateTime.Now,
                FriendRequestFlag = FriendRequestFlag.None
            };
            user.SentFriendRequests.Add(friendRequest);

            return View("SearchUsers", friendRequest);
                    
        }

        public async Task<List<Friend>> Friend()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            var friends = applicationUser.SentFriendRequests.Where(x => x.Approved).ToList();
            //var friends = _context.Friends.Where(x => x.SentFriendRequests == )
            friends.AddRange(applicationUser.ReceievedFriendRequests.Where(x => x.Approved));

            return friends;
        } 

        public IActionResult UploadProfilefiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileFiles([Bind("Id, ProfileImageName")] List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            var uniqueName = GetUniqueFileName(files[0].FileName);
            var uploads = Path.Combine(_env.WebRootPath, "images/profileImages");
            var ImageName = Path.Combine(uploads, uniqueName);
            await files[0].CopyToAsync(new FileStream(ImageName, FileMode.Create));


            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            applicationUser.ProfileImageName = uniqueName;

            _context.ApplicationUsers.Update(applicationUser);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

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

        public IActionResult LikeImage(int imageId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            Image likedImage = _context.Images.Where(c => c.Id == imageId).SingleOrDefault();
            Like newLike = new Like();
            newLike.ImageId = likedImage.Id;
            newLike.ApplicationUserId = applicationUser.Id;
            newLike.IsLiked = true;
            newLike.DateLiked = DateTime.Now;


            _context.Update(newLike);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    

        // GET: ClothingEnthusiasts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUsers.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return View(applicationUser);
        }

        // POST: ClothingEnthusiasts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FName,LName,PhoneNumber,ProfileImageId,ImageId,EventId,FriendsListId")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUsersExists(applicationUser.Id))
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
            return View(applicationUser);
        }

        // GET: ClothingEnthusiasts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ClothingEnthusiasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationUser = await _context.ApplicationUsers.FindAsync(id);
            _context.ApplicationUsers.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUsersExists(int id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }
    }
}
