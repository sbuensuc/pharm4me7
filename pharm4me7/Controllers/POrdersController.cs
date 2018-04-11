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
    public class POrdersController : Controller
    {
        private OrderContext db = new OrderContext();

        // GET: POrders
        public ActionResult Index()
        {
            var pOrders = db.POrders.Include(p => p.Pharmacy).Include(p => p.Prescript);
            return View(pOrders.ToList());
        }

        //public ActionResult PharmacyIndex()
        //{
        //    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //    var currentUser = manager.FindById(User.Identity.GetUserId());
        //    int? value = currentUser.Pharmacist.PharmacyId;
        //    int currentLocation = value.Value;

        //    var pOrders = db.POrders.Include(p => p.Pharmacy).Include(p => p.Prescript).Where(p => p.PharmacyId == currentLocation);
        //    return View(pOrders.ToList());
        //}

        
        public ActionResult PharmacyIndex(int? id)
        {
            if (id != null)
            {
                POrder pOrder = db.POrders.Find(id);
                pOrder.Accept = true;
                db.Entry(pOrder).State = EntityState.Modified;

                db.SaveChanges();
            }
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            int? value = currentUser.Pharmacist.PharmacyId;
            int currentLocation = value.Value;

            var pOrders = db.POrders.Include(p => p.Pharmacy).Include(p => p.Prescript).Where(p => p.PharmacyId == currentLocation);
            return View(pOrders.ToList());
        }

        // GET: POrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POrder pOrder = db.POrders.Find(id);
            if (pOrder == null)
            {
                return HttpNotFound();
            }
            return View(pOrder);
        }

        // GET: POrders/Create
        public ActionResult Create()
        {
            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name");
            ViewBag.PrescriptId = new SelectList(db.Prescripts, "PrescriptId", "PrescriptId");
            return View();
        }

        [ActionName("PorderCreate")]
        public ActionResult Create(int? id)
        {
            Prescript prescript = db.Prescripts.Find(id);
            POrder model = new POrder();
            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name");
            model.PrescriptId = prescript.PrescriptId;
            return View(model);
        }

        // POST: POrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "POrderId,PrescriptId,PharmacyId,DateOrdered,Note,Fill,Deny,Accept")] POrder pOrder)
        {
            if (ModelState.IsValid)
            {
                db.POrders.Add(pOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name", pOrder.PharmacyId);
            ViewBag.PrescriptId = new SelectList(db.Prescripts, "PrescriptId", "DispType", pOrder.PrescriptId);
            return View(pOrder);
        }

        // GET: POrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POrder pOrder = db.POrders.Find(id);
            if (pOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name", pOrder.PharmacyId);
            ViewBag.PrescriptId = new SelectList(db.Prescripts, "PrescriptId", "DispType", pOrder.PrescriptId);
            return View(pOrder);
        }

        // POST: POrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "POrderId,PrescriptId,PharmacyId,DateOrdered,Note,Fill,Deny,Accept")] POrder pOrder)
        {
            pOrder.Accept = false;
            pOrder.Deny = true;

            Prescript prescript = new Prescript();
            prescript = db.Prescripts.Find(pOrder.PrescriptId);

            prescript.Sent = false;

            if (ModelState.IsValid)
            {
                db.Entry(prescript).State = EntityState.Modified;
                db.Entry(pOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PharmacyIndex");
            }
            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name", pOrder.PharmacyId);
            ViewBag.PrescriptId = new SelectList(db.Prescripts, "PrescriptId", "DispType", pOrder.PrescriptId);
            return View(pOrder);
        }

        // GET: POrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POrder pOrder = db.POrders.Find(id);
            if (pOrder == null)
            {
                return HttpNotFound();
            }
            return View(pOrder);
        }

        // POST: POrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            POrder pOrder = db.POrders.Find(id);
            db.POrders.Remove(pOrder);
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
