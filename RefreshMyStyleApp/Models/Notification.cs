using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<NotificationUser> NotificationUsers { get; set; }
    }
}

