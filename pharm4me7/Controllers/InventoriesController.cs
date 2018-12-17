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
    public class InventoriesController : Controller
    {
        private PharmacyContext db = new PharmacyContext();
        


        // GET: Inventories
        public ActionResult Index(string sortOrder)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            int currentLocation = currentUser.Pharmacist.PharmacyId ?? default(int);

            //Sort table
            ViewBag.ItemSortParm = String.IsNullOrEmpty(sortOrder) ? "item" : "";
            ViewBag.AmountParm = sortOrder == "Amount" ? "amount_desc" : "Amount";
            ViewBag.UnitParm = sortOrder == "Unit" ? "unit_desc" : "Unit";
            ViewBag.Brand = sortOrder == "Brand" ? "brand_desc" : "Brand";

            var inventories = db.Inventories.Include(i => i.item).Include(i => i.Pharmacy).Where(po => po.PharmacyId == currentLocation);



            switch (sortOrder)
            {


                case "item":
                    inventories = inventories.OrderByDescending(p => p.item.Name);
                    break;

                case "amount_desc":
                    inventories = inventories.OrderByDescending(p => p.Amount).ThenByDescending(p => p.item.Name);
                    break;
                case "Amount":
                    inventories = inventories.OrderBy(p => p.Amount).ThenBy(p => p.item.Name);
                    break;
                case "unit_desc":
                    inventories = inventories.OrderByDescending(p => p.DispType).ThenByDescending(p => p.item.Name);
                    break;
                case "Unit":
                    inventories = inventories.OrderBy(p => p.DispType).ThenBy(p => p.item.Name);
                    break;
                case "brand_desc":
                    inventories = inventories.OrderByDescending(p => p.Brand).ThenByDescending(p => p.item.Name);
                    break;
                case "Brand":
                    inventories = inventories.OrderBy(p => p.Brand).ThenBy(p => p.item.Name);
                    break;
                default:
                    inventories = inventories.OrderBy(p => p.item.Name);
                    break;
            }

            return View(inventories.ToList());

            
        }

        // GET: Inventories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: Inventories/Create
        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.items, "ItemId", "Name");
            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name");
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InventoryId,ItemId,Amount,DispType,Brand,PharmacyId")] Inventory inventory)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            inventory.PharmacyId = currentUser.Pharmacist.PharmacyId;

            if (ModelState.IsValid)
            {
                db.Inventories.Add(inventory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.items, "ItemId", "Name", inventory.ItemId);
            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name", inventory.PharmacyId);
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.items, "ItemId", "Name", inventory.ItemId);
            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name", inventory.PharmacyId);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InventoryId,ItemId,Amount,DispType,Brand,PharmacyId")] Inventory inventory)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            inventory.PharmacyId = currentUser.Pharmacist.PharmacyId;

            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.items, "ItemId", "Name", inventory.ItemId);
            ViewBag.PharmacyId = new SelectList(db.Pharmacies, "PharmacyId", "Name", inventory.PharmacyId);
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
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
