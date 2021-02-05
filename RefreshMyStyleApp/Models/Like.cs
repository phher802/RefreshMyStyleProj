﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Like
    {
        [Key]
        public int? LikeId { get; set; }


        [ForeignKey("Image")]
        public int? ImageId { get; set; }
        public Image Image { get; set; }

       
    }
}