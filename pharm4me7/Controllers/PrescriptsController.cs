﻿using System;
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
    public class PrescriptsController : Controller
    {
        private Pharm4MeContext db = new Pharm4MeContext();
        

        // GET: Prescripts
        public ActionResult Index(string sortOrder)
        {
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date" : "";
            ViewBag.FnameParm = sortOrder == "FirstName" ? "fName_desc" : "FirstName";
            ViewBag.LnameParm = sortOrder == "LastName" ? "lName_desc" : "LastName";
            ViewBag.ItemParm = sortOrder == "Item" ? "item_desc" : "Item";
            ViewBag.AmountParm = sortOrder == "Amount" ? "amount_desc" : "Amount";
            ViewBag.UnitParm = sortOrder == "Unit" ? "unit_desc" : "Unit";
            ViewBag.Brand = sortOrder == "Brand" ? "brand_desc" : "Brand";
            ViewBag.DocParm = sortOrder == "Doctor" ? "doctor_desc" : "Doctor";

            var prescripts = db.Prescripts.Include(p => p.item).Include(p => p.Patient);

            switch (sortOrder)
            {
                case "date":
                    prescripts = prescripts.OrderBy(p => p.Date);
                    break;
                case "fName_desc":
                    prescripts = prescripts.OrderByDescending(p => p.Patient.FirstName).ThenByDescending(p => p.Patient.LastName);
                    break;
                case "FirstName":
                    prescripts = prescripts.OrderBy(p => p.Patient.FirstName).ThenBy(p => p.Patient.LastName);
                    break;
                case "lName_desc":
                    prescripts = prescripts.OrderByDescending(p => p.Patient.LastName).ThenByDescending(p => p.Patient.FirstName);
                    break;
                case "LastName":
                    prescripts = prescripts.OrderBy(p => p.Patient.LastName).ThenBy(p => p.Patient.FirstName);
                    break;
                case "item_desc":
                    prescripts = prescripts.OrderByDescending(p => p.item).ThenByDescending(p => p.Date);
                    break;
                case "Item":
                    prescripts = prescripts.OrderBy(p => p.item).ThenBy(p => p.Date);
                    break;
                case "amount_desc":
                    prescripts = prescripts.OrderByDescending(p => p.Disp).ThenByDescending(p => p.Date);
                    break;
                case "Amount":
                    prescripts = prescripts.OrderBy(p => p.Disp).ThenBy(p => p.Date);
                    break;
                case "unit_desc":
                    prescripts = prescripts.OrderByDescending(p => p.DispType).ThenByDescending(p => p.Date);
                    break;
                case "Unit":
                    prescripts = prescripts.OrderBy(p => p.DispType).ThenBy(p => p.Date);
                    break;
                case "brand_desc":
                    prescripts = prescripts.OrderByDescending(p => p.item).ThenByDescending(p => p.Date);
                    break;
                case "Brand":
                    prescripts = prescripts.OrderBy(p => p.item).ThenBy(p => p.Date);
                    break;
                case "doctor_desc":
                    prescripts = prescripts.OrderByDescending(p => p.Doctor.LastName).ThenByDescending(p => p.Date);
                    break;
                case "Doctor":
                    prescripts = prescripts.OrderBy(p => p.item).ThenBy(p => p.Date);
                    break;

                default:
                    prescripts = prescripts.OrderByDescending(p => p.Date);
                    break;
            }
            return View(prescripts.ToList());
        }

        public ActionResult PatientsIndex()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            int? value = currentUser.PatientId;
            int currentPatient = value.Value;

            var patient = db.Patients.Find(currentPatient);

            


            var prescripts = db.Prescripts.Include(p => p.item).Include(p => p.Patient).Where(p => p.PatientId == currentPatient);
            return View(prescripts.ToList());
        }

        [ActionName("PorderCreate")]
        public ActionResult Create(int? id)
        {
            Prescript prescript = db.Prescripts.Find(id);
            POrder model = new POrder();

            var pharmacies = db.Pharmacies.ToList();

            IEnumerable<SelectListItem> selectList = from p in pharmacies
                                                     select new SelectListItem
                                                     {
                                                         Value = p.PharmacyId.ToString(),
                                                         Text = string.Format("{0} - {1}, {2}", p.Name, p.Address, p.City)
                                                     };

           
            
            ViewBag.PharmacyId = new SelectList(selectList, "Value", "Text");
            ViewData["Prescript"] = prescript;
            model.PrescriptId = prescript.PrescriptId;
            return View(model);
        }

        // POST: POrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ActionName("PorderCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "POrderId,PrescriptId,PharmacyId,DateOrdered,Note,Fill,Deny,Accept")] POrder pOrder)
        {
            pOrder.DateOrdered = System.DateTime.Now;
            pOrder.Deny = false;
            pOrder.Fill = false;
            pOrder.Accept = false;
            if (ModelState.IsValid)
            {
                var prectiptToUpdate = db.Prescripts.Find(pOrder.PrescriptId);
                prectiptToUpdate.Sent = true;
                db.Entry(prectiptToUpdate).State = EntityState.Modified;
                db.POrders.Add(pOrder);
                db.SaveChanges();
                return RedirectToAction("PatientsIndex");
            }

            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name", pOrder.PharmacyId);
            ViewBag.PrescriptId = new SelectList(db.Prescripts, "PrescriptId", "DispType", pOrder.PrescriptId);
            return View(pOrder);
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
            var patients = db.Patients.OrderBy(p => p.LastName).OrderBy(p => p.PatientId).ToList();

            IEnumerable<SelectListItem> selectList = from p in patients
                                                     select new SelectListItem
                                                     {
                                                         Value = p.PatientId.ToString(),
                                                         Text = string.Format("{0} - {1}, {2}", p.PatientId, p.LastName, p.FirstName)
                                                     };

            ViewBag.ItemId = new SelectList(db.items.OrderBy(p => p.Name), "ItemId", "Name");
            ViewBag.PatientId = new SelectList(selectList, "Value", "Text");
            return View();

            
        }

        // POST: Prescripts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrescriptId,PatientId,ItemId,Date,Disp,DispType,Sig,Sub,Refill")] Prescript prescript)
        {
            prescript.Date = System.DateTime.Now;
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
