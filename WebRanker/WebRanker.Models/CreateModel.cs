using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRanker.Models
{
    public class CreateList
    {
        [Required]
        public int ListID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string TheList { get; set; }

        public override string ToString() => Title;
    }
}
