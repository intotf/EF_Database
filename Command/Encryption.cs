using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Command
{
    public static class Encryption
    {
        /// <summary>
        /// RgbKey
        /// </summary>
        private static byte[] RgbKey = { 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01 };
        /// <summary>
        /// RgbIV
        /// </summary>
        private static byte[] RgbIV = { 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01 };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns></returns>
        public static string DESEncrypt(this string encryptString)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(encryptString.NullThenEmpty());
                using (var des = new DESCryptoServiceProvider())
                {
                    using (var stream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(stream, des.CreateEncryptor(RgbKey, RgbIV), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(bytes, 0, bytes.Length);
                            cryptoStream.FlushFinalBlock();
                            return Convert.ToBase64String(stream.ToArray());
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>    
        /// <returns></returns>
        public static string DESDecrypt(this string decryptString)
        {
            try
            {
                var bytes = Convert.FromBase64String(decryptString);
                using (var des = new DESCryptoServiceProvider())
                {
                    using (var stream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(stream, des.CreateDecryptor(RgbKey, RgbIV), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(bytes, 0, bytes.Length);
                            cryptoStream.FlushFinalBlock();
                            return Encoding.UTF8.GetString(stream.ToArray());
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取Md5
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns></returns>
        public static string GetMD5(string source)
        {
            //return source.GetHashCode().ToString();
            var data = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(source)).Select(item => item.ToString("X2")).ToArray();
            return string.Join(string.Empty, data);
        }

        /// <summary>       
        /// 密码MD5后反转再MD5
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns></returns>
        public static string GetReverseMd5(string source)
        {
            var reverseMd5 = Encryption.GetMD5(source).Reverse().ToArray();
            return Encryption.GetMD5(new string(reverseMd5));
        }
    }
}
