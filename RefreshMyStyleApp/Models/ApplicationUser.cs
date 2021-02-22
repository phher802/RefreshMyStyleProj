using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace RefreshMyStyleApp.Models
{
    public class ApplicationUser
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone Number Required", AllowEmptyStrings = false)]
        [Display(Name = "Primary Phone Number")]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
   
        public string ProfileImageName { get; set; }

        public int ImageOwnerId { get; set; }

        [NotMapped]
        public IFormFile ProfileImage { get; set; }

        public List<Image> Images { get; set; }

        [Display(Name = "Search friends")]
        public string SearchUsers { get; set; }
        
        public List<ApplicationUser> SearchResults { get; set; }
      
        public List<Like> Likes { get; set; }
  
        public bool EventAttendStatus { get; set; }

        public bool IsAttending { get; set; }


        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
   
        
    }

}
