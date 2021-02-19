using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Post
    {
        [Key]
        public int Id { set; get; }

        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public string PostByUser { get; set; }
        public DateTime? DateTimePosted { get; set; }

  

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }



    }
}
