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
            var collection = new Collection()
            {
                OwnerID = _userID,
                Title = model.Title,
                CreatedUTC = DateTimeOffset.Now
            };

            List<Item> items = new List<Item>();

            foreach (string item in model.TheList.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                var new_item = new Item()
                {
                    OwnerID = _userID,
                    ItemName = item,
                    CollectionID = 0,
                    RankingPoints = 0
                };

                items.Add(new_item);
            }

            return SaveDataToDatabase(collection, items);
        }

        public bool SaveDataToDatabase(Collection collection, List<Item> items)
        {
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Collections.Add(collection);
                ctx.SaveChanges();

                // Get the ListID of the collection we just added
                var id = ctx .Collections .Single(
                    c => c.OwnerID == collection.OwnerID && 
                    c.CreatedUTC == collection.CreatedUTC && 
                    c.Title == collection.Title).ListID;

                foreach (Item x in items)
                {
                    x.CollectionID = id;
                    ctx.ListOfItems.Add(x);
                }

                return ctx.SaveChanges() == items.Count;
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
