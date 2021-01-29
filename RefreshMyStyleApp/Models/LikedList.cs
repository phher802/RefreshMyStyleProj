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

        [ForeignKey("ClothingEnthusiast")]
        public int UserId { get; set; }
        public ClothingEnthusiast User { get; set; }


        [ForeignKey("Image")]
        public int? ImageId { get; set; }
        public Image Image { get; set; }


        [ForeignKey("Video")]
        public int? VideoId { get; set; }
        public Video Video { get; set; }
    }
}
