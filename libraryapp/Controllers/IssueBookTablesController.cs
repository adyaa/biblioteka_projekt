using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using DatabaseModel;

namespace libraryapp.Controllers
{
    public class IssueBookTablesController : Controller
    {
        private LibraryBDEntities db = new LibraryBDEntities();

        // GET: IssueBookTables
        public ActionResult IssueBooks()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var issueBookTables = db.IssueBookTables.Include(i => i.BookTable).Include(i => i.EmployeeTable).Include(i => i.UserTable).Where(b=>b.Status==true && b.ReserveNoOfCopies == false);
            return View(issueBookTables.ToList());
        }

        public ActionResult ReserveBooks()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var issueBookTables = db.IssueBookTables.Include(i => i.BookTable).Include(i => i.EmployeeTable).Include(i => i.UserTable).Where(b => b.Status == false && b.ReserveNoOfCopies == true && b.ReturnDate > DateTime.Now);
            return View(issueBookTables.ToList());
        }

        public ActionResult ReturnPendingBooks()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            List<IssueBookTable> list = new List<IssueBookTable>();
            var issueBookTables = db.IssueBookTables.Where(b => b.Status == true || b.ReserveNoOfCopies == true).ToList();
            
            foreach(var item in issueBookTables)
            {
                var returndate = item.ReturnDate;
                int noofdays = (returndate - DateTime.Now).Days;
                if (noofdays < 0)
                {
                    list.Add(new IssueBookTable
                    {
                        BookID = item.BookID,
                        BookTable = item.BookTable,
                        Description = item.Description,
                        EmployeeID = item.EmployeeID,
                        EmployeeTable = item.EmployeeTable,
                        IssueBookID = item.IssueBookID,
                        IssueCopies = item.IssueCopies,
                        IssueDate = item.IssueDate,
                        ReserveNoOfCopies = item.ReserveNoOfCopies,
                        ReturnDate = item.ReturnDate,
                        Status = item.Status,
                        UserID = item.UserID,
                        UserTable = item.UserTable
                    });
                }
            }
            
            return View(list.ToList());
        }
 
        // GET: IssueBookTables/Details/5
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
            IssueBookTable issueBookTable = db.IssueBookTables.Find(id);
            if (issueBookTable == null)
            {
                return HttpNotFound();
            }
            return View(issueBookTable);
        }

        // GET: IssueBookTables/Create
        public ActionResult Create()
        {
            ViewBag.BookID = new SelectList(db.BookTables, "BookID", "BookTitle","0");
            ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName","0");
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName","0");
            return View();
        }

        // POST: IssueBookTables/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IssueBookTable issueBookTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            issueBookTable.UserID = userid;
            if (ModelState.IsValid)
            {

                var find = db.IssueBookTables.Where(b => b.ReturnDate >= DateTime.Now && b.BookID == issueBookTable.BookID  && (b.Status==true || b.ReserveNoOfCopies == true)).ToList();
                int issuebooks = 0;
                foreach(var item in find)
                {
                    issuebooks = issuebooks + item.IssueCopies;
                }

                var stockbooks = db.BookTables.Where(b => b.BookID == issueBookTable.BookID).FirstOrDefault();
                if((issuebooks == stockbooks.TotalCopies) || (issuebooks + issueBookTable.IssueCopies > stockbooks.TotalCopies))
                {
                    ViewBag.Message = "Brak książek na stanie!";
                    return View(issueBookTable);
                }
                

                db.IssueBookTables.Add(issueBookTable);
                db.SaveChanges();
                ViewBag.Message = "Książka wypożyczona pomyślnie!";
                return RedirectToAction("IssueBooks");
            }

            ViewBag.BookID = new SelectList(db.BookTables, "BookID", "BookTitle", issueBookTable.BookID);
            ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName", issueBookTable.EmployeeID);
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", issueBookTable.UserID);
            return View(issueBookTable);
        }

        public ActionResult ApproveRequest(int? id)
        {
            var request = db.IssueBookTables.Find(id);
            request.ReserveNoOfCopies = false;
            request.Status = true;
            request.Description = "Zatwierdzona";
            db.Entry(request).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ReserveBooks");
           
        }


        public ActionResult BookReturn(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            var book = db.IssueBookTables.Find(id);
            int fine = 0;
            var returndate = book.ReturnDate;
            int noofdays = (DateTime.Now - returndate).Days;
            if (book.Status == true && book.ReserveNoOfCopies == false)
            {
                if (noofdays > 0)
                {
                    fine = 2 * noofdays;
                }
                var returnbook = new BookReturnTable()
                {
                    BookID = book.BookID,
                    CurrentDate = DateTime.Now,
                    EmployeeID = book.EmployeeID,
                    IssueDate = book.IssueDate,
                    ReturnDate = book.ReturnDate,
                    UserID = userid
                };
                db.BookReturnTables.Add(returnbook);
                db.SaveChanges();
            }

            book.Status = false;
            book.ReserveNoOfCopies = false;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();

            if(fine > 0 )
            {
                var addfine = new BookFineTable()
                {
                    BookID = book.BookID,
                    EmployeeID = book.EmployeeID,
                    FineAmount = fine,
                    FineDate = DateTime.Now,
                    NoOfDays = noofdays,
                    ReceiveAmount = 0,
                    UserID = userid
                };
                db.BookFineTables.Add(addfine);
                db.SaveChanges();
            }
            return RedirectToAction("IssueBooks");
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
