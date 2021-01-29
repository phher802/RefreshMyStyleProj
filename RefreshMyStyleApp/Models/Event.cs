using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Event
    {
        [Key]
        public int? EventId { get; set; }

        public DateTime? DatePosted { get; set; }

        public DateTime? EventDate { get; set; }

        public string EventTitle { get; set; }

        public string Message { get; set; }


    }
}
