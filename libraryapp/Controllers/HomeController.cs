using DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace libraryapp.Controllers
{
    public class HomeController : Controller
    {
        private LibraryBDEntities db = new LibraryBDEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(string username, string password)
        {
            try
            {
                if (username != null && password != null)
                {
                    var finduser = db.UserTables.Where(u => u.UserName == username && u.Password == password && u.IsActive==true).ToList();
                    if (finduser.Count() == 1)
                    {
                        Session["UserID"] = finduser[0].UserID;
                        Session["UserTypeID"] = finduser[0].UserTypeID;
                        Session["UserName"] = finduser[0].UserName;
                        Session["Password"] = finduser[0].Password;
                        Session["EmployeeID"] = finduser[0].EmployeeID;
                        //Typy użytkowników w bazie:
                        // 1 - Admin
                        // 2 - Pracownik
                        // 3 - Czytelnik


                        string url = string.Empty;
                        if (finduser[0].UserTypeID == 2)
                        {
                            return RedirectToAction("About");
                        }
                        else if (finduser[0].UserTypeID == 3)
                        {
                            return RedirectToAction("About");
                        }
                        else if (finduser[0].UserTypeID == 4)
                        {
                            return RedirectToAction("About");
                        }
                        else if (finduser[0].UserTypeID == 1)
                        {
                            url = "About";
                        }
                        else
                        {
                            url = "About";
                        }
                        return RedirectToAction(url);

                    }
                    else
                    {
                        Session["UserID"] = string.Empty;
                        Session["UserTypeID"] = string.Empty;
                        Session["UserName"] = string.Empty;
                        Session["Password"] = string.Empty;
                        Session["EmployeeID"] = string.Empty;
                        ViewBag.message = "Wprowadzono nieprawidłowe dane";
                    }
                }
                else
                {
                    Session["UserID"] = string.Empty;
                    Session["UserTypeID"] = string.Empty;
                    Session["UserName"] = string.Empty;
                    Session["Password"] = string.Empty;
                    Session["EmployeeID"] = string.Empty;
                    ViewBag.message = "Wystąpił nieoczekiwany błąd";
                }

            }
            catch (Exception ex)
            {
                Session["UserID"] = string.Empty;
                Session["UserTypeID"] = string.Empty;
                Session["UserName"] = string.Empty;
                Session["Password"] = string.Empty;
                Session["EmployeeID"] = string.Empty;
                ViewBag.message = "Błąd";
            }
            return View("Login");
        }





        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {

                int usertypeid = 0;
                if (Session["UserID"] != null)
                {
                    usertypeid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
                }
                if(usertypeid == 3)
                {
                return RedirectToAction("Index", "ReserveBook", null);
                }

            return RedirectToAction("Index", "ReserveBook", null);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    


    public ActionResult Logout()
    {
        Session["UserID"] = string.Empty;
        Session["UserTypeID"] = string.Empty;
        Session["UserName"] = string.Empty;
        Session["Password"] = string.Empty;
        Session["EmployeeID"] = string.Empty;
        return RedirectToAction("Login");

    }

   }
}