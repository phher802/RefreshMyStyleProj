﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace RefreshMyStyleApp.Models
{
    public class Friendship
    {
        [Key]
        public int? FriendshipId { get; set; }

        [ForeignKey("Person")]
        public int MeId { get; set; }
        public Person Me { get; set; }

        [ForeignKey("Person")]
        public int? FriendId { get; set; }
        public Person Friend { get; set; }
 
    }
}