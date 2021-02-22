using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Sms
    {
        [Key]
        [Required(ErrorMessage = "To Number Required", AllowEmptyStrings = false)]
        [Phone]
        [Display(Name = "To Number")]
        public string To { get; set; }

        [Required(ErrorMessage = "From Number Required", AllowEmptyStrings = false)]
        [Phone]
        [Display(Name = "From Number")]
        public string From { get; set; }

        [Required(ErrorMessage = "Message Text Required", AllowEmptyStrings = false)]
        [Display(Name = "Message Text")]
        public string Text { get; set; }
    }
}

