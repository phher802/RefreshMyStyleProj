using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Message
    {
        [Key]
        public int? Id { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public int SenderID { get; set; }
        public string SenderName { get; set; }
        public string ImageFilePath { get; set; }
        public string MessageContent { get; set; }
        public bool ConfirmMsgIsSent { get; set; }
        public DateTime DateMessageSent { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }     
        public ApplicationUser ApplicationUser { get; set; }
        public int? ImageId { get; set; }
       

    }
}
