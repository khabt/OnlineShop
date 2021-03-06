﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OnlineShop.Common
{
    public static class Encryptor
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static string Encrypt(string data)
        {
            using (var des = new TripleDESCryptoServiceProvider { Mode = CipherMode.ECB, Key = GetKey("a1!B78s!5(j$S1c%"), Padding = PaddingMode.PKCS7 })
            using (var desEncrypt = des.CreateEncryptor())
            {
                var buffer = Encoding.UTF8.GetBytes(data);

                return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }                

        }

        public static string Decrypt(string data)
        {
            using (var des = new TripleDESCryptoServiceProvider { Mode = CipherMode.ECB, Key = GetKey("a1!B78s!5(j$S1c%"), Padding = PaddingMode.PKCS7 })
            using (var desEncrypt = des.CreateDecryptor())
            {
                var buffer = Convert.FromBase64String(data.Replace(" ", "+"));

                return Encoding.UTF8.GetString(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
        }

        private static byte[] GetKey(string password)
        {
            string pwd = null;
            if (Encoding.UTF8.GetByteCount(password) < 24)
                pwd = password.PadRight(24, ' ');
            else pwd = password.Substring(0, 24);

            return Encoding.UTF8.GetBytes(pwd);
        }
        
    }
}