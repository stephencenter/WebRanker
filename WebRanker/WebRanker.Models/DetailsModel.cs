using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRanker.Data;

namespace WebRanker.Models
{
    public class DetailsModel : GenericModel
    {
        public int ListID { get; set; }
        public string Title { get; set; }
        public List<Item> TheList { get; set; }

        [Display(Name="Created")]
        public DateTimeOffset CreatedUTC { get; set; }

        [Display(Name = "Modified")]
        public DateTimeOffset? ModifiedUTC { get; set; }
        public override string ToString() => $"[{ListID} {Title}";
    }
}
