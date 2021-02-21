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
        public int Id { get; set; }
        public List<Event> Events { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }

        public List<AttendEvent> Attendees { get; set; }

        public AttendEvent Attendee { get; set; }
        public Event Event { get; set;  }

        public ApplicationUser ApplicationUser { get; set; }

    }
}
