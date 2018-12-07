﻿using Combinatorics.Collections;
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
                var id = ctx.Collections.Single(c => c.OwnerID == _userID && c.CreatedUTC == CreatedUTC).ListID;
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
                        ListID = e.ListID,
                        Title = e.Title,
                        CreatedUTC = e.CreatedUTC,
                        Count = ctx.ListOfItems.Where(i => i.CollectionID == e.ListID && i.OwnerID == _userID).Count()
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
                var items = GetItemsByListID(ListID);
                items = items.OrderByDescending(e => e.RankingPoints).ToList();

                return new DetailsModel
                {
                    ListID = found_collection.ListID,
                    Title = found_collection.Title,
                    CreatedUTC = found_collection.CreatedUTC,
                    ModifiedUTC = found_collection.ModifiedUTC,
                    TheList = items
                };
            }
        }

        public List<Item> GetItemsByListID(int ListID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                return ctx.ListOfItems.Where(e => e.CollectionID == ListID && e.OwnerID == _userID).ToList();
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

        public List<Matchup> GetMatchups(int ListID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var items = GetItemsByListID(ListID);
                var combo_list = new Combinations<Item>(items.OrderByDescending(e => e.RankingPoints).ToList(), 2);
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
                var items = GetItemsByListID(ListID);

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

        public bool DeleteList(int ListID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var collection = ctx.Collections.Single(e => e.ListID == ListID && e.OwnerID == _userID);
                var items = GetItemsByListID(ListID);

                ctx.Collections.Remove(collection);

                foreach (Item item in items)
                {
                    ctx.ListOfItems.Remove(item);
                }

                return ctx.SaveChanges() == items.Count() + 1;
            }
        }

        public void UpdateNote(EditModel model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var collection = ctx.Collections.Single(e => e.ListID == model.ListID && e.OwnerID == _userID);

                collection.Title = model.Title;
                collection.ModifiedUTC = DateTimeOffset.UtcNow;
                SaveItemsToDatabase(ConvertStringToList(model.TheList), collection.Title, collection.CreatedUTC);
                ctx.SaveChanges();

                return;
            }
        }
    }
}
