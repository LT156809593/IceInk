﻿#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EkEncrypt
// 创 建 者：IceInk
// 创建时间：2020/08/09/星期日 16:47:21
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		 MD5加密 不可逆加密
//          1 防止被篡改
//          2 防止明文存储
//          3 防止抵赖，数字签名
//
//----------------------------------------------------------------*/
#endregion

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IceInk
{
    /// <summary>
    /// MD5加密 不可逆加密
    /// 1 防止被篡改
    /// 2 防止明文存储
    /// 3 防止抵赖，数字签名
    /// </summary>
    public class EkMd5Encrypt
    {
        #region MD5
        /// <summary>
        /// MD5加密,和动网上的16/32位MD5加密结果相同,
        /// 使用的UTF8编码
        /// </summary>
        /// <param name="source">待加密字串</param>
        /// <param name="length">16或32值之一,其它则采用.net默认MD5加密算法</param>
        /// <returns>加密后的字串</returns>
        public static string Encrypt(string source, int length = 32)//默认参数
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;
            HashAlgorithm provider = CryptoConfig.CreateFromName("MD5") as HashAlgorithm;
            byte[] bytes = Encoding.UTF8.GetBytes(source);//这里需要区别编码的
            byte[] hashValue = provider.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            switch (length)
            {
                case 16://16位密文是32位密文的9到24位字符
                    for (int i = 4; i < 12; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                case 32:
                    for (int i = 0; i < 16; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                default:
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
            }
            return sb.ToString();
        }
        #endregion MD5

        #region MD5摘要
        /// <summary>
        /// 获取文件的MD5摘要
        /// </summary>
        /// <param name="fullFileName">文件路径</param>
        /// <returns></returns>
        public static string AbstractFile(string fullFileName)
        {
            using (FileStream file = new FileStream(fullFileName, FileMode.Open))
            {
                return AbstractFile(file);
            }
        }
        /// <summary>
        /// 根据stream获取文件摘要
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string AbstractFile(Stream stream)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(stream);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion
    }

   


}
