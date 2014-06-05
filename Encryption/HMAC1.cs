using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Encryption
{


    public class HMAC1
    {
        public static void Main(string[] args)
        {
            string key = "469F67ED-772D-418A-8CA1-237B7DF104BF";
            string message = "e5ea5966-e137-4a09-8c2c-a5ffe818ef73";

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding(); 
            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
            byte[] messageBytes = encoding.GetBytes(message.ToLower());
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes); 
            string hmac1 = ByteToString(hashmessage);

            Console.WriteLine("key:{0}\nmessage:{1}\nhmac1:{2}", key, message, hmac1);
        }

        public static string ByteToString(byte[] buff)
        {
            StringBuilder sbinary = new StringBuilder();

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary.Append(buff[i].ToString("X2")); // hex format
            }
            return sbinary.ToString();
        }

    }
}