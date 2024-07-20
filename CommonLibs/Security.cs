using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibs
{
    public static class Security
    {
        public static string EncryptPassword(string password, string salt)
        {
            var saltedPassword = string.Format("{0}{1}", salt, password);
            byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
            return Convert.ToBase64String(SHA256.HashData(saltedPasswordAsBytes));
        }
        public static string MD5(this string s)
        {
            StringBuilder builder = new();

            foreach (byte b in System.Security.Cryptography.MD5.HashData(Encoding.UTF8.GetBytes(s)))
                builder.Append(b.ToString("x2").ToLower());

            return builder.ToString();
        }
    }
}