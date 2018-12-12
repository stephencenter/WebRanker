namespace WebRanker.Models
{
    public class MatchupModel
    {
        public int MatchupID { get; set; }
        public int CollectionID { get; set; }

        public int FirstItemID { get; set; }
        public int SecondItemID { get; set; }

        public string FirstItemName { get; set; }
        public string SecondItemName { get; set; }

        public int choice { get; set; }
    }
}
