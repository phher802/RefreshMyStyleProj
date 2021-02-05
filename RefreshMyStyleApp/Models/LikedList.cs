using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class LikedList
    {
        [Key]
        public int? LikedListId { get; set; }

        public bool IsLiked { get; set; }
       
    }
}
