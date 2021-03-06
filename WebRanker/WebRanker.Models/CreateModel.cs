﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRanker.Models
{
    public class CreateModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name="Your list")]
        public string TheList { get; set; }

        public override string ToString() => Title;
    }
}
