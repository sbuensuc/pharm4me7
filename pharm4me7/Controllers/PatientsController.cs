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
    public class PatientsController : Controller
    {
        //test
        private OrderContext db = new OrderContext();
        // GET: Patients
        public ActionResult Index(string sortOrder)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.PatientIdSortParm = String.IsNullOrEmpty(sortOrder) ? "patientid_desc" : "";
            ViewBag.FnameParm = sortOrder == "FirstName" ? "fName_desc" : "FirstName";
            ViewBag.LnameParm = sortOrder == "LastName" ? "lName_desc" : "LastName";
            ViewBag.DateSortParm = sortOrder == "DOB" ? "DOB_desc" : "DOB";


            var patients = db.Patients.Include(p => p.Clinic).Where(p => p.ClinicId == currentUser.Doctor.ClinicId);
            
            return View(patients.ToList());
        }

        // GET: Patients/PharmacyIndex
        public ActionResult PharmacyIndex(string sortOrder)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var currentUser = manager.FindById(User.Identity.GetUserId());
            int currentLocation = currentUser.Pharmacist.PharmacyId ?? default(int);

            var pOrders = db.POrders.Where(po => po.PharmacyId == currentLocation);

            List<Patient> patients = new List<Patient>();
            
                foreach (var p in pOrders)
                {
                var patient = p.Prescript.Patient;

                patients.Add(patient);

                }

            ViewBag.PatientIdSortParm = String.IsNullOrEmpty(sortOrder) ? "lastname" : "";
            ViewBag.FnameParm = sortOrder == "FirstName" ? "fName_desc" : "FirstName";
            ViewBag.CityParm = sortOrder == "City" ? "city_desc" : "City";

            IEnumerable<Patient> finalpatients = patients.Distinct().ToList();

            switch (sortOrder)
            {
                case "lName_desc":
                    finalpatients = patients.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName);
                    break;
                case "fName_desc":
                    finalpatients = finalpatients.OrderByDescending(p => p.FirstName).ThenByDescending(p => p.LastName);
                    break;
                case "FirstName":
                    finalpatients = finalpatients.OrderBy(p => p.FirstName).ThenBy(p => p.LastName);
                    break;
                case "city_desc":
                    finalpatients = finalpatients.OrderByDescending(p => p.City).ThenByDescending(p => p.LastName);
                    break;
                case "City":
                    finalpatients = finalpatients.OrderBy(p => p.City).ThenBy(p => p.LastName);
                    break;
                case "DOB_desc":
                    finalpatients = finalpatients.OrderByDescending(p => p.Birth).ThenByDescending(p => p.LastName);
                    break;
                case "DOB":
                    finalpatients = finalpatients.OrderBy(p => p.Birth).ThenBy(p => p.LastName);
                    break;
                default:
                    finalpatients = finalpatients.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
                    break;
            }

            
            return View(patients.Distinct().ToList());
        }

        // GET: Patients/AdminIndex
        public ActionResult AdminIndex()
        {
            var pharmacists = db.Pharmacists.Include(p => p.Pharmacy);
            var doctors = db.Doctors.Include(d => d.Clinic);

            ViewData["Doctors"] = doctors.ToList();
            ViewData["Pharmacists"] = pharmacists.ToList();
            var patients = db.Patients.Include(p => p.Clinic);
            return View(patients.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            //var viewModel = new PatientPrescriptModel();
            //viewModel.Prescripts = prescripts;
            //viewModel.Patient = patient;

            //return View(viewModel);
            return View(patient);

        }

        // GET: Patients/Details/5
        public ActionResult PharmacyDetails(int? id, int? requestid)
        {

            

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var currentUser = manager.FindById(User.Identity.GetUserId());
            int currentLocation = currentUser.Pharmacist.PharmacyId ?? default(int);
            var pOrders = db.POrders.Include(p => p.Pharmacy).Include(p => p.Prescript).Where(po => po.PharmacyId == currentLocation);

            List<POrderFill> pOrderFills = new List<POrderFill>();



            foreach (var p in pOrders)
            {
               
                var pOrderFill = p.POrderFills.Where(po => po.POrder.Prescript.PatientId == id);

                foreach (var i in pOrderFill)
                {
                    pOrderFills.Add(i);
                }
            }

            var patientPOrders = pOrders.Where(po => po.Prescript.PatientId == id).Distinct().ToList();

            ViewData["pOrders"] = patientPOrders;
            ViewData["porderFills"] = pOrderFills.Distinct().ToList();

            //var prescripts = db.Prescripts.Include(p => p.item).Include(p => p.Patient);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);

        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            ViewBag.ClinicId = new SelectList(db.Clinics, "ClinicId", "Name");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientId,FirstName,LastName,Healthcard,Address,City,Phone,Gender,Birth,ClinicId")] Patient patient)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            patient.ClinicId = currentUser.Doctor.ClinicId;

            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClinicId = new SelectList(db.Clinics, "ClinicId", "Name", patient.ClinicId);
            return View(patient);
        }

        // GET: Prescripts/Create
        [ActionName("PrescriptCreate")]
        public ActionResult Create(int? id)
        {
            

            Patient patient = db.Patients.Find(id);
            Prescript model = new Prescript();

            ViewData["Patient"] = patient;

            ViewBag.ItemId = new SelectList(db.items, "ItemId", "Name");
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName");
            model.PatientId = patient.PatientId;

            return View(model);


        }

        // POST: Prescripts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ActionName("PrescriptCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrescriptId,PatientId,ItemId,Date,Disp,DispType,Sig,Sub,Refill")] Prescript prescript)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            prescript.Date = System.DateTime.Now;

            prescript.DoctorId = currentUser.DoctorId;

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

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClinicId = new SelectList(db.Clinics, "ClinicId", "Name", patient.ClinicId);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientId,FirstName,LastName,Healthcard,Address,City,Phone,Gender,Birth,ClinicId")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClinicId = new SelectList(db.Clinics, "ClinicId", "Name", patient.ClinicId);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Patients/Delete/5
        public ActionResult PrescriptDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescript prescripts = db.Prescripts.Find(id);
            if (prescripts == null)
            {
                return HttpNotFound();
            }
            return View(prescripts);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete Prescription")]
        [ValidateAntiForgeryToken]
        public ActionResult PrescriptsDeleteConfirmed(int id)
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
