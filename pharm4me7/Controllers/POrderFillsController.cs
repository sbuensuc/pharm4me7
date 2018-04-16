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
    public class POrderFillsController : Controller
    {
        private Pharm4MeContext db = new Pharm4MeContext();

        // GET: POrderFills
        public ActionResult Index()
        {
            var pOrderFills = db.POrderFills.Include(p => p.Inventory).Include(p => p.POrder);
            return View(pOrderFills.ToList());
        }

        public ActionResult PharmacyIndex(int? id)
        {
            //if (id != null)
            //{
            //    POrder pOrder = db.POrders.Find(id);
            //    pOrder.Accept = true;
            //    db.Entry(pOrder).State = EntityState.Modified;

            //    db.SaveChanges();
            //}
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            int? value = currentUser.Pharmacist.PharmacyId;
            int currentLocation = value.Value;
            var pOrders = db.POrders.Include(p => p.Pharmacy).Include(p => p.Prescript).Where(p => p.PharmacyId == currentLocation).Where(p => p.Accept == true);

            
            ViewData["POrders"] = pOrders.ToList();
            var pOrderFills = db.POrderFills.Include(p => p.Inventory).Include(p => p.POrder).Where(p => p.POrder.PharmacyId == currentLocation); ;
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

        public ActionResult PharmacyCreate(int? id)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            var pOrders = db.POrders.Include(p => p.Pharmacy).Include(p => p.Prescript).Where(p => p.PharmacyId == currentUser.Pharmacist.PharmacyId).Where(p => p.Accept == true);

            POrder porder = db.POrders.Find(id);
            POrderFill model = new POrderFill();
            ViewData["porder"] = porder;

            var inventories = db.Inventories.Where(i => i.PharmacyId == currentUser.Pharmacist.PharmacyId).Where(i => i.ItemId == porder.Prescript.ItemId).Where(i => i.Amount >= porder.Prescript.Disp).ToList();

            IEnumerable<SelectListItem> selectList = from i in inventories
                                                     select new SelectListItem
                                                     {
                                                         Value = i.InventoryId.ToString(),
                                                         Text = string.Format("{0} {1} {2}", i.Brand , i.item.Name, i.DispType)
                                                     };

            ViewBag.InventoryId = new SelectList(selectList, "Value", "Text");
            model.POrderId = porder.POrderId;
            return View(model);
        }

        // POST: POrderFills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PharmacyCreate([Bind(Include = "POrderFillId,POrderId,InventoryId,DateFilled,DatePicked,Note,PhamacistId,Ready")] POrderFill pOrderFill)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            int? value = currentUser.PharmacistId;

            

            pOrderFill.PhamacistId = value;
            pOrderFill.DateFilled = System.DateTime.Now;

            POrder pOrder = db.POrders.Find(pOrderFill.POrderId);
            pOrder.Fill = true;

            Inventory inventory = db.Inventories.Find(pOrderFill.InventoryId);
            inventory.Amount = inventory.Amount - pOrder.Prescript.Disp;
            pOrderFill.Ready = true;

            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                db.Entry(pOrder).State = EntityState.Modified;
                db.POrderFills.Add(pOrderFill);
                db.SaveChanges();
                return RedirectToAction("PharmacyIndex");
            }

            ViewBag.InventoryId = new SelectList(db.Inventories, "InventoryId", "DispType", pOrderFill.InventoryId);
            ViewBag.POrderId = new SelectList(db.POrders, "POrderId", "Note", pOrderFill.POrderId);
            return View(pOrderFill);
        }

        [ActionName("PharmacyPickup")]
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
        [ActionName("PharmacyPickup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "POrderFillId,POrderId,InventoryId,DateFilled,DatePicked,Note,PhamacistId,Ready")] POrderFill pOrderFill)
        {
            pOrderFill.DatePicked = System.DateTime.Now;
            pOrderFill.Ready = false;

            POrder pOrder = db.POrders.Find(pOrderFill.POrderId);
            pOrder.Deny = true;

            if (ModelState.IsValid)
            {
                db.Entry(pOrder).State = EntityState.Modified;
                db.Entry(pOrderFill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PharmacyIndex", "Patients");
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
