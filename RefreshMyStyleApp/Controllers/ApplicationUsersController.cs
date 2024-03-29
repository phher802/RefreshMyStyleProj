﻿using System;
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

        // GET: ApplicationUsers
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

            var post = _context.Posts.Where(p => p.ApplicationUserId == applicationUserLoggedIn.Id).FirstOrDefault();

            ApplicationUserImageViewModel applicationUserImageViewModel = new ApplicationUserImageViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Images = _context.Images.Where(i => i.ApplicationUserId == applicationUserLoggedIn.Id).ToList(),
                Likes = _context.LikedItems.Where(x => x.ImageId == applicationUserLoggedIn.Id).ToList(),
                Posts = _context.Posts.Where(x => x.ApplicationUserId == applicationUserLoggedIn.Id).ToList(),
                Comments = GetComments(post),
                //Comments = _context.Comments.Where(x => x.ApplicationUserId == applicationUserLoggedIn.Id).ToList(),
            };

            return View(applicationUserImageViewModel);
        }

        // GET: ApplicationUser/Details/5
        public IActionResult Details(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUserLoggedIn = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var AppUserNotLoggedIn = _context.ApplicationUsers.Find(id);
            // var appUser = _context.ApplicationUsers.Where(a => a.Id == id).SingleOrDefault();
            var post = _context.Posts.Where(p => p.ApplicationUserId == id).FirstOrDefault();
            var currentEvent = _context.Events.Where(e => e.EventCreatorId == id).FirstOrDefault();

            ApplicationUserImageViewModel personViewModel = new ApplicationUserImageViewModel
            {
                AppUserNotLoggedIn = _context.ApplicationUsers.Find(id),
                //AppUserNotLoggedIn = _context.ApplicationUsers.Where(a => a.Id == id).FirstOrDefault(),
                Images = _context.Images.Where(i => i.ApplicationUserId == id).ToList(),
                Event = _context.Events.Where(e => e.EventCreatorId == id).FirstOrDefault(),
                Events = _context.Events.Where(e => e.EventCreatorId == id).ToList(),
                //Attendees = _context.Attendees.Where(x => x.AttendeeId == appUserLoggedIn.Id).ToList(),
                Posts = _context.Posts.Where(x => x.ApplicationUserId == AppUserNotLoggedIn.Id).ToList(),
                Comments = GetComments(post),
                Attendees = GetAttendees(currentEvent),
                //Comment = _context.Comments.Where(x => x.PostId == post.Id).FirstOrDefault(),

            };

            return View(personViewModel);
        }

 
        private List<AttendEvent> GetAttendees(Event newEvent){

            if(newEvent != null)
            {
                return _context.Attendees.Where(x => x.EventId == newEvent.Id).ToList();
            }

            return new List<AttendEvent>();
        }
      

        private List<Comment> GetComments(Post post)
        {
            if (post != null)
            {
               return  _context.Comments.Where(x => x.PostId == post.Id).ToList();
            }
            return new List<Comment>();
        }

        // GET: ApplicationUser/Create
        public IActionResult Create()
        {
            ApplicationUser applicationUser = new ApplicationUser();
            return View(applicationUser);
        }

        // POST: ApplicationUser/Create
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
                Likes = _context.LikedItems.Where(x => x.ApplicationUserId == appUserLoggedIn.Id).ToList(),
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

            LikedItem newLike = new LikedItem();
            newLike.ImageId = currentImage.Id;
            newLike.ImageFilePath = currentImage.ImageFilePath;
            newLike.LikedImageOwnerId = currentImage.ApplicationUserId;
            newLike.LikedImageOwnerFullName = imageOwner.FullName;
            newLike.ApplicationUserId = currentAppUser.Id;
            newLike.IsLiked = true;
            newLike.DateLiked = DateTime.Now;
            newLike.LikedById = currentAppUser.Id;
            newLike.LikedByName = currentAppUser.FullName;

            _context.LikedItems.Add(newLike);
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
                Claims = _context.ClaimedItems.Where(x => x.ApplicationUserId == appUserLoggedIn.Id).ToList(),
            };

            return View(personViewModel);
        }

        //public IActionResult CountClaims(int id)
        //{
        //    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var appUserLoggedIn = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
        //    var images = _context.Images.Where(i => i.ApplicationUserId == appUserLoggedIn.Id).ToList();
        //    var claimed = _context.ClaimedItems.Where(c => c.ClaimedImageOwnerId == appUserLoggedIn.Id).ToList();

        //    if (claimed.Count > 0)
        //    {
        //        return claimed.Count();
        //    }

        //    return View();
        //}

        public IActionResult ClaimImage(int id)
        {
        
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            Image currentImage = _context.Images.Find(id);
            var imageOwner = _context.ApplicationUsers.Where(x => x.Id == currentImage.ApplicationUserId).FirstOrDefault();


            ClaimedItem newClaim = new ClaimedItem();
            newClaim.ImageId = currentImage.Id;
            newClaim.ImageFilePath = currentImage.ImageFilePath;
            newClaim.ClaimedImageOwnerId = currentImage.ApplicationUserId;
            newClaim.ClaimImageOwnerFullName = imageOwner.FullName;
            newClaim.ApplicationUserId = currentAppUser.Id;
            newClaim.ClaimedById = currentAppUser.Id;
            newClaim.ClaimedByName = currentAppUser.FullName;
            newClaim.DateClaimed = DateTime.Now;

            currentImage.IsClaimed = true;
            currentImage.ClaimedById = currentAppUser.Id;
            currentImage.ClaimedByName = currentAppUser.FullName;

            _context.Images.Update(currentImage);
            _context.ClaimedItems.Add(newClaim);
            _context.SaveChanges();
            return RedirectToAction("GetClaims", new { Id = id });
        }

        public IActionResult DeleteClaimedImage(int? id)
        {
            //id = claimed.Id;
            var deleteClaimedImage = _context.ClaimedItems.Find(id);
            var image = _context.Images.Where(x => x.Id == deleteClaimedImage.ImageId).SingleOrDefault();
            image.IsClaimed = false;

            _context.Images.Update(image);
            _context.ClaimedItems.Remove(deleteClaimedImage);
            // _context.Images
            _context.SaveChanges();
            return RedirectToAction(nameof(GetClaims));
        }

        public IActionResult ConfirmClaim(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var claimItem = _context.ClaimedItems.Where(c => c.ClaimedImageOwnerId == currentAppUser.Id).FirstOrDefault();
            var image = _context.Images.Where(c => c.ImageFilePath == claimItem.ImageFilePath).FirstOrDefault();
            image.IsConfirmed = true;
            Message newMessage = new Message();

            if (image.IsConfirmed == true)
            {
                newMessage.ApplicationUserId = currentAppUser.Id;
                newMessage.SenderID = currentAppUser.Id;
                newMessage.SenderName = currentAppUser.FullName;
                newMessage.ReceiverId = claimItem.ClaimedById;
                newMessage.ReceiverName = claimItem.ClaimedByName;
                newMessage.ImageFilePath = image.ImageFilePath;
                newMessage.ImageId = image.Id;
                newMessage.DateMessageSent = DateTime.Now;
                newMessage.MessageContent = "Hello your claim for has been confirmed. Please contact me within 3 days if you're still interested. Thank you.";
                newMessage.ConfirmMsgIsSent = true;
            }

            _context.Messages.Add(newMessage);
            _context.SaveChanges();
            return RedirectToAction("GetMessages", new { Id = id });
        }

        //public List<SelectListItem> GetAllUsers()
        //{
        //    List<ApplicationUser> applicationUsers = _context.ApplicationUsers.Where(a => a.Id > 0).ToList();
        //    //return applicationUsers.OrderByDescending(a => a.LName).ThenBy(a => a.FName).ToList();

        //    List<SelectListItem> users = applicationUsers.ConvertAll(a =>
        //    {
        //        return new SelectListItem()
        //        {
        //            Text = a.Id.ToString(),
        //            Value = a.Id.ToString(),
        //            //Selected = false
        //        };
        //    });

        //    return users;

        //}

        //public IActionResult CreateMessage()
        //{
        //    ViewData["GetAllUsers"] = GetAllUsers();
           
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult CreateMessage(Message message)
        //{
           
        //    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
        //    var AppUserNotLoggedIn = _context.ApplicationUsers.Where(c => c.Id == message.ReceiverId).FirstOrDefault();
        //    var claimItem = _context.ClaimedItems.Where(c => c.ClaimedImageOwnerId == currentAppUser.Id).FirstOrDefault();
        //    var image = _context.Images.Where(c => c.ApplicationUserId == currentAppUser.Id).FirstOrDefault();
        //    //var dropdownList = _context.ApplicationUsers.Where(a => a.FullName == )

        //    Message newMessage = new Message();
        //    newMessage.ApplicationUserId = currentAppUser.Id;
        //    newMessage.SenderID = currentAppUser.Id;
        //    newMessage.SenderName = currentAppUser.FullName;
        //    newMessage.ReceiverId = message.ReceiverId;
        //    newMessage.ReceiverName = message.ReceiverName;
        //    newMessage.ImageFilePath = message.ImageFilePath;
        //    newMessage.ImageId = message.ImageId;
        //    newMessage.DateMessageSent = DateTime.Now;
        //    newMessage.MessageContent = message.MessageContent;


        //    _context.Messages.Add(newMessage);
        //    _context.SaveChanges();

        //    return RedirectToAction(nameof(GetMessages));

        //}


        public IActionResult GetMessages(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            //var messages = _context.Messages.Find(id);
            List<Message> allMessages = GetAllMessages();

            ApplicationUserImageViewModel personViewModel = new ApplicationUserImageViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Images = _context.Images.Where(i => i.ApplicationUserId == id).ToList(),
                //Messages = _context.Messages.Where(m => m.ApplicationUserId == id).ToList(),
                Messages = allMessages,
                //Message = _context.Messages.Where(m => m.ApplicationUserId == id).FirstOrDefault(),
            };

            return View(personViewModel);
        }

        public List<Message> GetAllMessages()
        {
            List<Message> getAllMessages = _context.Messages.Where(m => m.Id > 0).ToList();
            return getAllMessages.OrderByDescending(m => m.DateMessageSent).ToList();
        }

        public IActionResult DeleteMessage(int? id)
        {
            var deleteMsg = _context.Messages.Find(id);
            _context.Messages.Remove(deleteMsg);
            _context.SaveChanges();

            return RedirectToAction(nameof(GetMessages));
        }

        //passes in ClaimedImageId
        public IActionResult GetLikedAndClaimedItems()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            ApplicationUserImageViewModel personViewModel = new ApplicationUserImageViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Images = _context.Images.Where(i => i.ApplicationUserId == currentAppUser.Id).ToList(),
                Likes = _context.LikedItems.Where(c => c.LikedImageOwnerId == currentAppUser.Id).ToList(),
                Claims = _context.ClaimedItems.Where(c => c.ClaimedImageOwnerId == currentAppUser.Id).ToList(),
            };

            return View(personViewModel);
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

        public IActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editPost = _context.Posts.Find(id);

            _context.Posts.Update(editPost);
            _context.SaveChanges();
            return View("Index");
        }
        public IActionResult AddCommentToIndex(int? id, Comment comment)
        {
            id = comment.PostId;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            Post postOwner = _context.Posts.Where(x => x.Id == id).FirstOrDefault();

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

            //id = postOwerId.Id;

            return RedirectToAction("Details", new { Id = id });
        }


        public IActionResult DeleteComment(int id)
        {
            var deleteComment = _context.Comments.Find(id);
            _context.Comments.Remove(deleteComment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteCommentDetails(int id)
        {
            var deleteComment = _context.Comments.Find(id);
            _context.Comments.Remove(deleteComment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Details));
        }

        public IActionResult EventList()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUserLoggedIn = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            //var appUserNotLoggedIn = _context.ApplicationUsers.Find(id);
            var currentEvent = _context.Events.Where(e => e.EventCreatorId == appUserLoggedIn.Id).FirstOrDefault();

            EventViewModel eventViewModel = new EventViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Event = _context.Events.Where(e => e.EventCreatorId == appUserLoggedIn.Id).FirstOrDefault(),
                Events = GetEvents(),
                Attendees = GetAttendees(currentEvent)
               
            };

            return View(eventViewModel);
        }

        private List<Event> GetEvents()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUserLoggedIn = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var events = _context.Events.Where(e => e.EventCreatorId == appUserLoggedIn.Id).ToList();

            if ( events != null)
            {
                return _context.Events.Where(x => x.EventCreatorId == appUserLoggedIn.Id).ToList();
            }
          

            return new List<Event>();
        }

    
        public IActionResult CreateEvent()
        {
            ViewData["states"] = new List<string> { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS",
                "KY", "LA", "ME", "MD", "MA", "MI","MN", "MS", "MO","MT", "NE", "NV","NH", "NJ", "NM","NY", "NC", "ND","OH", "OK", "OR","PA", "RI", "SC","SD",
                "TN", "TX","UT", "VT", "VA","WA", "WV", "WI","WY" };

            return View();
        }

        [HttpPost]
        public IActionResult CreateEvent(int? id, Event newEvent)
        {
            id = newEvent.Id;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAppUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            //var appUserNotLoggedIn = _context.ApplicationUsers.Find(id);

            Event createEvent = new Event();
            createEvent.EventCreatorId = currentAppUser.Id;
            createEvent.EventCreatorName = currentAppUser.FName + " " + currentAppUser.LName;
            createEvent.DatePosted = DateTime.Today;
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
            var attendees = _context.Attendees.Where(a => a.EventId == deleteEvent.Id).ToList();

            foreach (var attendee in attendees)
            {
                _context.Attendees.Remove(attendee);
            }
          
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
            cancelEvent.DateCanceled = DateTime.Today;

            _context.Events.Update(cancelEvent);
            _context.SaveChanges();
            return RedirectToAction(nameof(EventList));

        }

        public IActionResult EditEvent(int? id)
        {
            var currentEvent = _context.Events.Find(id);
            return View(currentEvent);
        }

        [HttpPost]
        public IActionResult EditEvent(int id, Event currentEvent)
        {
            
            var editEvent = _context.Events.Find(id);       
         
            if (ModelState.IsValid)
            {
                try
                {
                 
                    _context.Events.Update(currentEvent);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    {
                        throw;
                    }
                }
                return View("EventList");
            }
            return View(currentEvent);
            //var editEvent = _context.Events.Find(id);

            //_context.Events.Update(editEvent);
            //_context.SaveChanges();
            //return View("EventList");

        }

        //passes in the eventCreator id
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
            //return RedirectToAction("Details", newAttendee);
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
                catch (Exception)
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
            var deleteLikedImage = _context.LikedItems.Find(id);
            var image = _context.Images.Where(i => i.Id == deleteLikedImage.ImageId).FirstOrDefault();
            image.IsLiked = false;

            _context.Images.Update(image);
            _context.LikedItems.Remove(deleteLikedImage);
            _context.SaveChanges();
            return RedirectToAction(nameof(GetLikes));
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
