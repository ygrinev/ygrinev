using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class HashProvider
    {
        public static string GetHash<T>(object instance) where T : HashAlgorithm, new()
        {
            var cryptoServiceProvider = new T();
            return ComputeHash(instance, cryptoServiceProvider);
        }
        public static string GetMD5Hash(object instance)
        {
            return GetHash<MD5CryptoServiceProvider>(instance);
        }

        private static string ComputeHash<T>(object instance, T cryptoServiceProvider) where T : HashAlgorithm, new()
        {
            var serializer = new DataContractSerializer(instance.GetType());
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, instance);
                cryptoServiceProvider.ComputeHash(memoryStream.ToArray());
                return Convert.ToBase64String(cryptoServiceProvider.Hash);
            }
        }
    }
}
