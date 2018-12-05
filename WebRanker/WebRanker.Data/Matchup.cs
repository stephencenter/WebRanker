using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRanker.Data
{
    public class Matchup
    {
        public int ListID { get; set; }
        public Item FirstItem { get; set; }
        public Item SecondItem { get; set; }
    }
}
