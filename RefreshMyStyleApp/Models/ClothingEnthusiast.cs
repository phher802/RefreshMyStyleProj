using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class ClothingEnthusiast
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Display(Name = "Last Name")]
        public string LName { get; set; }


        [Display(Name = "Primary Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }


        [ForeignKey("ProfileImage")]
        public int? ProfileImageId { get; set; }
        public ProfileImage ProfileImage { get; set; }


        [ForeignKey("Image")]
        public int? ImageId { get; set; }
        public Image Image { get; set; }


        [ForeignKey("Event")]
        public int? EventId { get; set; }
        public Event Event { get; set; }


        [ForeignKey("FriendsList")]
        public int? FriendsListId { get; set; }
        public FriendsList FriendsList { get; set; }
    }
}
