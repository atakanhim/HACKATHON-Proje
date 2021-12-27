using artiktamam.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using artiktamam.Database;

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
            var islem = dbIslem.Register(register);
            if (islem)
            {
                ViewBag.DuplicateMessage = "Bu Username Daha önce alınmıs";
                return View("Index");
            }
            else
            {
                ModelState.Clear();
                ViewBag.SuccessMessage = "Kayıt oldunuz lütfen giriş yapınız";
                return View("Index");
            }        
          
        }
        [HttpPost]
        public ActionResult Login(Login login)

        {
                var userDetail = dbIslem.GetUser(login.UserName);
                if(userDetail == null)
                {
                    ViewBag.DuplicateMessage = "Yanlis Kullanıcı adı Veya Şifre";
                     return View("Index");
                }

                FormsAuthentication.SetAuthCookie(userDetail.UserName, false);


                Session.Add("userId", userDetail.UserId);
                Session.Add("roleId", userDetail.UserRoleId);
                //  Session["userId"] = userDetail.UserId;
                return RedirectToAction("Index","Home");
           
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}