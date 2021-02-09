using System;
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
        public Image Image { get; set; }

        public List<Image> Images {get; set;}
    }
}
