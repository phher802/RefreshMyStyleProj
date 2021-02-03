using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{

    public class ProfileImage
    {
        [Key]
        public int? ProfileImageId { get; set; }

        [Display(Name = "Profile Picture")]
        public string ProfileImageTitle { get; set; }

        public byte[] ProfileImageData { get; set; }
   
    }
}
