#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：EkRsaEncrypt
// 创 建 者：IceInk
// 创建时间：2020/9/9 16:12:36
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		RSA ECC
//          可逆非对称加密 
//          非对称加密算法的优点是密钥管理很方便，缺点是速度慢。

//		
//----------------------------------------------------------------*/

#endregion

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace IceInk.Tools._03.Encrypt
{
    /// <summary>
    ///     RSA ECC
    ///     可逆非对称加密
    ///     非对称加密算法的优点是密钥管理很方便，缺点是速度慢。
    /// </summary>
    public class EkRsaEncrypt
    {
        /// <summary>
        ///     获取加密/解密对
        ///     给你一个，是无法推算出另外一个的
        ///     Encrypt   Decrypt
        /// </summary>
        /// <returns>Encrypt   Decrypt</returns>
        public static KeyValuePair<string, string> GetKeyPair()
        {
            var rsa = new RSACryptoServiceProvider();
            var publicKey = rsa.ToXmlString(false); //在C#中只实现了 公钥加密  私钥解密  如果有需要其他场景的，需要引入一个第三方dll
            var privateKey = rsa.ToXmlString(true);
            return new KeyValuePair<string, string>(publicKey, privateKey);
        }

        /// <summary>
        ///     加密：内容+加密key
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encryptKey">加密key</param>
        /// <returns></returns>
        public static string Encrypt(string content, string encryptKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(encryptKey);
            var byteConverter = new UnicodeEncoding();
            var dataToEncrypt = byteConverter.GetBytes(content);
            var resultBytes = rsa.Encrypt(dataToEncrypt, false);
            return Convert.ToBase64String(resultBytes);
        }

        /// <summary>
        ///     解密  内容+解密key
        /// </summary>
        /// <param name="content"></param>
        /// <param name="decryptKey">解密key</param>
        /// <returns></returns>
        public static string Decrypt(string content, string decryptKey)
        {
            var dataToDecrypt = Convert.FromBase64String(content);
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(decryptKey);
            var resultBytes = rsa.Decrypt(dataToDecrypt, false);
            var byteConverter = new UnicodeEncoding();
            return byteConverter.GetString(resultBytes);
        }


        /// <summary>
        ///     可以合并在一起的，每次产生一组新的密钥
        /// </summary>
        /// <param name="content"></param>
        /// <param name="publicKey">加密key</param>
        /// <param name="privateKey">解密key</param>
        /// <returns>加密后结果</returns>
        private static string Encrypt(string content, out string publicKey, out string privateKey)
        {
            var rsaProvider = new RSACryptoServiceProvider();
            publicKey = rsaProvider.ToXmlString(false);
            privateKey = rsaProvider.ToXmlString(true);

            var byteConverter = new UnicodeEncoding();
            var dataToEncrypt = byteConverter.GetBytes(content);
            var resultBytes = rsaProvider.Encrypt(dataToEncrypt, false);
            return Convert.ToBase64String(resultBytes);
        }
    }
}