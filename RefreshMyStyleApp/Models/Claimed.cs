using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Claimed
    {
        [Key]
        public int? Id { get; set; }
        
        public bool IsClaimed { get; set; }

        public DateTime? DateClaimed { get; set; }

        public int ClaimedById { get; set; }

        public int UserId { get; set; }
        public string ClaimImageOwnerFullName { get; set; }

        public string ImageTitle { get; set; }

        [ForeignKey("Image")]
        public int? ImageId { get; set; }
        public Image Image { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
