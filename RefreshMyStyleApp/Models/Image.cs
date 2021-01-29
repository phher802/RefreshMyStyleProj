using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Image
    {
        [Key]
        public int? ImageId { get; set; }

        public string ImageTitle { get; set; }

        public byte[] ImageData { get; set; }

        [Display(Name = "Category")]
        public string ClothingCategory { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string Description { get; set; }

        public bool ToShare { get; set; }

        public bool ToGiveAway { get; set; }
    }
}
