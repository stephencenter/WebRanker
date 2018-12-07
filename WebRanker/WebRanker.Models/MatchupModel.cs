using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRanker.Data;

namespace WebRanker.Models
{
    public class MatchupModel
    {
        public int ListID { get; set; }
        public Item FirstItem { get; set; }
        public Item SecondItem { get; set; }
        public Item SelectedItem { get; set; }
        public List<MatchupModel> MatchupList { get; set; }
    }
}
