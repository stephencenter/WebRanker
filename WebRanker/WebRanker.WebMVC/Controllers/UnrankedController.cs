using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRanker.WebMVC.Controllers
{
    public class UnrankedController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}