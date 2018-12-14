using System;
using System.ComponentModel.DataAnnotations;

namespace WebRanker.Data
{
    public class Collection
    {
        [Key]
        public int CollectionID { get; set; }

        [Required]
        public Guid OwnerID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTimeOffset CreatedUTC { get; set; }

        public DateTimeOffset ModifiedUTC { get; set; }
    }
}
