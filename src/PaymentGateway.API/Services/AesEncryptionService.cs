using Microsoft.Extensions.Configuration;
using PaymentGateway.API.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.API.Services
{
    public class AesEncryptionService : IEncryptionService
    {
        private readonly IConfiguration _config;
        private byte[] _key;
        private byte[] _iv;

        public AesEncryptionService(IConfiguration config)
        {
            _config = config;
            _key = Encoding.ASCII.GetBytes(_config["Key"]);
            _iv = Encoding.ASCII.GetBytes(_config["Iv"]);
        }

        public byte[] Encrypt(string str)
        {
            var encryptedBytes = Encrypt(Encoding.UTF8.GetBytes(str), _key, _iv);

            return encryptedBytes;
        }

        public string Decrypt(byte[] encryptedBytes)
        {
            var decryptedBytes = Decrypt(encryptedBytes, _key, _iv);
            var decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedString;
        }

        private byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = key;
                aes.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(),
                        CryptoStreamMode.Write);

                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }

        private byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = key;
                aes.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(),
                        CryptoStreamMode.Write);

                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    var decryptBytes = memoryStream.ToArray();

                    return decryptBytes;
                }
            }
        }
    }
}
