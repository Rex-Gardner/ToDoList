using System;
using System.Security.Cryptography;
using System.Text;

namespace API.Auth
{
    public static class PasswordEncoder
    {
        public static string Encode(string password)
        {
            using (var md5 = MD5.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = md5.ComputeHash(passwordBytes);
                var hash = BitConverter.ToString(hashBytes);
                return hash;
            }
        }
    }
}