using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRanker.Models;

namespace WebRanker.WebMVC.Controllers
{
    [Authorize]
    public class ItemListController : Controller
    {
        public ActionResult Index()
        {
            var model = new ViewLists[0];
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}