﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshMyStyleApp.Models
{
    public class Image
    {
        [Key]
        public int? ImageId { get; set; }

        public string ImageTitle { get; set; }

        public byte[] ImageData { get; set; }

    }
}