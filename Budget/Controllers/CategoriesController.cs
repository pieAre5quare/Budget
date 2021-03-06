﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Budget.Models;
using Budget.Helpers;

namespace Budget.Controllers
{
    [RequireHttps]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        [AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
            var hhId = Convert.ToInt32(User.Identity.GetHouseholdId());
            var categories = db.Categories.Where(c => c.HouseholdId == hhId);
            return View(categories.ToList());
        }

        // GET: Categories/Details/5
        [AuthorizeHouseholdRequired]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var transactions = db.Transactions.Where(t => t.CategoryId == category.Id && 
                t.Date.Month.CompareTo(DateTimeOffset.Now.Month) == 0
                && t.Date.Year.CompareTo(DateTimeOffset.Now.Year) == 0).ToList();
            decimal budgetUsed = 0;
            foreach(var trans in transactions)
            {
                budgetUsed += Math.Abs(trans.Amount);
            }

            var vm = new CatDetailsVM();
            vm.Category = category;
            vm.BudgetUsed = budgetUsed;

            return View(vm);
        }

        // GET: Categories/Create
        [AuthorizeHouseholdRequired]
        public ActionResult Create()
        {
            var hhId = Convert.ToInt32(User.Identity.GetHouseholdId());
            var cat = new Category();
            cat.HouseholdId = hhId;
            return View(cat);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,BudgetedAmount,HouseholdId")] Category category)
        {
            if (ModelState.IsValid)
            {
                
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", category.HouseholdId);
            return View(category);
        }

        // GET: Categories/Edit/5
        [AuthorizeHouseholdRequired]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", category.HouseholdId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,BudgetedAmount,HouseholdId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", category.HouseholdId);
            return View(category);
        }

        // GET: Categories/Delete/5
        [AuthorizeHouseholdRequired]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            var transaction = db.Transactions.Where(t => t.CategoryId == id).ToList();
            foreach (var trans in transaction)
            {
                trans.CategoryId = null;
            }
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        

        public PartialViewResult _CatTransactions(int id)
        {
            var hhId = Convert.ToInt32(User.Identity.GetHouseholdId());
            var transactions = db.Transactions.Where(t => t.CategoryId == id && t.BankAccount.HouseholdId == hhId).ToList();
            return PartialView(transactions);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
