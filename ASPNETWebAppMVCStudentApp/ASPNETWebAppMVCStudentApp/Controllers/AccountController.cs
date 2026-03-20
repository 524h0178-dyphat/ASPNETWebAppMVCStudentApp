using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp.Models;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class AccountController : Controller
    {
        private SchoolDBLab06Entities db = new SchoolDBLab06Entities();

        // Hiển thị form đăng nhập
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            var user = db.tblUsers.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (user != null)
            {
                Session["LoggedUser"] = user.Username;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid Username or Password. Please try again.";
                return View();
            }
        }
    }
}