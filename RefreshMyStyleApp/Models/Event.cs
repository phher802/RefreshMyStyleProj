using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Event
    {
        [Key]
        public int? EventId { get; set; }

        [Display(Name = "Date Event Was Posted")]
        public DateTime? DatePosted { get; set; }

        [Display(Name = "Date of Event")]
        public DateTime? EventDate { get; set; }

        [Display(Name = "Event Name")]
        public string EventTitle { get; set; }

        public string Message { get; set; }


        [ForeignKey("EventList")]
        public int? EventListId { get; set; }
        public EventList EvenList { get; set; }

        [ForeignKey("Person")]
        public int? Id { get; set; }
        public Person Person { get; set; }

    }
}
