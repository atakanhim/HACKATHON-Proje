using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using artiktamam.Models;
using artiktamam.Database;
using System.Web.Security;

namespace artiktamam.Controllers
{
    [Authorize(Roles = "kullanici")]
    public class ProfileController : Controller
    {
        private projeDataEntities1 db = new projeDataEntities1();
        private DatabaseIslemleri dbIslem = new DatabaseIslemleri();

        // GET: Users_Tablo/Edit/5

        public ActionResult Edit()
        {

            int id = 0;
            Users_Tablo users_Tablo = new Users_Tablo();
            id = dbIslem.getUserId(User.Identity.Name);
            if (id != 0)
            {
                users_Tablo = db.Users_Tablo.Find(id);
            }
            else
            {
                RedirectToAction("Index", "Home");
            }

            return View(users_Tablo);
        }

        // POST: Users_Tablo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,UserPassword,UserRoleId,UserEmail,UserAdres")] Users_Tablo users_Tablo)
        {
           
            if (ModelState.IsValid)
            {
                string islem = dbIslem.Edit(users_Tablo);
                if (islem== "Bu Email Daha Önce kayıtlı")
                {
                    ViewBag.EditAlertMessage = islem;

                    return View(users_Tablo);
                }
                else if (islem == "Bu Username Daha Önce kayıtlı")
                {
                    ViewBag.EditAlertMessage = islem;

                    return View(users_Tablo);
                }
                else if(islem== "Edit Islemi Basarili")
                {
                    int x = 2;
                    string[] dizi = dbIslem.getRole(users_Tablo.UserId);
                    for (int i = 0; i < dizi.Length; i++)
                    {
                        if (dizi[i] == "admin")
                            x = 1;
                    }

                    users_Tablo.UserRoleId = x;
                    db.Entry(users_Tablo).State = EntityState.Modified;
                    db.SaveChanges();

                    ModelState.Clear();
                    ViewBag.EditSuccessMessage = islem;

                    return View(users_Tablo);
                }          
            }
            return View(users_Tablo);
        }



    }
}
