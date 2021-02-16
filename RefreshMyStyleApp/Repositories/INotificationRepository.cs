using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RefreshMyStyleApp.Models;

namespace RefreshMyStyleApp.Repositories
{
    interface INotificationRepository
    {
        List<NotificationUser> GetUserNotifications(string userId);
        void Create(Notification notification, int imageId);
        void ReadNotification(int notificationId, string userId);
    }
}

