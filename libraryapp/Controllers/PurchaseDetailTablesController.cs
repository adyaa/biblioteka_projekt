using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseModel;

namespace libraryapp.Controllers
{
    public class PurchaseDetailTablesController : Controller
    {
        private LibraryBDEntities db = new LibraryBDEntities();

        // GET: PurchaseDetailTables
        public ActionResult Index()
        {
            var purchaseDetailTables = db.PurchaseDetailTables.Include(p => p.BookTable).Include(p => p.PurchaseTable);
            return View(purchaseDetailTables.ToList());
        }

        // GET: PurchaseDetailTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseDetailTable purchaseDetailTable = db.PurchaseDetailTables.Find(id);
            if (purchaseDetailTable == null)
            {
                return HttpNotFound();
            }
            return View(purchaseDetailTable);
        }

        // GET: PurchaseDetailTables/Create
        public ActionResult Create()
        {
            ViewBag.BookID = new SelectList(db.BookTables, "BookID", "BookTitle");
            ViewBag.PurchaseID = new SelectList(db.PurchaseTables, "PurchaseID", "PurchaseID");
            return View();
        }

        // POST: PurchaseDetailTables/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PurchaseDetailID,BookID,PurchaseID,Qty,UnitPrice")] PurchaseDetailTable purchaseDetailTable)
        {
            if (ModelState.IsValid)
            {
                db.PurchaseDetailTables.Add(purchaseDetailTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookID = new SelectList(db.BookTables, "BookID", "BookTitle", purchaseDetailTable.BookID);
            ViewBag.PurchaseID = new SelectList(db.PurchaseTables, "PurchaseID", "PurchaseID", purchaseDetailTable.PurchaseID);
            return View(purchaseDetailTable);
        }

        // GET: PurchaseDetailTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseDetailTable purchaseDetailTable = db.PurchaseDetailTables.Find(id);
            if (purchaseDetailTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookID = new SelectList(db.BookTables, "BookID", "BookTitle", purchaseDetailTable.BookID);
            ViewBag.PurchaseID = new SelectList(db.PurchaseTables, "PurchaseID", "PurchaseID", purchaseDetailTable.PurchaseID);
            return View(purchaseDetailTable);
        }

        // POST: PurchaseDetailTables/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PurchaseDetailID,BookID,PurchaseID,Qty,UnitPrice")] PurchaseDetailTable purchaseDetailTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseDetailTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookID = new SelectList(db.BookTables, "BookID", "BookTitle", purchaseDetailTable.BookID);
            ViewBag.PurchaseID = new SelectList(db.PurchaseTables, "PurchaseID", "PurchaseID", purchaseDetailTable.PurchaseID);
            return View(purchaseDetailTable);
        }

        // GET: PurchaseDetailTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseDetailTable purchaseDetailTable = db.PurchaseDetailTables.Find(id);
            if (purchaseDetailTable == null)
            {
                return HttpNotFound();
            }
            return View(purchaseDetailTable);
        }

        // POST: PurchaseDetailTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseDetailTable purchaseDetailTable = db.PurchaseDetailTables.Find(id);
            db.PurchaseDetailTables.Remove(purchaseDetailTable);
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
