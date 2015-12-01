using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Budget.Models;
using Microsoft.AspNet.Identity;

namespace Budget.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                household.Users.Add(user);
                db.Households.Add(household);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Get: Households/Invite
        public ActionResult Invite()
        {
            return View();
        }

        // Post: Households/Invite
        [HttpPost]
        public ActionResult Invite(string email)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var code = new InviteCode();
            code.Code = Guid.NewGuid();
            code.HouseholdId = (int)user.HouseholdId;
            db.InviteCodes.Add(code);
            db.SaveChanges();
            new EmailService().SendAsync(new IdentityMessage()
            {
                Destination = email,
                Subject = "You have been invited to a Household Budget",
                Body = "Your invite code is: " + code.Code
            });

            return View();
        }

        public ActionResult Join()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Join(string code)
        {
            code = code.Trim();
            var inviteCode = db.InviteCodes.Where(c => c.Code.ToString() == code).First();
            var hh = inviteCode.Household;
            var user = db.Users.Find(User.Identity.GetUserId());
            hh.Users.Add(user);
            db.InviteCodes.Remove(inviteCode);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Leave()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var hh = user.Household;
            hh.Users.Remove(user);

            return RedirectToAction("Create");
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
