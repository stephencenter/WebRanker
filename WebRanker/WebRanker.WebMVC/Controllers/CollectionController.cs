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

        public ActionResult Index()
        {
            var model = GetCollectionService().GetCollection();
            return View(model);
        }

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

        public ActionResult Details(int id)
        {
            var service = GetCollectionService();
            var model = service.GetCollectionByID(id, true);

            return View(model);
        }

        public ActionResult Rank(int id)
        {
            var service = GetCollectionService();
            var model = service.GetCollectionByID(id, false);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rank(Matchup matchup)
        {
            return View(matchup);
        }
    }
}