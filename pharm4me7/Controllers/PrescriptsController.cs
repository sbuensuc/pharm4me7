using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using pharm4me7.Models;

namespace pharm4me7.Controllers
{
    public class PrescriptsController : Controller
    {
        private ClinicContext db = new ClinicContext();

        // GET: Prescripts
        public ActionResult Index()
        {
            var prescripts = db.Prescripts.Include(p => p.item).Include(p => p.Patient);
            return View(prescripts.ToList());
        }

        // GET: Prescripts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescript prescript = db.Prescripts.Find(id);
            if (prescript == null)
            {
                return HttpNotFound();
            }
            return View(prescript);
        }

        // GET: Prescripts/Create
        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.items, "ItemId", "Name");
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName");
            return View();
        }

        // POST: Prescripts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrescriptId,PatientId,ItemId,Date,Disp,DispType,Sig,Sub,Refill")] Prescript prescript)
        {
            if (ModelState.IsValid)
            {
                db.Prescripts.Add(prescript);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.items, "ItemId", "Name", prescript.ItemId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", prescript.PatientId);
            return View(prescript);
        }

        // GET: Prescripts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescript prescript = db.Prescripts.Find(id);
            if (prescript == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.items, "ItemId", "Name", prescript.ItemId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", prescript.PatientId);
            return View(prescript);
        }

        // POST: Prescripts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PrescriptId,PatientId,ItemId,Date,Disp,DispType,Sig,Sub,Refill")] Prescript prescript)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prescript).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.items, "ItemId", "Name", prescript.ItemId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", prescript.PatientId);
            return View(prescript);
        }

        // GET: Prescripts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescript prescript = db.Prescripts.Find(id);
            if (prescript == null)
            {
                return HttpNotFound();
            }
            return View(prescript);
        }

        // POST: Prescripts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prescript prescript = db.Prescripts.Find(id);
            db.Prescripts.Remove(prescript);
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
