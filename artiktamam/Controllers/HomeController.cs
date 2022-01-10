using artiktamam.Database;
using artiktamam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace artiktamam.Controllers
{

    [Authorize(Roles = "kullanici")]
    public class HomeController : Controller
    {
        private DatabaseIslemleri dbIslem = new DatabaseIslemleri();
        private masterEntities db = new masterEntities();
        public ActionResult Index()
        {
            
            return View(db.ShowCars.ToList());
        }

        public ActionResult SatinAl(int? id)
        {
           
            if (id == null)
            {
                return RedirectToAction("Index","Home");
            }
            ShowCars showCars = db.ShowCars.Find(id);
            if (showCars == null)
            {
                return HttpNotFound();
            }
            return View(showCars);
        }

        // POST: admin/Delete/5
        [HttpPost, ActionName("SatinAl")]
        [ValidateAntiForgeryToken]
        public ActionResult SatinAlConfirmed(int id)
        {      
            ShowCars showCars = db.ShowCars.Find(id);
            dbIslem.addSatinAlmaGecmisine(showCars,User.Identity.Name);
            ViewBag.SatinAlma = "Satin Alma Başarılı , Sayın " + User.Identity.Name + " Güle Güle Kullanın ";
            return View();
        }

        [HttpPost]
        public ActionResult Index(ShowCars cars)
        {

            return View(db.ShowCars.ToList());
        }


    }
}