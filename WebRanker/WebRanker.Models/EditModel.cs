using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRanker.Models
{
    public class EditModel
    {
        public int ListID { get; set; }
        public string Title { get; set; }
        public string TheList { get; set; }
    }
}
