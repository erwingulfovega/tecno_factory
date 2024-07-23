using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MDBinASP.NET.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["username"] != string.Empty && Session["username"]!=null)
            {
                ViewBag.username  = Session["username"];
                ViewBag.idusuario = Session["idusuario"];
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }

        public ActionResult Tables()
        {
            ViewBag.Message = "Sample page for Tables.";

            return View();
        }

        public ActionResult Maps()
        {
            ViewBag.Message = "Sample page for Maps.";

            return View();
        }

        public ActionResult Orders()
        {
            ViewBag.Message = "Sample page for Orders.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Sample page for Login.";

            return View();
        }
    }
}