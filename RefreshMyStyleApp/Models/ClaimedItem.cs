using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class ClaimedItem
    {
        [Key]
        public int? Id { get; set; }      

        public DateTime? DateClaimed { get; set; }
    
        public int ClaimedImageOwnerId { get; set; }
        public string ClaimImageOwnerFullName { get; set; }
        public string ImageTitle { get; set; }

        public int ClaimedById { get; set; }
        public string ClaimedByName { get; set; }
        
        public bool IsDeleted { get; set; }

        [ForeignKey("Image")]
        public int? ImageId { get; set; }
        public Image Image { get; set; }
   
        public int ApplicationUserId { get; set; }
      
    }
}
