using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace GK.Library.Common
{
    public class Encryption
    {
        private const int SaltSize = 32;
        private static readonly HashAlgorithm Algorithm = SHA1.Create();

        public static string GenerateSaltValue()
        {
            //Generate a cryptographic random number.
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[SaltSize];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        public static string HashPassword(string clearData, string saltValue)
        {
            var bytes = Encoding.Unicode.GetBytes(clearData);
            var src = Encoding.Unicode.GetBytes(saltValue);
            var dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            var inArray = Algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }

        public bool ValidateHash(string clearData, string saltValue, string hash)
        {
            return hash == HashPassword(clearData, saltValue);
        }
    }
}
