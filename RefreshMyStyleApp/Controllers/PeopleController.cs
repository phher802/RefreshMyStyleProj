using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
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
            // ViewData["EventId"] = new SelectList(_context.Set<Event>(), "EventId", "EventId");
            //ViewData["FriendsListId"] = new SelectList(_context.Set<FriendsList>(), "FriendsListId", "FriendsListId");
            //ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId");
            //ViewData["ProfileImageId"] = new SelectList(_context.profileImages, "ProfileImageId", "ProfileImageId");

        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FName,LName,PhoneNumber,")] Person person)
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

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId");
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
