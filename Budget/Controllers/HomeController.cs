using Budget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Budget.Helpers;
using Newtonsoft.Json;

namespace Budget.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
            var hh = db.Households.Find(Convert.ToInt32(User.Identity.GetHouseholdId()));
            return View(hh);
        }
        public ActionResult GetChart()
        {
            Household hh = User.Identity.GetUserHousehold();
            var barData = (from c in hh.Categories
                           where c.isIncome == false
                           let sum = (from b in db.Transactions
                                      where b.CategoryId == c.Id && b.BankAccount.HouseholdId == hh.Id && !b.BankAccount.IsArchived
                                      select b.Amount).DefaultIfEmpty().Sum()
                           select new
                           {
                               y = c.Name,
                               a = Math.Abs(sum),
                               b = c.BudgetedAmount
                           }).ToArray();

            var donutData = (from c in hh.Categories
                             where c.isIncome == false
                             let sum = (from t in db.Transactions
                                        where t.CategoryId == c.Id && t.BankAccount.HouseholdId == hh.Id && !t.BankAccount.IsArchived
                                        select t.Amount).DefaultIfEmpty().Sum()
                             select new
                             {
                                 label = c.Name,
                                 value = Math.Abs(sum)

                             }).ToArray();

            var JsonData = new
            {
                bar = barData,
                donut = donutData
            };



            return Content(JsonConvert.SerializeObject(JsonData), "application/json");
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