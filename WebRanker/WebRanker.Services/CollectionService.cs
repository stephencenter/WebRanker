using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public void CreateCollection(CreateModel model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var collection = new Collection()
                {
                    OwnerID = _userID,
                    Title = model.Title,
                    CreatedUTC = DateTimeOffset.Now
                };

                ctx.Collections.Add(collection);
                ctx.SaveChanges();
                SaveItemsToDatabase(ConvertStringToList(model.TheList), collection.Title, collection.CreatedUTC);
            }

            return;
        }

        public void SaveItemsToDatabase(List<Item> new_items, string Title, DateTimeOffset CreatedUTC)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var id = ctx.Collections.Single(c => c.OwnerID == _userID && c.CreatedUTC == CreatedUTC).CollectionID;
                var existing_items = ctx.ListOfItems.Where(i => i.OwnerID == _userID && i.CollectionID == id); 

                foreach (Item e in existing_items)
                {
                    ctx.ListOfItems.Remove(e);
                }

                foreach (Item i in new_items)
                {
                    i.CollectionID = id;
                    ctx.ListOfItems.Add(i);
                }

                ctx.SaveChanges();
                return;
            }
        }

        public IEnumerable<ViewModel> GetCollection()
        {
            using (var ctx = new ApplicationDbContext())
            {

                var query = ctx.Collections.Where(e => e.OwnerID == _userID).Select(
                    e => new ViewModel
                    {
                        CollectionID = e.CollectionID,
                        Title = e.Title,
                        CreatedUTC = e.CreatedUTC,
                        Count = ctx.ListOfItems.Where(i => i.CollectionID == e.CollectionID && i.OwnerID == _userID).Count()
                    }
                );

                return query.ToArray();
            }
        }

        public DetailsModel GetCollectionByID(int CollectionID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var found_collection = ctx.Collections.Single(e => e.CollectionID == CollectionID && e.OwnerID == _userID);
                var items = GetItemsByCollectionID(CollectionID);
                items = items.OrderByDescending(e => e.RankingPoints).ToList();

                return new DetailsModel
                {
                    CollectionID = found_collection.CollectionID,
                    Title = found_collection.Title,
                    CreatedUTC = found_collection.CreatedUTC,
                    ModifiedUTC = found_collection.ModifiedUTC,
                    TheList = items
                };
            }
        }

        public List<Item> GetItemsByCollectionID(int CollectionID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                return ctx.ListOfItems.Where(e => e.CollectionID == CollectionID && e.OwnerID == _userID).ToList();
            }
        }

        public string ConvertListToString(List<Item> the_list)
        {
            List<string> string_list = new List<string>();

            foreach (Item item in the_list)
            {
                string_list.Add(item.ItemName);
            }

            return String.Join("\n", string_list);
        }

        public List<Item> ConvertStringToList(string the_string)
        {
            List<Item> items = new List<Item>();

            foreach (string item in the_string.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {

                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }

                var new_item = new Item()
                {
                    OwnerID = _userID,
                    ItemName = item,
                    CollectionID = 0,
                    RankingPoints = 0
                };

                items.Add(new_item);
            }

            return items;
        }

        public string GetNameOfItem(int ItemID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                return ctx.ListOfItems.Single(x => x.ItemID == ItemID && x.OwnerID == _userID).ItemName;
            }
        }

        public void GetMatchups(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var items = GetItemsByCollectionID(id);
                var combo_list = new Combinations<Item>(items.OrderByDescending(e => e.RankingPoints).ToList(), 2);

                foreach (IList<Item> i in combo_list) {
                    var new_matchup = new Matchup
                    {
                        FirstItemID = i[0].ItemID,
                        SecondItemID = i[1].ItemID,
                        CollectionID = id,
                        OwnerID = _userID
                    };

                    ctx.ListOfMatchups.Add(new_matchup);
                }

                ctx.SaveChanges();
            }
        }

        public void ResetRankingPoints(int CollectionID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var items = ctx.ListOfItems.Where(e => e.CollectionID == CollectionID && e.OwnerID == _userID);

                foreach (Item i in items)
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

        public bool DeleteList(int CollectionID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var collection = ctx.Collections.Single(e => e.CollectionID == CollectionID && e.OwnerID == _userID);
                var items = GetItemsByCollectionID(CollectionID);

                ctx.Collections.Remove(collection);

                foreach (Item item in items)
                {
                    ctx.Entry(item).State = EntityState.Deleted;
                    ctx.ListOfItems.Remove(item);
                }

                return ctx.SaveChanges() == items.Count() + 1;
            }
        }

        public void UpdateCollection(EditModel model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var collection = ctx.Collections.Single(e => e.CollectionID == model.CollectionID && e.OwnerID == _userID);

                collection.Title = model.Title;
                collection.ModifiedUTC = DateTimeOffset.UtcNow;
                SaveItemsToDatabase(ConvertStringToList(model.TheList), collection.Title, collection.CreatedUTC);
                ctx.SaveChanges();

                return;
            }
        }

        public RankModel GetCurrentMatchup(int CollectionID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var found_matchup = ctx.ListOfMatchups.First(x => x.CollectionID == CollectionID && x.OwnerID == _userID);

                return new RankModel
                {
                    CollectionID = found_matchup.CollectionID,
                    MatchupID = found_matchup.MatchupID,
                    FirstItemID = found_matchup.FirstItemID,
                    SecondItemID = found_matchup.SecondItemID,
                    FirstItemName = GetNameOfItem(found_matchup.FirstItemID),
                    SecondItemName = GetNameOfItem(found_matchup.SecondItemID)
                };
            }
        }

        public void DeleteMatchup(int MatchupID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var to_be_deleted = ctx.ListOfMatchups.Single(x => x.MatchupID == MatchupID && x.OwnerID == _userID);
                ctx.ListOfMatchups.Remove(to_be_deleted);
                ctx.SaveChanges();
            }
        }

        public void DeleteAllMatchups(int CollectionID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var to_be_deleted = ctx.ListOfMatchups.Where(x => x.CollectionID == CollectionID && x.OwnerID == _userID);

                foreach(Matchup matchup in to_be_deleted)
                {
                    ctx.ListOfMatchups.Remove(matchup);
                }

                ctx.SaveChanges();
            }
        }

        public bool AreThereMoreMatchups(int CollectionID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var matchup_list = ctx.ListOfMatchups.Where(x => x.CollectionID == CollectionID && x.OwnerID == _userID);

                return !(matchup_list.Count() == 0);
            }
        }
    }
}
