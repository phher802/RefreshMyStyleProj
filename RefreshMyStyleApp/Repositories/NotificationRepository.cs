using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RefreshMyStyleApp.Hubs;
using RefreshMyStyleApp.Models;
using RefreshMyStyleApp.Repositories;

namespace RefreshMyStyleApp.Data
{
    public class NotificationRepository: INotificationRepository
    {
        public ApplicationDbContext _context;
        private IHubContext<NotificationHub> _hubContext;

        public NotificationRepository(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public void Create(Notification notification, int imageId, int id)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();


            var images = _context.Images.Where(x => x.Id == imageId).ToList();
            foreach (var image in images)
            {
                var userNotification = new NotificationUser();
                userNotification.ApplicationUserId = image.ApplicationUserId;
                userNotification.NotificationId = notification.Id;

                _context.UserNotifications.Add(userNotification);
                _context.SaveChanges();
            }
            _hubContext.Clients.All.SendAsync("displayNotification", "");
        }

        public List<NotificationUser> GetUserNotifications(string userId)
        {
            return _context.UserNotifications.Where(u => u.ApplicationUserId.Equals(userId) && !u.IsRead)
                                            .Include(n => n.Notification)
                                            .ToList();
        }
        public void ReadNotification(int notificationId, string userId)
        {
            var notification = _context.UserNotifications
                                        .FirstOrDefault(n => n.ApplicationUserId.Equals(userId)
                                        && n.NotificationId == notificationId);
            notification.IsRead = true;
            _context.UserNotifications.Update(notification);
            _context.SaveChanges();
        }


    }




}
