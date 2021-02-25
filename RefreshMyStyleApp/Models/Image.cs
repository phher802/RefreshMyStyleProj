using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RefreshMyStyleApp.Models
{
    public class Image
    {
        [Key]
        public int? Id { get; set; }
        public string ImageName { get; set; }
        public string ImageFilePath { get; set; }

        [Display(Name = "Search your style")]
        public string SearchImages { get; set; }


        [NotMapped]
        public IFormFile ImageFile { get; set; }


        [Display(Name = "Category")]
        public string ClothingCategory { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public string ItemStatus { get; set; }
        public bool IsLiked { get; set; }    
        public bool IsConfirmed { get; set; }
        public bool IsClaimed { get; set; }
        public int ClaimedById { get; set; }
        public string ClaimedByName { get; set; }

        public List<ClaimedItem> Claimed { get; set; }
        public List<LikedItem> Likes { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

  


    }
}
