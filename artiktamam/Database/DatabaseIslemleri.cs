using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using artiktamam.Models;

namespace artiktamam.Database
{
    public class DatabaseIslemleri
    {
        private galeriEntities7 db = new galeriEntities7();
        public Users_Tablo GetUser(string username)
        {
            var selectedUser = (from user in db.Users_Tablo
                                where user.UserName == username
                                select user
                                ).FirstOrDefault();
            return selectedUser;
        }

        public Boolean Register(Register register)//register objesinden aldıgımız degeri users a ekliyoruz
        {
            Users_Tablo usertablo = new Users_Tablo()
            {
                UserName = register.UserName,
                UserPassword = register.PassWord,
                UserRoleId = 2
            };

            if (db.Users_Tablo.Any(x => x.UserName == register.UserName))
            {
                return true;
            }
            else
            {
                db.Users_Tablo.Add(usertablo);
                db.SaveChanges();

                var registerId = getUserId(register.UserName);
                addRole(registerId);
                return false;
            }
        }
        public Boolean addSatinAlmaGecmisine(ShowCars car, string username)
        {
            Random rastgele = new Random();
            bool okaymi = true;
            int siparisKod = 0;
            while (okaymi)
            {
                siparisKod = rastgele.Next(0, 100000);
                if (db.SatinalmaGecmisi_Table.Any(x => x.SiparisKodu == siparisKod))
                {
                    okaymi = false; ;
                }
            }
            SatinalmaGecmisi_Table satinalma = new SatinalmaGecmisi_Table()
            {
                MusteriAdi = username,
                ArabaninMarkasi = car.MarkaName,
                ArabaninModeli = car.ModelName,
                ArabaninFiyati = Convert.ToInt32(car.CarFiyat),
                SiparisKodu = siparisKod,
                SatinAlmaZamani = DateTime.Now
            };
            
            db.SatinalmaGecmisi_Table.Add(satinalma);
            db.SaveChanges();

            return true;
        }
        public string[] getRole(int userid)
        {

            var userRoles = (from user in db.Users_Tablo
                             join roleMap in db.UserRoleMapping
                                on user.UserId equals roleMap.UserId
                             join role in db.Roles_Tablo
                               on roleMap.RoleId equals role.RoleId
                             where user.UserId == userid
                             select role.RoleName
                                      ).ToArray();
            return userRoles;

        }
        public int getMapRoleId(int id)
        {
            var mapId = (from user in db.UserRoleMapping
                         where user.UserId == id
                         select user.Id
                                      ).FirstOrDefault();
            return mapId;

        }

        private int getUserId(string username)
        {
            {
                var userid = (from user in db.Users_Tablo
                              where user.UserName == username
                              select user.UserId
                                    ).FirstOrDefault();
                return userid;
            }
        }
        private void addRole(int userid)//role ekliyoruz
        {
            UserRoleMapping role = new UserRoleMapping()
            {
                UserId = userid,
                RoleId = 2
            };
            db.UserRoleMapping.Add(role);
            db.SaveChanges();
        }
    }
}