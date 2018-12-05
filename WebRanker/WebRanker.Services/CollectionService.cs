using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRanker.Data;
using WebRanker.Models;

namespace WebRanker.Services
{
    public class CollectionService
    {
        private readonly Guid _userID;

        public CollectionService(Guid userID)
        {
            _userID = userID;
        }

        public bool CreateCollection(CreateModel model)
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

        public IEnumerable<ViewModel> GetCollection()
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

        public DetailsModel GetCollectionByID(int ListID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var found_collection = ctx.Collections.Single(e => e.ListID == ListID && e.OwnerID == _userID);
                List<Item> found_list = ctx.ListOfItems.Where(e => e.CollectionID == ListID && e.OwnerID == _userID).ToList();
                found_list = found_list.OrderByDescending(e => e.RankingPoints).ToList();

                return new DetailsModel
                {
                    ListID = found_collection.ListID,
                    Title = found_collection.Title,
                    CreatedUTC = found_collection.CreatedUTC,
                    ModifiedUTC = found_collection.ModifiedUTC,
                    TheList = found_list
                };
            }
        }

        public List<Matchup> GetMatchups(int ListID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var found_list = ctx.ListOfItems.Where(e => e.CollectionID == ListID && e.OwnerID == _userID).ToList();
                var combo_list = new Combinations<Item>(found_list.OrderByDescending(e => e.RankingPoints).ToList(), 2);
                var matchup_list = new List<Matchup>();

                foreach (IList<Item> i in combo_list) {
                    matchup_list.Add(new Matchup{FirstItem = i[0], SecondItem = i[1]});
                }

                return matchup_list;
            }
        }

        public void ResetRankingPoints(int ListID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var found_items = ctx.ListOfItems.Where(e => e.CollectionID == ListID && e.OwnerID == _userID);
                
                foreach (Item i in found_items)
                {
                    i.RankingPoints = 0;
                }

                ctx.SaveChanges();
            }
        }

        public void IncreaseItemRankingPoints(int ItemID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var found_item = ctx.ListOfItems.Single(i => i.ItemID == ItemID && i.OwnerID == _userID);
                found_item.RankingPoints++;
                ctx.SaveChanges();
            }
        }
    }
}
