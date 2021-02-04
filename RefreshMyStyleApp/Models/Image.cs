using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Image
    {
        [Key]
        public int? ImageId { get; set; }

        public string ImageTitle { get; set; }

        public string FilePath { get; set; }

        [Display(Name = "Category")]
        public string ClothingCategory { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string Description { get; set; }

        [Display(Name = "Share")]
        public bool ToShare { get; set; }

        [Display(Name ="Give")]
        public bool ToGiveAway { get; set; }


        [ForeignKey("Person")]
        public int Id { get; set; }
        public Person Person { get; set; }

        [ForeignKey("LikedList")]
        public int? LikedListId { get; set; }
        public LikedList LikedList { get; set; }

        [ForeignKey("ClaimedList")]
        public int? ClaimedListId { get; set; }
        public ClaimedList ClaimedList { get; set; }

    }
}
