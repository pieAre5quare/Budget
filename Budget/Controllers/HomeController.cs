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

        public PartialViewResult _RecentTransactions()
        {
            var hhId = Convert.ToInt32(User.Identity.GetHouseholdId());
            List<Transaction> transactions = db.Transactions.Where(t => t.BankAccount.HouseholdId == hhId && !t.BankAccount.IsArchived )
                .OrderByDescending(tran => tran.Date).Take(10).ToList();
            
            return PartialView(transactions);
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

        public ActionResult Chart()
        {
            return View();
        }

    }
}