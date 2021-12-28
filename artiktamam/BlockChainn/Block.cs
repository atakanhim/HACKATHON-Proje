﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using artiktamam.Models;

namespace artiktamam.BlockChainn
{
    public class Block
    {
        /// <summary>
        /// Proof of Work için gerekli Nonce
        /// </summary>
        public int Nonce { get; set; }
        /// <summary>
        /// Transaction'ların tutulduğu değişken
        /// </summary>
        public List<SatinalmaGecmisi_Table> Data { get; set; }
        /// <summary>
        /// önceki blogun hash değeri
        /// </summary>
        public string PrevHash { get; set; }
        /// <summary>
        /// Blog hash değeri
        /// </summary>
        public string Hash { get; set; }

        public bool changed { get; set; }

        /// <summary>
        /// Blok oluşturulduğu tarih
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// SHA256 hash'ini oluştururken kullanacağımız string değerini döndürdük.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Nonce + ":" + PrevHash + ":" + TimeStamp.ToString() + GetDataString();

        }
        /// <summary>
        /// Transaction'ları string'e dönüştüren metod
        /// </summary>
        /// <returns></returns>
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