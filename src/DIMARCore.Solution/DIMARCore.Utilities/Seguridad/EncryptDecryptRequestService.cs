using DIMARCore.Utilities.Helpers;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DIMARCore.Utilities.Seguridad
{
    public class EncryptDecryptRequestService
    {
        private readonly string _encryptKeyRequest;
        private readonly string _encryptIVRequest;

        public EncryptDecryptRequestService()
        {
            _encryptKeyRequest = ConfigurationManager.AppSettings[Constantes.KEY_ENCRYPTION_REQUEST];
            _encryptIVRequest = ConfigurationManager.AppSettings[Constantes.IV_ENCRYPTION_REQUEST];
        }

        public EncryptDecryptRequestService(string encriptKeyRequest, string encryptIVRequest)
        {
            _encryptKeyRequest = encriptKeyRequest;
            _encryptIVRequest = encryptIVRequest;
        }

        public byte[] EncryptReturnBytes(string plainText)
        {
            byte[] encrypted;
            using (Aes aes = GetEncryptionAlgorithm())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encrypted = ms.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public string EncryptReturnString(string plainText)
        {
            byte[] encrypted;
            using (Aes aes = GetEncryptionAlgorithm())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encrypted = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public string DecryptBytes(byte[] cipherBytes)
        {
            using (Aes aes = GetEncryptionAlgorithm())
            {
                using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
                {
                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }


        public string DecryptString(string cipherText)
        {
            using (Aes aes = GetEncryptionAlgorithm())
            {
                byte[] buffer = Convert.FromBase64String(cipherText);
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        private Aes GetEncryptionAlgorithm()
        {
            Aes aes = Aes.Create();
            var secret_key = Encoding.UTF8.GetBytes(_encryptKeyRequest);
            var initialization_vector = Encoding.UTF8.GetBytes(_encryptIVRequest);
            aes.Key = secret_key;
            aes.IV = initialization_vector;
            return aes;
        }
    }
}
