﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RefreshMyStyleApp.Models;

namespace RefreshMyStyleApp.ViewModels
{

    public class ApplicationUserImageViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        public List<ApplicationUser> SearchUsers { get; set; }
        public List<Image> Images { get; set; }
        
        public int? ImageId { get; set; }
        public List<Like> Likes { get; set; }
        public List<Friend> Friends { get; set; }
    }
}
