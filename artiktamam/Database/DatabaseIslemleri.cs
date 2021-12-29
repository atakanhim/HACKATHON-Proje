using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using artiktamam.BlockChainn;
using artiktamam.Models;

namespace artiktamam.Database
{
    public class DatabaseIslemleri
    {
        private galeriEntities10 db = new galeriEntities10();

        public Users_Tablo GetUser(string username)
        {
            var selectedUser = (from user in db.Users_Tablo
                                where user.UserName == username
                                select user
                                ).FirstOrDefault();
            return selectedUser;
        }
        public void DatabaseVeri0la()
        {
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE BlokZinciri");
        }
        public void DataBaseZincirEkle(List<Block> block)
        {
            BlokZinciri blokZinciri = new BlokZinciri();

            foreach (var b in block)
            {
                foreach (var data in b.Data)
                {
                    blokZinciri.MusteriAdi = data.MusteriAdi;
                    blokZinciri.ArabaninMarkasi = data.ArabaninMarkasi;
                    blokZinciri.ArabaninModeli = data.ArabaninModeli;
                    blokZinciri.SiparisKodu = data.SiparisKodu;
                    blokZinciri.SatinAlmaZamani = data.SatinAlmaZamani;
                    blokZinciri.ArabaninFiyati = data.ArabaninFiyati;


                }
                blokZinciri.Hash = b.Hash;
                blokZinciri.PrevHash = b.PrevHash;
                blokZinciri.TimeStamp = b.TimeStamp;
                blokZinciri.Changed = b.changed;
                db.BlokZinciri.Add(blokZinciri);
                db.SaveChanges();
            }
        }
        public List<Block> karsilastir(List<Block> block)
        {
            BlockChain currentBlock = new BlockChain(new Block()
            {
                Data = new List<SatinalmaGecmisi_Table>(),
                Hash = "00000000000000000000000000000000000000000000000000000000",
                Nonce = 1,
                PrevHash = "00000000000000000000000000000000000000000000000000000",
                TimeStamp = DateTime.UtcNow,
                changed = 1,
                BlockNo = 1
            });
            //   currentBlock.Chain = block;// bize gönderilen block - kullanıcıya sunulacak olan block databasede degişse bile henüz degişmeyen 
            //  databaseBlock.Chain = getZincirFromDatabase();//veri tabanındaki 



            var blocks = this.getBlokeIds();// zincir tablosundaki veriler
            var arabalar = this.getArabaIds();// satin alma tablosundaki veriler
            int zincirin_bozuldugu_yer = 0;


            for (int i = 1; i < arabalar.Length + 1; i++)
            {
                var arabaid = arabalar[i - 1];
                var Araba = this.getArabaWithId(arabaid);//araba bilgilerini sıra ile alıyoruz
                var blokeId = blocks[i];
                var bloke = this.getBlokeWithId(blokeId);// bloke bilgilerini sıra ile alıyoruz
                
                if (
                    (Araba.ArabaninFiyati != bloke.ArabaninFiyati) || (Araba.ArabaninMarkasi != bloke.ArabaninMarkasi) || (Araba.ArabaninModeli != bloke.ArabaninModeli)
                    || (Araba.MusteriAdi != bloke.MusteriAdi) || (Araba.SiparisKodu != bloke.SiparisKodu) || (Araba.SatinAlmaZamani != bloke.SatinAlmaZamani)
                    )
                {
                    zincirin_bozuldugu_yer = i;
                    break;
                }
                else
                {
                    zincirin_bozuldugu_yer = 0;
                }
            }

            if (zincirin_bozuldugu_yer > 0)// ziincirde veri degişirse  buraya geliyor
            {
                int changed;


                for (int i = 0; i < arabalar.Length; i++)
                {
                    
                    var arabaid = arabalar[i];
                    var Araba = this.getArabaWithId(arabaid);//araba bilgilerini sıra ile alıyoruz
                    var blokeId = blocks[i+1];
                    var bloke = this.getBlokeWithId(blokeId);// bloke bilgilerini sıra ile alıyoruz
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
                    if (zincirin_bozuldugu_yer < i + 2)
                    {
                        Block b = new Block()
                        {
                            Data = SatinAlmaGecmisi,
                            TimeStamp = DateTime.UtcNow,
                            changed = 0,
                            BlockNo = i + 1
                        };
                        currentBlock.Mine(b);
                    }
                    else 
                    {
                        Block b = new Block()
                        {
                            Data = SatinAlmaGecmisi,
                            TimeStamp = DateTime.UtcNow,
                            changed = 1,
                            BlockNo = i + 1,
                            Hash=bloke.Hash,
                            PrevHash=bloke.PrevHash
                        };

                        currentBlock.MineNoHash(b);
                    }



                  
                }

                return currentBlock.Chain;
            }
            else
            {
                return block;
            }

        }
        public List<Block> getZincirFromDatabase()
        {
            var blocks = this.getBlokeIds();
            int blockNo = 0;

            //zincir olusturuyoruz
            BlockChain blockChain = new BlockChain(new Block()
            {
                Data = new List<SatinalmaGecmisi_Table>(),
                Hash = "00000000000000000000000000000000000000000000000000000000",
                Nonce = 1,
                PrevHash = "00000000000000000000000000000000000000000000000000000",
                TimeStamp = DateTime.UtcNow,
                changed = 1,
                BlockNo = blockNo
            });

            var zincir = (from blok in db.BlokZinciri //   veri tabanındakiZincir çekiyiyoruz 
                          select blok
                                ).ToList();

            for (int i = 0; i < zincir.Count; i++)
            {
                var blokeId = blocks[i];
                var bloke = this.getBlokeWithId(blokeId);// bloke bilgilerini sıra ile alıyoruz

                List<SatinalmaGecmisi_Table> SatinAlmaGecmisi = new List<SatinalmaGecmisi_Table>();
                SatinAlmaGecmisi.Add(new SatinalmaGecmisi_Table()
                {
                    MusteriAdi = bloke.MusteriAdi,
                    ArabaninMarkasi = bloke.ArabaninMarkasi,
                    ArabaninModeli = bloke.ArabaninModeli,
                    SiparisKodu = bloke.SiparisKodu,
                    SatinAlmaZamani = bloke.SatinAlmaZamani,
                    ArabaninFiyati = bloke.ArabaninFiyati
                });
                Block b = new Block()
                {
                    Data = SatinAlmaGecmisi,
                    TimeStamp = DateTime.UtcNow,
                    changed = 1,
                    BlockNo = blockNo++,
                    Hash = bloke.Hash,
                    PrevHash = bloke.PrevHash
                };
                blockChain.MineNoHash(b);
            }


            return blockChain.Chain;

        }
        public bool ZincirDoluMu()
        {
            var zincir = (from blok in db.BlokZinciri
                          select blok
                                  ).ToList();
            int x = zincir.Count();
            return x > 2 ? true : false;
        }
        public SatinalmaGecmisi_Table getArabaWithId(int id)
        {
            var selectedCar = (from car in db.SatinalmaGecmisi_Table
                               where car.Id == id
                               select car
                                  ).FirstOrDefault();
            return selectedCar;

        }
        public BlokZinciri getBlokeWithId(int id)
        {
            var selectedBlok = (from blok in db.BlokZinciri
                                where blok.BlockId == id
                                select blok
                                  ).FirstOrDefault();
            return selectedBlok;

        }
        public int[] getArabaIds()
        {
            var ids = (
                 from car in db.SatinalmaGecmisi_Table
                 select car.Id).ToList();
            return ids.ToArray();
        }

        public int[] getBlokeIds()
        {
            var ids = (
                 from bloke in db.BlokZinciri
                 select bloke.BlockId).ToList();
            return ids.ToArray();
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
                    okaymi = true;
                }
                else
                {
                    okaymi = false;
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

        public int getUserId(string username)
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