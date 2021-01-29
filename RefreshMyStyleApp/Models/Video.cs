using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RefreshMyStyleApp.Models
{
    public class Video
    {
        [Key]
        public int? VideoId { get; set; }

        public string VideoTitle { get; set; }

        public IFormFile VideoData { get; set; }

        public string ClothingCategory { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string Description { get; set; }

        public bool ToShare { get; set; }

        public bool ToGiveAway { get; set; }
    }
}
