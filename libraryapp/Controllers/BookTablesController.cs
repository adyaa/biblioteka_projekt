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
    public class BookTablesController : Controller
    {
        private LibraryBDEntities db = new LibraryBDEntities();

        // GET: BookTables
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var bookTables = db.BookTables.Include(b => b.BookTypeTable).Include(b => b.DepartmentTable).Include(b => b.UserTable);
            return View(bookTables.ToList());
        }

        // GET: BookTables/Details/5
        public ActionResult Details(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTable bookTable = db.BookTables.Find(id);
            if (bookTable == null)
            {
                return HttpNotFound();
            }
            return View(bookTable);
        }

        // GET: BookTables/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.BookTypeID = new SelectList(db.BookTypeTables, "BookTypeID", "Name","0");
            ViewBag.DepartmentID = new SelectList(db.DepartmentTables, "DepartmentID", "Name","0");
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName","0");
            return View();
        }

        // POST: BookTables/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookTable bookTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            bookTable.UserID = userid;
            if (ModelState.IsValid)
            {
                db.BookTables.Add(bookTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookTypeID = new SelectList(db.BookTypeTables, "BookTypeID", "Name", bookTable.BookTypeID);
            ViewBag.DepartmentID = new SelectList(db.DepartmentTables, "DepartmentID", "Name", bookTable.DepartmentID);
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookTable.UserID);
            return View(bookTable);
        }

        // GET: BookTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTable bookTable = db.BookTables.Find(id);
            if (bookTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookTypeID = new SelectList(db.BookTypeTables, "BookTypeID", "Name", bookTable.BookTypeID);
            ViewBag.DepartmentID = new SelectList(db.DepartmentTables, "DepartmentID", "Name", bookTable.DepartmentID);
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookTable.UserID);
            return View(bookTable);
        }

        // POST: BookTables/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookTable bookTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            bookTable.UserID = userid;
            if (ModelState.IsValid)
            {
                db.Entry(bookTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookTypeID = new SelectList(db.BookTypeTables, "BookTypeID", "Name", bookTable.BookTypeID);
            ViewBag.DepartmentID = new SelectList(db.DepartmentTables, "DepartmentID", "Name", bookTable.DepartmentID);
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookTable.UserID);
            return View(bookTable);
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
