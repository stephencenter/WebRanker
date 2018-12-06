using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRanker.Data;

namespace WebRanker.Models
{
    public class ViewPostTransfer
    {
        public List<Matchup> MatchupList { get; set; }
        public Item ChosenItem { get; set; }

        public ViewPostTransfer(List<Matchup> ml, Item ci)
        {
            MatchupList = ml;
            ChosenItem = ci;
        }

        public ViewPostTransfer()
        {

        }
    }
}
