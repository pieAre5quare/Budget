using System;
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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        [AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
            var hhId = Convert.ToInt32(User.Identity.GetHouseholdId());
            var transactions = db.Transactions.Where(trans => trans.BankAccount.HouseholdId == hhId)
                .Include(t => t.BankAccount).Include(t => t.Category);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        [AuthorizeHouseholdRequired]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        [AuthorizeHouseholdRequired]
        public ActionResult Create(int id)
        {
            var hh = db.Households.Find(Convert.ToInt32(User.Identity.GetHouseholdId()));

            ViewBag.CategoryId = new SelectList(hh.Categories, "Id", "Name");
            Transaction transaction = new Transaction();
            transaction.BankAccountId = id;
            transaction.Date = DateTimeOffset.Now;
            
            return View(transaction);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BankAccountId,CategoryId,IsDeposit,IsReconciled,Date,Amount,Description")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var account = db.BankAccounts.Find(transaction.BankAccountId);
                if (!transaction.IsDeposit)
                    transaction.Amount *= -1;
                var cat = db.Categories.Find(transaction.CategoryId);
                
                account.Balance += transaction.Amount;
                db.Entry(cat).State = EntityState.Modified;
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        [AuthorizeHouseholdRequired]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BankAccountId,CategoryId,IsDeposit,IsReconciled,Date,Amount,Description")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var oldTransaction = db.Transactions.AsNoTracking().FirstOrDefault(t => t.Id == transaction.Id);
                if(oldTransaction.Amount != transaction.Amount)
                {
                    var account = db.BankAccounts.Find(transaction.BankAccountId);
                    account.Balance -= oldTransaction.Amount;
                    account.Balance += transaction.Amount;
                }
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        [AuthorizeHouseholdRequired]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            var account = db.BankAccounts.Find(transaction.BankAccountId);
            account.Balance -= transaction.Amount;
            db.Entry(account).State = EntityState.Modified;
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
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
