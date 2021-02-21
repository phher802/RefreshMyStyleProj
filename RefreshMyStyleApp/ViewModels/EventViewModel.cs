using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using RefreshMyStyleApp.Models;

namespace RefreshMyStyleApp.ViewModels
{
    public class EventViewModel
    {
        [Key]
        public int? Id { get; set; }
        public int AttendeeId { get; set; }
        public string AttendeeName { get; set; }
        public bool IsAttending { get; set; }
        public bool IsNotAttending { get; set; }

        public List<Event> Events { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public List<ApplicationUser> AppUsersNotLoggedIn { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
