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
    [RequireHttps]
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetChart()
        {
            Household hh = User.Identity.GetUserHousehold();
            var barData = (from c in hh.Categories
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

       
        
    }
}
