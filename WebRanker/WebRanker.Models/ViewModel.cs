using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRanker.Models
{
    public class ViewModel
    {
        public int ListID { get; set; }
        public string Title { get; set; }
        
        [Display(Name="Created on")]
        public DateTimeOffset CreatedUTC { get; set; }

        public override string ToString() => Title;
    }
}
