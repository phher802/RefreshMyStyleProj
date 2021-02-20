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
        public int? Id { get; set; }

        [Display(Name = "Date Event Was Posted")]
        public DateTime? DatePosted { get; set; }

        [Display(Name = "Date of Event")]
        public DateTime? EventDate { get; set; }

        [Display(Name = "Event Name")]
        public string EventTitle { get; set; }

        public string Message { get; set; }

        public string CancelEvent { get; set; }

        public bool IsCanceled { get; set; }

        public int EventCreatorId { get; set; }
        public string EventCreatorName { get; set; }
        public int AttendeeId { get; set; }
        public string AttendeeName { get; set; }
        public bool IsAttending { get; set; }
        public bool IsNotAttending { get; set; }


    }
}
