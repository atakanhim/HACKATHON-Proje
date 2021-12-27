using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace artiktamam.BlockChain
{
    public class SHA256Manager
    {
        public string GetHash(string data)
        {
            var sha256 = new SHA256Managed ();
            var hasBuilder =new StringBuilder();

            var dataBytes =Encoding.Unicode.GetBytes(data);
            var hash = sha256.ComputeHash(dataBytes);

            foreach (var b in hash)
            {
                hasBuilder.Append($"{b:x2}");
            }
            return hasBuilder.ToString();
        }
    }
}