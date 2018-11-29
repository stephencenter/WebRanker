using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRanker.Data;
using WebRanker.Models;

namespace WebRanker.Services
{
    public class ItemListService
    {
        private readonly Guid _userID;

        public ItemListService(Guid userID)
        {
            _userID = userID;
        }

        public bool CreateItemList(CreateModel model)
        {
            var entity = new Collection()
            {
                OwnerID = _userID,
                Title = model.Title,
                CreatedUTC = DateTimeOffset.Now
            };

            List<Item> item_list = new List<Item>();

            foreach (string item in model.TheList.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                var new_item = new Item()
                {
                    OwnerID = _userID,
                    ItemName = item,
                    CollectionID = model.ListID,
                    RankingPoints = 0
                };

                item_list.Add(new_item);
            }

            return SaveDataToDatabase(entity, item_list);
        }

        public bool SaveDataToDatabase(Collection list, List<Item> item_list)
        {
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Collections.Add(list);

                foreach (Item x in item_list)
                {
                    ctx.ListOfItems.Add(x);
                }

                return ctx.SaveChanges() == 1 + item_list.Count;
            }
        } 

        public IEnumerable<ViewModel> GetItemList()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Collections
                        .Where(e => e.OwnerID == _userID)
                        .Select(
                            e =>
                                new ViewModel
                                {
                                    ListID = e.ListID,
                                    Title = e.Title,
                                    CreatedUTC = e.CreatedUTC
                                }
                        );

                return query.ToArray();
            }
        }
    }
}
