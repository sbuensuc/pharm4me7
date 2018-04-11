﻿using System;
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
    public class POrderFillsController : Controller
    {
        private ClinicContext db = new ClinicContext();

        // GET: POrderFills
        public ActionResult Index()
        {
            var pOrderFills = db.POrderFills.Include(p => p.Inventory).Include(p => p.POrder);
            return View(pOrderFills.ToList());
        }

        // GET: POrderFills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POrderFill pOrderFill = db.POrderFills.Find(id);
            if (pOrderFill == null)
            {
                return HttpNotFound();
            }
            return View(pOrderFill);
        }

        // GET: POrderFills/Create
        public ActionResult Create()
        {
            ViewBag.InventoryId = new SelectList(db.Inventories, "InventoryId", "DispType");
            ViewBag.POrderId = new SelectList(db.POrders, "POrderId", "Note");
            return View();
        }

        // POST: POrderFills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "POrderFillId,POrderId,InventoryId,DateFilled,DatePicked,Note,PhamacistId,Ready")] POrderFill pOrderFill)
        {
            if (ModelState.IsValid)
            {
                db.POrderFills.Add(pOrderFill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InventoryId = new SelectList(db.Inventories, "InventoryId", "DispType", pOrderFill.InventoryId);
            ViewBag.POrderId = new SelectList(db.POrders, "POrderId", "Note", pOrderFill.POrderId);
            return View(pOrderFill);
        }

        // GET: POrderFills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POrderFill pOrderFill = db.POrderFills.Find(id);
            if (pOrderFill == null)
            {
                return HttpNotFound();
            }
            ViewBag.InventoryId = new SelectList(db.Inventories, "InventoryId", "DispType", pOrderFill.InventoryId);
            ViewBag.POrderId = new SelectList(db.POrders, "POrderId", "Note", pOrderFill.POrderId);
            return View(pOrderFill);
        }

        // POST: POrderFills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "POrderFillId,POrderId,InventoryId,DateFilled,DatePicked,Note,PhamacistId,Ready")] POrderFill pOrderFill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pOrderFill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InventoryId = new SelectList(db.Inventories, "InventoryId", "DispType", pOrderFill.InventoryId);
            ViewBag.POrderId = new SelectList(db.POrders, "POrderId", "Note", pOrderFill.POrderId);
            return View(pOrderFill);
        }

        // GET: POrderFills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POrderFill pOrderFill = db.POrderFills.Find(id);
            if (pOrderFill == null)
            {
                return HttpNotFound();
            }
            return View(pOrderFill);
        }

        // POST: POrderFills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            POrderFill pOrderFill = db.POrderFills.Find(id);
            db.POrderFills.Remove(pOrderFill);
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
