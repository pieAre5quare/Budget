using Budget.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Budget.Helpers;

namespace Budget.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetChart()
        {
            Household hh = User.Identity.GetUserHousehold();
            var bar = (from c in hh.Categories
                       where c.isIncome == false
                       let sum = (from b in db.Transactions
                                  where b.CategoryId == c.Id && b.BankAccount.HouseholdId == hh.Id
                                  select b.Amount).DefaultIfEmpty().Sum()
                       select new
                       {
                           y = c.Name,
                           a = Math.Abs(sum),
                           b = c.BudgetedAmount
                       }).ToArray();

            //var s = new[] { new { label = "2008", value= 20 },
            //    new { label= "2008", value= 5 },
            //    new { label= "2010", value= 7 },
            //    new { label= "2011", value= 10 },
            //    new { label= "2012", value= 20 }};
            return Content(JsonConvert.SerializeObject(bar), "application/json");
        }

        public ActionResult SpendingChart()
        {
            Household hh = User.Identity.GetUserHousehold();
            var donut = (from t in db.Transactions
                         where t.BankAccount.HouseholdId == hh.Id && !t.IsDeposit && !t.BankAccount.IsArchived)
        }
    }
}
