using artiktamam.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using artiktamam.Database;
using System.Collections.Generic;

namespace artiktamam.Controllers
{
    public class LoginController : Controller
    {
        private DatabaseIslemleri dbIslem = new DatabaseIslemleri();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Register(Register register)

        {
            string mesaj = dbIslem.Register(register);
            if (mesaj=="Bu Username Daha önce alınmıs")
            {
                ViewBag.DuplicateMessage = mesaj;
                return View("Index");
            }
            else if (mesaj == "Bu E-Posta Daha önce alınmıs")
            {
                ModelState.Clear();
                ViewBag.DuplicateMessage = mesaj;
                return View("Index");
            }
            else if (mesaj == "Kayıt oldunuz lütfen giriş yapınız")
            {
                ModelState.Clear();
                ViewBag.SuccessMessage = mesaj;
                return View("Index");
            }
            else
            {
                return View("Index");
            }

        }
        [HttpPost]
        public ActionResult Login(Login login)

        {
            var userDetail = dbIslem.GetUser(login.UserName);
            if (userDetail == null)
            {
                ViewBag.DuplicateMessage = "Yanlis Kullanıcı adı Veya Şifre";
                return View("Index");
            }

            FormsAuthentication.SetAuthCookie(userDetail.UserName, false);
            Session.Add("userId", userDetail.UserId);
            Session.Add("roleId", userDetail.UserRoleId);

            return RedirectToAction("Index", "Home");




        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}