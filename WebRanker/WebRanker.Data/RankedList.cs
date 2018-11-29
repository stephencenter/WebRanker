﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRanker.Data
{
    public class RankedList
    {
        [Key]
        public int ListID { get; set; }

        [Required]
        public Guid OwnerID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public List<string> TheList { get; set; }

        [Required]
        public DateTimeOffset CreatedUTC { get; set; }

        // No ModifiedUTC because RankedLists can't be modified
    }
}
