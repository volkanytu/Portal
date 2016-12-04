using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace GK.Library.Utility
{
    public static class Encryption
    {
        public static string SHA1Hash(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);

            using (SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider())
            {
                string hashText = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

                return hashText;
            }
        }

        public static string GetHash<T>(this object instance) where T : HashAlgorithm, new()
        {
            using (T cryptoServiceProvider = new T())
            {
                return ComputeHash(instance, cryptoServiceProvider);
            }
        }

        public static bool CompareObjectsEquality(params object[] objects)
        {
            bool returnValue = true;
            string initalHash = string.Empty;

            foreach (var item in objects)
            {
                if (string.IsNullOrWhiteSpace(initalHash))
                {
                    initalHash = item.GetHash<SHA1CryptoServiceProvider>();

                    continue;
                }

                string hash = item.GetHash<SHA1CryptoServiceProvider>();
                returnValue = initalHash == hash;

                if (!returnValue)
                {
                    break;
                }
            }

            return returnValue;
        }

        private static string ComputeHash<T>(object instance, T cryptoServiceProvider) where T : HashAlgorithm, new()
        {
            DataContractSerializer serializer = new DataContractSerializer(instance.GetType());

            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, instance);
                cryptoServiceProvider.ComputeHash(memoryStream.ToArray());
                return BitConverter.ToString(cryptoServiceProvider.Hash).Replace("-", "");
            }
        }
    }
}
