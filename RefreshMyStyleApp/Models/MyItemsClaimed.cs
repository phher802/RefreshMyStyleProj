using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class MyItemClaimed
    {
        [Key]
        public int? Id { get; set; }

        public string ClothingEnthusiastId { get; set; }

        public string ClothingEnthusiastName { get; set; }


        [ForeignKey("Image")]
        public int? ImageId { get; set; }
        public Image Image { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
