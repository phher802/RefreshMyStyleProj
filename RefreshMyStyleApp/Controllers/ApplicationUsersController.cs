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
using Microsoft.EntityFrameworkCore;
using RefreshMyStyleApp.Data;
using RefreshMyStyleApp.Models;
using RefreshMyStyleApp.ViewModels;

namespace RefreshMyStyleApp.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        //private readonly IClientNotification _clientNotification;
        //private readonly Notification _Notification;


        public ApplicationUsersController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            //_clientNotification = clientNotification;
            //_Notification = notification;

        }


        // GET: People
        public IActionResult Index()
        {       
            ApplicationUser applicationUser = new ApplicationUser();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            applicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            if (applicationUser == null)
            {
                return RedirectToAction(nameof(Create));
            }

            applicationUser.FullName = applicationUser.FName + " " + applicationUser.LName;
            //Image image = new Image();
            //image.ApplicationUserId = applicationUser.Id;

            ApplicationUserImageViewModel applicationUserImageViewModel = new ApplicationUserImageViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Images = _context.Images.Where(i => i.ApplicationUserId == applicationUser.Id).ToList(),
                              
            };

            return View(applicationUserImageViewModel);

            //return View(applicationUser);
        }

        // GET: People/Details/5
        public IActionResult Details(int? id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            ApplicationUserImageViewModel personViewModel = new ApplicationUserImageViewModel
            {
                ApplicationUser = _context.ApplicationUsers.Where(c => c.IdentityUserId == userId).FirstOrDefault(),
                Images = _context.Images.Where(i => i.ApplicationUserId == applicationUser.Id).ToList()
            };

            return View(personViewModel);

        //if (id == null)
        //{
        //    return NotFound();
        //}

        //var person = await _context.People
        //    .FirstOrDefaultAsync(m => m.Id == id);
        //if (person == null)
        //{
        //    return NotFound();
        //}

        //return View(person);
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
    public IActionResult Create([Bind("FName,LName,PhoneNumber,ImageName")] ApplicationUser applicationUser)
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
