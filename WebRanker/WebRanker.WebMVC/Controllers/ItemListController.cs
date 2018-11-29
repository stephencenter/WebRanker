using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRanker.Models;
using WebRanker.Services;

namespace WebRanker.WebMVC.Controllers
{
    [Authorize]
    public class ItemListController : Controller
    {
        public ItemListService GetItemListService()
        {
            var userID = Guid.Parse(User.Identity.GetUserId());
            return new ItemListService(userID);
        }

        public ActionResult Index()
        {
            var model = GetItemListService().GetItemList();
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

            var service = GetItemListService();

            if (service.CreateItemList(model))
            {
                TempData["SaveResult"] = "List created!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to create list!");

            return View(model);
        }
    }
}