using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRanker.Models
{
    public class UnrankedViewItem
    {
        public int ListID { get; set; }
        public string Title { get; set; }
        public DateTimeOffset CreatedUTC { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
