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


        public ApplicationUsersController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

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

            var appUser = _context.ApplicationUsers.Where(a => a.Id == applicationUserLoggedIn.Id).SingleOrDefault();
            var post = _context.Posts.Where(p => p.ApplicationUserId == appUser.Id).FirstOrDefault();

            ApplicationUserImageViewModel applicationUserImageViewModel = new ApplicationUserImageViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Images = _context.Images.Where(i => i.ApplicationUserId == applicationUserLoggedIn.Id).ToList(),
                Likes = _context.Likes.Where(x => x.ImageId == applicationUserLoggedIn.Id).ToList(),
                Posts = _context.Posts.Where(x => x.ApplicationUserId == applicationUserLoggedIn.Id).ToList(),
                Comments = _context.Comments.Where(x => x.ApplicationUserId == applicationUserLoggedIn.Id).ToList(),
                //Comments = _context.Comments.Where(x => x.PostId == post.Id).ToList(),
            };
            return View(applicationUserImageViewModel);
        }


        // GET: People/Details/5
        public IActionResult Details(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUserLoggedIn = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var AppUserNotLoggedIn = _context.ApplicationUsers.Find(id);
            var appUser = _context.ApplicationUsers.Where(a => a.Id == id).SingleOrDefault();
            var post = _context.Posts.Where(p => p.ApplicationUserId == appUser.Id).FirstOrDefault();

            ApplicationUserImageViewModel personViewModel = new ApplicationUserImageViewModel
            {
                AppUserNotLoggedIn = _context.ApplicationUsers.Find(id),
                Images = _context.Images.Where(i => i.ApplicationUserId == id).ToList(),
                Event = _context.Events.Where(e => e.EventCreatorId == id).FirstOrDefault(),
                Events = _context.Events.Where(e => e.EventCreatorId == id).ToList(),
                Attendees = _context.Attendees.Where(x => x.EventId == id).ToList(),
                Posts = _context.Posts.Where(x => x.ApplicationUserId == id).ToList(),
                Comments = _context.Comments.Where(x => x.PostId == post.Id).ToList(),

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


        public IActionResult UploadProfilefiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileFiles(List<IFormFile> files)
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

        public IActionResult GetLikes(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUserLoggedIn = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            ApplicationUserImageViewModel personViewModel = new ApplicationUserImageViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Images = _context.Images.Where(i => i.ApplicationUserId == id).ToList(),
                Likes = _context.Likes.Where(x => x.ApplicationUserId == appUserLoggedIn.Id).ToList(),
            };
            return View(personViewModel);
        }

        public IActionResult LikeImage(int id)
        {
            //get image
            //get appUser that liked image
            //add image and appUser to like table
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            Image currentImage = _context.Images.Find(id);
            var imageOwner = _context.ApplicationUsers.Where(x => x.Id == currentImage.ApplicationUserId).FirstOrDefault();

            Like newLike = new Like();
            newLike.ImageId = currentImage.Id;
            newLike.ImageTitle = currentImage.ImageTitle;
            newLike.UserId = currentImage.ApplicationUserId;
            newLike.LikedImageOwnerFullName = imageOwner.FullName;
            newLike.ApplicationUserId = currentAppUser.Id;
            newLike.IsLiked = true;
            newLike.DateLiked = DateTime.Now;

            _context.Likes.Add(newLike);
            _context.SaveChanges();
            return RedirectToAction("GetLikes", new { Id = id });
        }

        public IActionResult GetClaims(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUserLoggedIn = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            ApplicationUserImageViewModel personViewModel = new ApplicationUserImageViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Images = _context.Images.Where(i => i.ApplicationUserId == id).ToList(),
                Claims = _context.ClaimItems.Where(x => x.ApplicationUserId == appUserLoggedIn.Id).ToList(),
            };

            return View(personViewModel);
        }

        public IActionResult ClaimImage(int id, Claimed currentClaim)
        {
            //get image
            //get appUser that liked image
            //add image and appUser to like table
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            Image currentImage = _context.Images.Find(id);
            var imageOwner = _context.ApplicationUsers.Where(x => x.Id == currentImage.ApplicationUserId).FirstOrDefault();

            Claimed newClaim = new Claimed();
            newClaim.ImageId = currentImage.Id;
            newClaim.ImageTitle = currentImage.ImageTitle;
            newClaim.UserId = currentImage.ApplicationUserId;
            newClaim.ClaimImageOwnerFullName = imageOwner.FullName;
            newClaim.ApplicationUserId = currentAppUser.Id;
            newClaim.ClaimedById = currentAppUser.Id;
            newClaim.IsClaimed = true;
            newClaim.DateClaimed = DateTime.Now;
            currentImage.IsClaimed = true;

            _context.Images.Update(currentImage);
            _context.ClaimItems.Add(newClaim);
            _context.SaveChanges();
            return RedirectToAction("GetClaims", new { Id = id });
        }

        public IActionResult ConfirmClaim(int id)
        {
            //var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var image = _context.Images.Where(c => c.ApplicationUserId == id).FirstOrDefault();


            if (image.IsClaimed)
            {
                //  image.Claimed += 25.00;
                _context.Update(image);
                _context.SaveChanges();

            }

            return RedirectToAction("GetLikedAndClaimed");
        }

        public IActionResult GetLikedAndClaimed(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var claimedImages = _context.Images.Find(id);
            var claimedItem = _context.Images.Where(c => c.IsClaimed == true).FirstOrDefault(c => c.Id == claimedImages.Id);


            ApplicationUserImageViewModel personViewModel = new ApplicationUserImageViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Images = _context.Images.Where(i => i.ApplicationUserId == id).ToList(),
                Claimed = _context.ClaimItems.Find(id),
                Claims = _context.ClaimItems.Where(c => c.ImageId == claimedItem.Id).ToList(),
            };
            return View();
        }
        public IActionResult NewPost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewPost(Post Post)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            Post newPost = new Post();
            newPost.ApplicationUserId = appUser.Id;
            newPost.DateTimePosted = DateTime.Now;
            newPost.PostTitle = Post.PostTitle;
            newPost.PostContent = Post.PostContent;
            newPost.PostByUser = appUser.FName + " " + appUser.LName;

            _context.Posts.Add(newPost);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeletePost(int id)
        {
            var deletePost = _context.Posts.Find(id);
            _context.Posts.Remove(deletePost);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddcommentToIndex(Comment comment)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            Post postOwner = _context.Posts.Where(x => x.Id == comment.PostId).FirstOrDefault();

            Comment newComment = new Comment();
            newComment.PostId = comment.PostId;
            newComment.ApplicationUserId = appUser.Id;
            newComment.CommentorFullName = appUser.FName + " " + appUser.LName;
            newComment.CommentDateTime = DateTime.Now;
            newComment.CommentContent = comment.CommentContent;

            _context.Comments.Add(newComment);
            _context.SaveChanges();
            //return RedirectToAction();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddCommentToDetails(int? id, Comment comment)
        {
            id = comment.PostId;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var post = _context.Posts.Find(id);
            var postOwner = _context.Posts.Where(a => a.ApplicationUserId == post.ApplicationUserId).FirstOrDefault();
            var postOwerId = _context.ApplicationUsers.Where(a => a.Id == postOwner.ApplicationUserId).SingleOrDefault();

            Comment newComment = new Comment();
            newComment.PostId = post.Id;
            newComment.ApplicationUserId = appUser.Id;
            newComment.CommentorFullName = appUser.FName + " " + appUser.LName;
            newComment.CommentDateTime = DateTime.Now;
            newComment.CommentContent = comment.CommentContent;

            _context.Comments.Add(newComment);
            _context.SaveChanges();
            //return RedirectToAction();

            id = postOwerId.Id;

            return RedirectToAction("Details", new { Id = id });
        }



        public IActionResult DeleteComment(int id)
        {
            var deleteComment = _context.Comments.Find(id);
            _context.Comments.Remove(deleteComment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult EventList(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUserLoggedIn = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var appUserNotLoggedIn = _context.ApplicationUsers.Find(id);
            var currentEvent = _context.Events.Find(id);

            EventViewModel eventViewModel = new EventViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Event = _context.Events.Where(e => e.EventCreatorId == appUserLoggedIn.Id).FirstOrDefault(),
                Events = _context.Events.Where(e => e.EventCreatorId == appUserLoggedIn.Id).ToList(),
                Attendees = _context.Attendees.Where(x => x.AttendeeId == appUserLoggedIn.Id).ToList(),
            };
            return View(eventViewModel);
        }

        public IActionResult CreateEvent()
        {
            ViewData["states"] = new List<string> { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS",
                "KY", "LA", "ME", "MD", "MA", "MI","MN", "MS", "MO","MT", "NE", "NV","NH", "NJ", "NM","NY", "NC", "ND","OH", "OK", "OR","PA", "RI", "SC","SD",
                "TN", "TX","UT", "VT", "VA","WA", "WV", "WI","WY" };

            return View();
        }

        [HttpPost]
        public IActionResult CreateEvent(Event newEvent)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            //var appUserNotLoggedIn = _context.ApplicationUsers.Find(id);

            Event createEvent = new Event();
            createEvent.EventCreatorId = currentAppUser.Id;
            createEvent.EventCreatorName = currentAppUser.FName + " " + currentAppUser.LName;
            createEvent.DatePosted = DateTime.Now;
            createEvent.EventDate = newEvent.EventDate;
            createEvent.EventTitle = newEvent.EventTitle;
            createEvent.Message = newEvent.Message;
            createEvent.Address = newEvent.StreetAddress + ", " + newEvent.City + ", " + newEvent.State + " " + newEvent.Zipcode;


            _context.Events.Add(createEvent);
            _context.SaveChanges();
            return RedirectToAction(nameof(EventList));
        }

        public IActionResult DeleteEvent(int id)
        {
            var deleteEvent = _context.Events.Find(id);
            _context.Events.Remove(deleteEvent);
            _context.SaveChanges();
            return RedirectToAction(nameof(EventList));
        }

        public IActionResult CancelEvent(int id)
        {
            var cancelEvent = _context.Events.Find(id);
            cancelEvent.IsCanceled = true;
            cancelEvent.Message = "This event has been canceled.";
            cancelEvent.Address = "Not Available";
            cancelEvent.DateCanceled = DateTime.Now;

            _context.Events.Update(cancelEvent);
            _context.SaveChanges();
            return RedirectToAction(nameof(EventList));

        }

        public IActionResult GetAttendee(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            AttendEvent newAttendee = new AttendEvent();
            newAttendee.AttendeeId = currentAppUser.Id;
            newAttendee.AttendeeName = currentAppUser.FName + " " + currentAppUser.LName;
            newAttendee.EventId = id;

            _context.Attendees.Add(newAttendee);
            _context.SaveChanges();

            // return RedirectToAction(nameof(EventList));
            return RedirectToAction("Details", new { Id = id });
        }

        // GET: ApplicationUser/Edit/5
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

        // POST: ApplicationUser/Edit/5
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

        public IActionResult DeleteLikedImage(int id)
        {
            var deleteLikedImage = _context.Likes.Find(id);
            _context.Likes.Remove(deleteLikedImage);
            _context.SaveChanges();
            return RedirectToAction(nameof(GetLikes));
        }

        public IActionResult DeleteClaimedImage(int id)
        {
            var deleteClaimedImage = _context.ClaimItems.Find(id);
            _context.ClaimItems.Remove(deleteClaimedImage);
            _context.SaveChanges();
            return RedirectToAction(nameof(GetClaims));
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
