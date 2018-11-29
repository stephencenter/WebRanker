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

        public bool CreateItemList(CreateList model)
        {
            var entity = new ItemList()
            {
                OwnerID = _userID,
                Title = model.Title,
                CreatedUTC = DateTimeOffset.Now
            };

            foreach (string item in model.TheList.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                var new_item = new Item()
                {
                    OwnerID = _userID,
                    ItemName = item,
                    CollectionID = model.ListID,
                    RankingPoints = 0
                };

                using (var ctx = new ApplicationDbContext())
                {
                    ctx.ListOfItems.Add(new_item);
                }
            }

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Collections.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ViewList> GetNotes()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Collections
                        .Where(e => e.OwnerID == _userID)
                        .Select(
                            e =>
                                new ViewList
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
