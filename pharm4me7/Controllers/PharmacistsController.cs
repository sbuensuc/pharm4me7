using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using pharm4me7.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace pharm4me7.Controllers
{
    //test
    public class PharmacistsController : Controller
    {
        private Pharm4MeContext db = new Pharm4MeContext();

        // GET: Pharmacists
        public ActionResult Index()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            int currentLocation = currentUser.Pharmacist.PharmacyId ?? default(int);

            var pharmacists = db.Pharmacists.Include(p => p.Pharmacy).Where(po => po.PharmacyId == currentLocation);
            return View(pharmacists.ToList());
        }

        // GET: Pharmacists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pharmacist pharmacist = db.Pharmacists.Find(id);
            if (pharmacist == null)
            {
                return HttpNotFound();
            }
            return View(pharmacist);
        }

        // GET: Pharmacists/Create
        public ActionResult Create()
        {
            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name");
            return View();
        }

        // POST: Pharmacists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PharmacistId,FirstName,LastName,Email,PharmacyId")] Pharmacist pharmacist)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            pharmacist.PharmacyId = currentUser.Pharmacist.PharmacyId;

            if (ModelState.IsValid)
            {
                db.Pharmacists.Add(pharmacist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name", pharmacist.PharmacyId);
            return View(pharmacist);
        }

        // GET: Pharmacists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pharmacist pharmacist = db.Pharmacists.Find(id);
            if (pharmacist == null)
            {
                return HttpNotFound();
            }
            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name", pharmacist.PharmacyId);
            return View(pharmacist);
        }

        // POST: Pharmacists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PharmacistId,FirstName,LastName,Email,PharmacyId")] Pharmacist pharmacist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pharmacist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name", pharmacist.PharmacyId);
            return View(pharmacist);
        }

        // GET: Pharmacists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pharmacist pharmacist = db.Pharmacists.Find(id);
            if (pharmacist == null)
            {
                return HttpNotFound();
            }
            return View(pharmacist);
        }

        // POST: Pharmacists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pharmacist pharmacist = db.Pharmacists.Find(id);
            db.Pharmacists.Remove(pharmacist);
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
