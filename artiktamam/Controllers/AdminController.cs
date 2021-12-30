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
using artiktamam.BlockChainn;

namespace artiktamam.Controllers
{

    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private hackathonProjeEntities db = new hackathonProjeEntities();
        private DatabaseIslemleri dbIslem = new DatabaseIslemleri();
        SatinalinanlisteleViewModel model = new SatinalinanlisteleViewModel();
        // GET: admin

        public ActionResult Index()         
        {
            return View(db.Users_Tablo.ToList());
        }

        public  ActionResult Satinalinanlar()
        {
            var arabalar = dbIslem.getArabaIds();
            int blockNo = 0;


            BlockChain blockChain = new BlockChain(new Block()
            {
                Data = new List<SatinalmaGecmisi_Table>(),
                Hash = "00000000000000000000000000000000000000000000000000000000",
                Nonce = 1,
                PrevHash = "00000000000000000000000000000000000000000000000000000",
                TimeStamp = DateTime.UtcNow,
                changed = 1,
                BlockNo=blockNo++
            });
            if (dbIslem.ZincirDoluMu())
            {
                blockChain.Chain = dbIslem.getZincirFromDatabase();
            }
            else
            {
                for (int i = 0; i < db.SatinalmaGecmisi_Table.Count(); i++)
                {
                    var arabaid = arabalar[i];
                    var Araba = dbIslem.GetArabaWithId(arabaid);//araba bilgilerini sıra ile alıyoruz
                    List<SatinalmaGecmisi_Table> SatinAlmaGecmisi = new List<SatinalmaGecmisi_Table>();
                    SatinAlmaGecmisi.Add(new SatinalmaGecmisi_Table()
                    {
                        MusteriAdi = Araba.MusteriAdi,
                        ArabaninMarkasi = Araba.ArabaninMarkasi,
                        ArabaninModeli = Araba.ArabaninModeli,
                        SiparisKodu = Araba.SiparisKodu,
                        SatinAlmaZamani = Araba.SatinAlmaZamani,
                        ArabaninFiyati = Araba.ArabaninFiyati
                    });
                    Block b = new Block()
                    {
                        Data = SatinAlmaGecmisi,
                        TimeStamp = DateTime.UtcNow,
                        changed = 1,
                        BlockNo = blockNo++

                    };
                    blockChain.Mine(b);
                }
                dbIslem.DataBaseZincirEkle(blockChain.Chain);// database ekliyoruz 
            }
            // siradki islem zincir verileri ile , databasedeki verileri karsilastirmak

            // zincir doluysa dolu verileri alacam
            // zincir boşsa zincir oluşturup o verileri alacam
            // kullanıcıya göstermek için aldıgım verileri tam kullanıcıya gösterecekken kontrol ediyorum bu kontrol  satinalma tablosundaki verilerle olmalı

           blockChain.Chain =  dbIslem.karsilastir(blockChain.Chain);
            model.block = blockChain.Chain;
                return View(model);
         
                
        }
        public ActionResult Yenile()
        {

            dbIslem.DatabaseVeri0la("BlokZinciri");
            return RedirectToAction("Satinalinanlar", "Admin");
        }
        public ActionResult GecmisSiparisSil()
        {

            dbIslem.DatabaseVeri0la("SatinalmaGecmisi_Table");
            dbIslem.DatabaseVeri0la("BlokZinciri");
            return RedirectToAction("Satinalinanlar", "Admin");
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
