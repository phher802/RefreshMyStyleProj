using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace RefreshMyStyleApp.Models
{
    public class Friend
    {

        [Key]
        public int Id { get; set; }

        public int? RequestedById { get; set; }    
        public int? RequestedToId { get; set; }

        [ForeignKey("RequestedById")]
        [Column(Order = 0)]
        public virtual ApplicationUser RequestedBy { get; set; }

        [ForeignKey("RequestedToId")]
        [Column(Order = 1)]
        public virtual ApplicationUser RequestedTo { get; set; }



        public DateTime? RequestTime { get; set; }

        public DateTime? BecameFriendsTime { get; set; }

        [ForeignKey("FriendRequestFlag")]
        public FriendRequestFlag FriendRequestFlag { get; set; }

        [NotMapped]
        public bool Approved => FriendRequestFlag == FriendRequestFlag.Approved;

       
    }

    public enum FriendRequestFlag
    {
        None,
        Approved,
        Rejected,
        Blocked,
        Spam
    }
}




