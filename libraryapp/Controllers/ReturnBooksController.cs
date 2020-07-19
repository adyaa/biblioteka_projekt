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
    public class ReturnBooksController : Controller
    {
        private LibraryBDEntities db = new LibraryBDEntities();
        // GET: ReturnBooks
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var returnbooks = db.BookReturnTables.ToList();

            return View(returnbooks);
        }
    }
}