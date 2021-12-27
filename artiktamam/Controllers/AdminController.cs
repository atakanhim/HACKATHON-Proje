using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using artiktamam.Database;
using artiktamam.Models;

namespace artiktamam.Controllers
{

    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private galeriEntities7 db = new galeriEntities7();
        private DatabaseIslemleri dbIslem = new DatabaseIslemleri();
        // GET: admin
        public ActionResult Index()
        {
            return View(db.Users_Tablo.ToList());
        }

        public ActionResult Satinalinanlar()
        {
            return View(db.SatinalmaGecmisi_Table.ToList());
        }

        // GET: admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Users_Tablo users_Tablo = db.Users_Tablo.Find(id);
            if (users_Tablo == null)
            {
                return HttpNotFound();
            }
            return View(users_Tablo);
        }

        // POST: admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users_Tablo users_Tablo = db.Users_Tablo.Find(id);
            int roleId = dbIslem.getMapRoleId(id);
            UserRoleMapping roleMapping = db.UserRoleMapping.Find(roleId);
            db.Users_Tablo.Remove(users_Tablo);
            db.UserRoleMapping.Remove(roleMapping);
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
