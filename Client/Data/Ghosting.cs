using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Client.Data
{
   public static class Ghosting
    {

        /// <summary>
        /// Ghoster bizim encrypterımız
        /// DeGhoster ise decrypterımız
        /// </summary>
        /// <param name="veri"></param>
        /// <returns></returns>
        public static byte[] Byte8(string veri)
        {
            char[] ArrayChar = veri.ToCharArray();
            byte[] ArrayByte = new byte[ArrayChar.Length];
            for (int i = 0; i < ArrayByte.Length; i++)
            {
                ArrayByte[i] = Convert.ToByte(ArrayChar[i]);
            }

            return ArrayByte;
        }


        public static string Ghoster (this string s)
        {
            byte[] aryKey = Byte8("12345678901234567891234567891234"); // BURAYA 8 bit string DEĞER GİRİN
            byte[] aryIV = Byte8("5152535491235468"); // BURAYA 8 bit string DEĞER GİRİN

            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            encryptor.Key = aryKey;
            encryptor.IV = aryIV;


            MemoryStream memoryStream = new MemoryStream();


            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            byte[] plainBytes = Encoding.ASCII.GetBytes(s);

            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            return cipherText;
        }


        public static string DeGhoster(this string s)
        {
            byte[] aryKey = Byte8("12345678901234567891234567891234"); // BURAYA 8 bit string DEĞER GİRİN
            byte[] aryIV = Byte8("5152535491235468"); // BURAYA 8 bit string DEĞER GİRİN


            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            encryptor.Key = aryKey;
            encryptor.IV = aryIV;


            MemoryStream memoryStream = new MemoryStream();

            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            string plainText = String.Empty;

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(s);

                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                cryptoStream.FlushFinalBlock();

                byte[] plainBytes = memoryStream.ToArray();

                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }


            return plainText;
        }
    }
}
