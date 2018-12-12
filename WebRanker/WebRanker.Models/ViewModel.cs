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
        public int CollectionID { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public DateTimeOffset CreatedUTC { get; set; }
    }
}
