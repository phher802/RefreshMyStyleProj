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

        //public ApplicationUser()
        //{
        //    SentFriendRequests = new List<Friend>();
        //    ReceievedFriendRequests = new List<Friend>();
        //}

        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Primary Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
   
        public string ProfileImageName { get; set; }

        public bool IsGoing { get; set; }

        [Display(Name = "Search friends")]
        public string SearchUsers { get; set; }
        
        public List<ApplicationUser> SearchResults { get; set; }
      
        [NotMapped]
        public IFormFile ProfileImage { get; set; }

        public List<Like> Likes { get; set; }
        public List<Image> Images { get; set; }

        public List<NotificationUser> NotificationUsers { get; set; }

        [NotMapped]
        public virtual List<Friend> Friends { get; set; }
   

        [InverseProperty("RequestedBy")]
        public virtual List<Friend> RequestedBy { get; set; }

        [InverseProperty("RequestedTo")]
        public virtual List<Friend> RequestedTo { get; set; }

        public List<Friend> SentFriendRequests { get; }
        public List<Friend> ReceievedFriendRequests { get; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
   
        
    }
    public class FindUser
    {
        public int Id { get; set; }

        public string FName { get; set; }

        public string LName { get; set; }

        public bool IsFound { get; set; }


    }
}
