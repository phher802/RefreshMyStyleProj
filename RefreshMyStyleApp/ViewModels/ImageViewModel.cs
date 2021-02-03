using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RefreshMyStyleApp.ViewModels
{
    public class ImageViewModel
    {
        [Key]
        public int? ImageId { get; set; }

        public string ImageTitle { get; set; }

        public byte[] ImageData { get; set; }

        public IFormFile Img { get; set; }

        [Display(Name = "Category")]
        public string ClothingCategory { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string Description { get; set; }

        [Display(Name = "Share")]
        public bool ToShare { get; set; }

        [Display(Name = "Give")]
        public bool ToGiveAway { get; set; }
    }
}

