using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace artiktamam.Models
{
    public class SatinalmaGecmisi
    {
        public string MusteriAdi { get; set; }
        public string ArabaninMarkasi { get; set; }
        public string ArabaninModeli { get; set; }
        public int ArabaninFiyati { get; set; }
        public int SiparisKod { get; set; }
        public DateTime SatinalmaZamani { get; set; }
    }
}