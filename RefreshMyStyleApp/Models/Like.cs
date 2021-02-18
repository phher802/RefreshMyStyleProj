using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Like
    {
        [Key]
        public int? Id { get; set; }
        public bool IsLiked { get; set; }
        public int UserId { get; set; }
        public DateTime? DateLiked { get; set; }

        public string ImageTitle { get; set; }

        public string LikedImageOwnerFullName { get; set; }

        [ForeignKey("Image")]
        public int? ImageId { get; set; }
        public Image Image { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
      

    }
}
