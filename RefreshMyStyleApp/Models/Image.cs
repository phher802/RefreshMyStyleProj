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

        public string ImageTitle { get; set; }

        public string FilePath { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Category")]
        public string ClothingCategory { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string Description { get; set; }

        [Display(Name = "Share")]
        public bool ToShare { get; set; }

        [Display(Name ="Give")]
        public bool ToGiveAway { get; set; }

        public bool IsLiked { get; set; }

        public bool IsClaimed { get; set; }

        public ICollection<Like> Likes { get; set; }

        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

  


    }
}
