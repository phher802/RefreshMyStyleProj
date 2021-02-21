using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class AttendEvent
    {
        [Key]
        public int Id { get; set; }

        public string AttendeeName { get; set; }
        public int AttendeeId { get; set; }
        public int? EventId { get; set; }

        

    }
}
