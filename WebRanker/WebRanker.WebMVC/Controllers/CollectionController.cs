using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRanker.Models;
using WebRanker.Services;
using WebRanker.Data;

namespace WebRanker.WebMVC.Controllers
{
    [Authorize]
    public class CollectionController : Controller
    {
        public CollectionService GetCollectionService()
        {
            var userID = Guid.Parse(User.Identity.GetUserId());
            return new CollectionService(userID);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = GetCollectionService().GetCollection();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var service = GetCollectionService();

            if (service.CreateCollection(model))
            {
                TempData["SaveResult"] = "List created!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to create list!");

            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var service = GetCollectionService();
            var model = service.GetCollectionByID(id);

            return View(model);
        }

        [HttpGet]
        public ActionResult Rank(int id)
        {
            var service = GetCollectionService();

            if (!TempData.Keys.Contains("matchuplist"))
            {
                TempData["matchuplist"] = service.GetMatchups(id);
            }

            return View(TempData["matchuplist"]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rank(ViewPostTransfer vpt)
        {
            var service = GetCollectionService();
            List<Matchup> matchup_list = vpt.MatchupList;
            Item item = vpt.ChosenItem;

            TempData["matchuplist"] = matchup_list.Skip(1);
            service.IncreaseItemRankingPoints(item.ItemID);

            return View(item.CollectionID);
        }

        [HttpGet]
        public PartialViewResult Help()
        {
            return PartialView();
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var service = GetCollectionService();
            var model = service.GetCollectionByID(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = GetCollectionService();
            service.DeleteList(id);
            TempData["SaveResult"] = "Your note was deleted";

            return RedirectToAction("Index");
        }
    }
}