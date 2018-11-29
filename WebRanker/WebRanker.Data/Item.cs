using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRanker.Data
{
    public class Item
    {
        public int ItemID { get; set; }
        public int CollectionID { get; set; }
        public Guid OwnerID { get; set; }
        public string ItemName { get; set; }
        public int RankingPoints { get; set; }
    }
}
