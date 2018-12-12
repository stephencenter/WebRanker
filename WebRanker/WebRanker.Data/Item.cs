using System;
using System.ComponentModel.DataAnnotations;

namespace WebRanker.Data
{
    public class Item
    {
        [Key]
        public int ItemID { get; set; }

        [Required]
        public int CollectionID { get; set; }

        [Required]
        public Guid OwnerID { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public int RankingPoints { get; set; }
    }
}
