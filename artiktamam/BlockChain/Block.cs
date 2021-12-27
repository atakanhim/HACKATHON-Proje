using artiktamam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace artiktamam.BlockChain
{
    public class Block
    {
        public int Nonce { get; set; }
        public List<SatinalmaGecmisi> Data { get; set; }
        public string PrevHash { get; set; }
        public string Hash { get; set; }
        public DateTime TimeStamp { get; set; }
        // sha256 hasini olustutukrnen kullancagımız string degerini dondurduk
        public override string ToString()
        {
            return Nonce + ":" + PrevHash + ":" + TimeStamp.ToString() + GetDataString();
        }

        // transactionları stringe donusturen method
        private string GetDataString()
        {
            string res = "";
            foreach (var d in Data)
            {
                res += d.ToString() + ":";
            }
            return res;
        }
    }
}