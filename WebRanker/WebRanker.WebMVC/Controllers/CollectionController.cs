using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;
using WebRanker.Models;
using WebRanker.Services;

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
            var model = GetCollectionService().GetViewModelList();
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
            service.CreateCollection(model);
            TempData["SaveResult"] = "Your list has been created!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var service = GetCollectionService();
            var model = service.GetDetailsModel(id);

            return View(model);
        }

        [HttpGet]
        [ActionName("Rank")]
        public ActionResult Rank(int id)
        {
            var service = GetCollectionService();
            service.DeleteAllMatchups(id);
            service.GetMatchups(id);
            service.ResetRankingPoints(id);
            var model = service.GetCurrentMatchup(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Rank")]
        public ActionResult RankPost(int id)
        {
            var service = GetCollectionService();
            service.DeleteMatchup(service.GetCurrentMatchup(id).MatchupID);
            service.IncreaseItemRankingPoints(int.Parse(Request["[0].choice"]));

            if (service.AreThereMoreMatchups(id))
            {
                var model = service.GetCurrentMatchup(id);
                return View(model);
            }

            else
            {
                TempData["SaveResult"] = "Your list has been ranked!";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Help()
        {
            return View();
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var service = GetCollectionService();
            var model = service.GetDetailsModel(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = GetCollectionService();
            service.DeleteList(id);
            TempData["SaveResult"] = "Your list was deleted!";

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var service = GetCollectionService();
            var collection = service.GetDetailsModel(id);

            var model = new EditModel
            {
                CollectionID = collection.CollectionID,
                Title = collection.Title,
                TheList = service.ConvertListToString(service.GetItemsByCollectionID(id))
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.CollectionID != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = GetCollectionService();
            service.UpdateCollection(model);
            TempData["SaveResult"] = "Your list was updated!";

            return RedirectToAction("Index");
        }
    }
}