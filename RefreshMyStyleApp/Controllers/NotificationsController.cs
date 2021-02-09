using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClientNotifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RefreshMyStyleApp.Data;
using RefreshMyStyleApp.Hubs;
using RefreshMyStyleApp.Models;

namespace RefreshMyStyleApp.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
      


        public NotificationsController(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
           
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            return View(await _context.Notifications.ToListAsync());
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Notifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text")] Notification notification, int id)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();

            //TODO: Assign notification to users
            var likeLists = _context.Likes.Where(l => l.PersonId == id).ToList();
            var claimLists = _context.Claims.Where(l => l.PersonId == id).ToList();

            foreach (var likeList in likeLists)
            {
                var userNotification = new NotificationUser();
                userNotification.PersonId = likeList.PersonId;
                userNotification.NotificationId = notification.Id;

                _context.UserNotifications.Add(userNotification);
                _context.SaveChanges();
            }

            foreach (var claimList in claimLists)
            {
                var userNotification = new NotificationUser();
                userNotification.PersonId = claimList.PersonId;
                userNotification.NotificationId = notification.Id;

                _context.UserNotifications.Add(userNotification);
                _context.SaveChanges();

            }
              await _hubContext.Clients.All.SendAsync("displayNotification","");
          

            //if (ModelState.IsValid)
            //{
            //    _context.Add(notification);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            return View(notification);
        }

        public List<NotificationUser> GetUserNotifications(string id)
        {
            var getUserNoti = _context.UserNotifications.Where(u => u.PersonId.Equals(id) && !u.IsRead)
                .Include(n => n.Notification).ToList();

            return getUserNoti; 
        }

        public IActionResult GetNotification()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var person = _context.People.Where(c => c.IdentityUserId == userId).FirstOrDefault().ToString();
           
            var notification = GetUserNotifications(person);
            return Ok(new { UserNotification = notification, Count = notification.Count });
        }

        public IActionResult ReadNotification(int notificationId, string id)
        {
            var notification = _context.UserNotifications
                                        .FirstOrDefault(n => n.PersonId.Equals(id)
                                        && n.NotificationId == notificationId);

            notification.IsRead = true;
            _context.UserNotifications.Update(notification);
            _context.SaveChanges();


            return Ok();
        }
        // GET: Notifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text")] Notification notification)
        {
            if (id != notification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.Id))
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
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}
