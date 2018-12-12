using System;
using System.ComponentModel.DataAnnotations;

namespace WebRanker.Models
{
    public class Matchup
    {
        [Key]
        public int MatchupID { get; set; }

        [Required]
        public int CollectionID { get; set; }

        [Required]
        public Guid OwnerID { get; set; }

        [Required]
        public int FirstItemID { get; set; }

        [Required]
        public int SecondItemID { get; set; }

        public int choice { get; set; }
    }
}
