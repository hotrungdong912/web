using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace G5_Website_giay.Models
{
    public class endCode
    {
        public static string enCodeMD5(string key)
        {
            MD5 md5 = MD5.Create();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            StringBuilder sbHash = new StringBuilder();
            foreach(byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }
    }
}