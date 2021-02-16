using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class NotificationUser
    {
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }
        public bool IsRead { get; set; } = false;


        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


    }
}
