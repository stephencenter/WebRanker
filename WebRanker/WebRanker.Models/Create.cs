using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRanker.Models
{
    public class Create
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string TheList { get; set; }
    }
}
