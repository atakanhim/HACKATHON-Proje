using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace artiktamam.BlockChain
{
    public class Transaction
    {
        public string Sender { get; set; }

        public string Receiver { get; set; }
        public float Value { get; set; }
        public override string ToString()
        {
            return Sender + ":" + Receiver + ":" + Value ;
        }
    }
}