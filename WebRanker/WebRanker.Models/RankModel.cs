using System.Collections.Generic;
using WebRanker.Data;

namespace WebRanker.Models
{
    public class RankModel : GenericModel
    {
        public int ListID { get; set; }
        public string Title { get; set; }
        public List<IList<Item>> MatchupList { get; set; }
    }
}
