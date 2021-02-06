using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Noti
    {

        public int Id { get; set; } = 0;
        public int FromUserId { get; set; } = 0;
        public int ToUserId { get; set; } = 0;
        public string NotiHeader { get; set; } = "";
        public string NotiBody { get; set; } = "";
        public bool IsRead { get; set; } = false;
        public string Url { get; set; } = "";
        public DateTime? CreatedDate { get; set; }
        public string Message { get; set; } = "";
        public string IsReadSt => this.IsRead ? "YES" : "NO";

        public string FromUserName { get; set; } = "";
        public string ToUserName { get; set; } = "";

        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

    }
}
