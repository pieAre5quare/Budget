using Budget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Budget.Helpers;

namespace Budget.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
            var hh = db.Households.Find(Convert.ToInt32(User.Identity.GetHouseholdId()));
            return View(hh);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}