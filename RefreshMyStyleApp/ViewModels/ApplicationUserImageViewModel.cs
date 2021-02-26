using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using RefreshMyStyleApp.Models;

namespace RefreshMyStyleApp.ViewModels
{

    public class ApplicationUserImageViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public ApplicationUser AppUserNotLoggedIn { get; set; }
        //public string SelectAppUsersID { get; set; }
        //public List<SelectListItem> AppUsers { get; set; }
        public List<ApplicationUser> AppUsersNotLoggedIn { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public Image Image { get; set; }
        public List<ApplicationUser> SearchUsers { get; set; }

        public List<Image> Images { get; set; }

        public int? ImageId { get; set; }
        public List<LikedItem> Likes { get; set; }
        public LikedItem Like { get; set; }

        public List<ClaimedItem> Claims { get; set; }
        public ClaimedItem Claimed { get; set; }
        public List<Friend> Friends { get; set; }

        public Post Post { get; set; }
        public List<Post> Posts { get; set; }
        public Comment Comment { get; set; }
        public List<Comment> Comments { get; set; }
        public Event Event { get; set; }

        public List<Event> Events {get; set; }

        public List<AttendEvent> Attendees { get; set; }
       public AttendEvent Attendee { get; set; }

        public Message Message { get; set; }
        public List<Message> Messages { get; set; }
    }
}
