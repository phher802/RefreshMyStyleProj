using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class EventList
    {
        [Key]
        public int? EventListId { get; set; }


        [ForeignKey("ClothingEnthusiast")]
        public int UserId { get; set; }
        public ClothingEnthusiast User { get; set; }

    }
}
