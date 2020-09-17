#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：EkDesEncrypt
// 创 建 者：IceInk
// 创建时间：2020/9/9 16:10:05
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		DES AES Blowfish
//          对称加密算法的优点是速度快，
//          缺点是密钥管理不方便，要求共享密钥。
//          可逆对称加密  密钥长度8

//		
//----------------------------------------------------------------*/

#endregion

using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IceInk.Tools._03.Encrypt
{
    /// <summary>
    ///  DES AES Blowfish
    ///  对称加密算法的优点是速度快，
    ///  缺点是密钥管理不方便，要求共享密钥。
    ///  可逆对称加密  密钥长度8
    /// </summary>
    public class EkDesEncrypt
    {
        private static readonly byte[] _rgbKey = Encoding.ASCII.GetBytes(Constant.DesKey.Substring(0, 8));
        private static readonly byte[] _rgbIV = Encoding.ASCII.GetBytes(Constant.DesKey.Insert(0, "w").Substring(0, 8));

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="text">需要加密的值</param>
        /// <returns>加密后的结果</returns>
        public static string Encrypt(string text)
        {
            var dsp = new DESCryptoServiceProvider();
            using (var memStream = new MemoryStream())
            {
                var crypStream = new CryptoStream(memStream, dsp.CreateEncryptor(_rgbKey, _rgbIV),
                    CryptoStreamMode.Write);
                var sWriter = new StreamWriter(crypStream);
                sWriter.Write(text);
                sWriter.Flush();
                crypStream.FlushFinalBlock();
                memStream.Flush();
                return Convert.ToBase64String(memStream.GetBuffer(), 0, (int) memStream.Length);
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns>解密后的结果</returns>
        public static string Decrypt(string encryptText)
        {
            var dsp = new DESCryptoServiceProvider();
            var buffer = Convert.FromBase64String(encryptText);

            using (var memStream = new MemoryStream())
            {
                var crypStream = new CryptoStream(memStream, dsp.CreateDecryptor(_rgbKey, _rgbIV),
                    CryptoStreamMode.Write);
                crypStream.Write(buffer, 0, buffer.Length);
                crypStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(memStream.ToArray());
            }
        }

        public static class Constant
        {
            public static string DesKey = AppSettings("DesKey", "IceInk");


            private static T AppSettings<T>(string key, T defaultValue)
            {
                var v = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(v) ? defaultValue : (T) Convert.ChangeType(v, typeof(T));
            }
        }
    }
}