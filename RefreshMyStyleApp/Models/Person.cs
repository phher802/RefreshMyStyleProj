﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace RefreshMyStyleApp.Models
{
    public class Person
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
   
        public string ImageName { get; set; }
      
        [NotMapped]
        public IFormFile ProfileImage { get; set; }


        [ForeignKey("ClaimedList")]
        public int? ClaimedListId { get; set; }
        public ClaimedList ClaimedList { get; set; }


        [ForeignKey("LikedList")]
        public int? LikedListId { get; set; }
        public LikedList LikedList { get; set; }


        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
      

    }
}
