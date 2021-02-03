using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RefreshMyStyleApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace RefreshMyStyleApp.ViewModels
{
    public class PersonViewModel
    {
        [Key]
        public int Id { get; set; }

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

        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }


        [ForeignKey("Image")]
        public int? ImageId { get; set; }
        public Models.Image Image { get; set; }


        [ForeignKey("Event")]
        public int? EventId { get; set; }
        public Event Event { get; set; }


        [ForeignKey("FriendsList")]
        public int? FriendsListId { get; set; }
        public FriendsList FriendsList { get; set; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
