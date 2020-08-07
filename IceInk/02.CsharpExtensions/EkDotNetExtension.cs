/**************************************************************************
*   
*   =================================
*   CLR 版本    ：
*   命名空间    ：IceInk.Extension
*   文件名称    ：EkDotNetExtension.cs
*   =================================
*   创 建 者    ：IceInk
*   创建日期    ：2018/9/10 11:46:04 
*   功能描述    ：
*           一些类型的扩展  
*   使用说明    ：
*   =================================
*  
***************************************************************************/

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace IceInk.Extension
{
    /// <summary>
    /// Int类型扩展
    /// </summary>
    public static class IntExtension
    {
        /// <summary>
        /// 转换为枚举类型
        /// </summary>
        /// <typeparam name="TEnum">目标枚举类型</typeparam>
        /// <param name="number"></param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this int number) where TEnum : struct
        {
            return Enum.TryParse(number.ToString(), out TEnum result) ? result : default;
        }

        /// <summary>
        /// 转换为Byte
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static byte ToByte(this int number)
        {
            return Convert.ToByte(number);
        }

        #region 判断是否 质数 奇数 偶数

        /// <summary>
        /// 是否为奇数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsOdd(this int number)
        {
            return (number & 1) == 1;
        }

        /// <summary>
        /// 是否为偶数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsEven(this int number)
        {
            return (number & 1) != 1;
        }

        /// <summary>
        /// 是否为素数(质数)
        /// </summary>
        /// <param name="number">(非负数)</param>
        /// <returns></returns>
        public static bool IsPrime(this int number)
        {
            if (number <= 3)
            {
                return number > 1;
            }
            // 不在6的倍数两侧的一定不是质数
            if (number % 6 != 1 && number % 6 != 5)
            {
                return false;
            }
            int sqrt = (int)Math.Sqrt(number);
            for (int i = 5; i <= sqrt; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }

    /// <summary>
    /// string类型扩展
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 检查字符串是否为空
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string selfStr)
        {
            return string.IsNullOrEmpty(selfStr);
        }

        /// <summary>
        /// 检查字符串是否不为空
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static bool IsNotNullAndEmpty(this string selfStr)
        {
            return !string.IsNullOrEmpty(selfStr);
        }

        /// <summary>
        /// 检查字符串移除空字符后是否为空
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static bool IsTrimNotNullAndEmpty(this string selfStr)
        {
            return !string.IsNullOrEmpty(selfStr.Trim());
        }

        /// <summary>
        /// 缓存
        /// </summary>
        private static readonly char[] mCachedSplitCharArray = { '.' };

        /// <summary>
        /// 分割
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="splitSymbol"></param>
        /// <returns></returns>
        public static string[] Split(this string selfStr, char splitSymbol)
        {
            mCachedSplitCharArray[0] = splitSymbol;
            return selfStr.Split(mCachedSplitCharArray);
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UppercaseFirst(this string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string LowercaseFirst(this string str)
        {
            return char.ToLower(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnixLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        /// <summary>
        /// 转换成 CSV
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToCsv(this string[] values)
        {
            return string.Join(", ", values
                .Where(value => !string.IsNullOrEmpty(value))
                .Select(value => value.Trim())
                .ToArray()
            );
        }

        public static string[] ArrayFromCsv(this string values)
        {
            return values
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(value => value.Trim())
                .ToArray();
        }

        public static string ToSpacedCamelCase(this string text)
        {
            StringBuilder sb = new StringBuilder(text.Length * 2);
            sb.Append(char.ToUpper(text[0]));
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                {
                    sb.Append(' ');
                }

                sb.Append(text[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 有点不安全,编译器不会帮你排查错误。
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FillFormat(this string selfStr, params object[] args)
        {
            return string.Format(selfStr, args);
        }

        /// <summary>
        /// 添加前缀
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="toAppend"></param>
        /// <returns></returns>
        public static StringBuilder Append(this string selfStr, string toAppend)
        {
            return new StringBuilder(selfStr).Append(toAppend);
        }

        /// <summary>
        /// 添加后缀
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="toPrefix"></param>
        /// <returns></returns>
        public static string AddPrefix(this string selfStr, string toPrefix)
        {
            return new StringBuilder(toPrefix).Append(selfStr).ToString();
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="toAppend"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static StringBuilder AppendFormat(this string selfStr, string toAppend, params object[] args)
        {
            return new StringBuilder(selfStr).AppendFormat(toAppend, args);
        }

        /// <summary>
        /// 最后一个单词
        /// </summary>
        /// <param name="selfUrl"></param>
        /// <returns></returns>
        public static string LastWord(this string selfUrl)
        {
            return selfUrl.Split('/').Last();
        }

        /// <summary>
        /// 解析到 Int 类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this string selfStr, int defaultValue = int.MinValue)
        {
            return int.TryParse(selfStr, out int retValue) ? retValue : defaultValue;
        }

        /// <summary>
        /// 解析到 Float 类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string selfStr, float defaultValue = float.MinValue)
        {
            return float.TryParse(selfStr, out float retValue) ? retValue : defaultValue;
        }
        /// <summary>
        /// 解析到 Double 类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string selfStr, double defaultValue = double.MinValue)
        {
            return double.TryParse(selfStr, out double retValue) ? retValue : defaultValue;
        }


        /// <summary>
        /// 解析到时间类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string selfStr, DateTime defaultValue = default(DateTime))
        {
            return DateTime.TryParse(selfStr, out DateTime retValue) ? retValue : defaultValue;
        }



        /// <summary>
        /// 是否存在中文字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasChinese(this string input)
        {
            return Regex.IsMatch(input, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 是否存在空格
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasSpace(this string input)
        {
            return input.Contains(" ");
        }

        /// <summary>
        /// 删除特定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        public static string RemoveString(this string str, params string[] targets)
        {
            return targets.Aggregate(str, (current, t) => current.Replace(t, string.Empty));
        }



        /// <summary>
        ///字符串相似程度查询(结果供参考)
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="otherStr"></param>
        /// <returns>相似程度</returns>
        public static float EkSimilarity(this string selfStr, string otherStr)
        {
            //计算两个字符串的长度。  
            int len1 = selfStr.Length;
            int len2 = otherStr.Length;
            //建立上面说的数组，比字符长度大一个空间  
            int[,] dif = new int[len1 + 1, len2 + 1];
            //赋初值，步骤B。  
            for (int a = 0; a <= len1; a++)
            {
                dif[a, 0] = a;
            }
            for (int a = 0; a <= len2; a++)
            {
                dif[0, a] = a;
            }
            //计算两个字符是否一样，计算左上的值  
            int temp;
            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    if (selfStr[i - 1] == otherStr[j - 1])
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }
                    //取三个值中最小的  
                    dif[i, j] = Math.Min(Math.Min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1), dif[i - 1, j] + 1);
                }
            }
            //计算相似度  
            float similarity = 1 - (float)dif[len1, len2] / Math.Max(selfStr.Length, otherStr.Length);

            //在Unity 中打印
#if UNITY_EDITOR 
            Debug.Log("字符串\"" + selfStr + "\"与\"" + otherStr + "\"的比较");

            //取数组右下角的值，同样不同位置代表不同字符串的比较  
            Debug.Log("差异步骤：" + dif[len1, len2]);

            Debug.Log("相似度：" + similarity);
#endif
            return similarity;
        }
    }


    /// <summary>
    /// Byte类型扩展
    /// </summary>
    public static class ByteExtension
    {
        public static UInt16 ToUInt16(this byte[] array, int offset = 0)
        {
            return (UInt16)((array[offset] << 8) | array[offset + 1]);
        }
        public static UInt32 ToUInt32(this byte[] array, int offset = 0)
        {
            return (((UInt32)array[offset] << 24)
               | ((UInt32)array[offset + 1] << 16)
               | ((UInt32)array[offset + 2] << 8)
               | array[offset + 3]);
        }
        public static UInt64 ToUInt64(this byte[] array, int offset = 0)
        {
            return (((UInt64)array[offset] << 56)
                 | ((UInt64)array[offset + 1] << 48)
                 | ((UInt64)array[offset + 2] << 40)
                 | ((UInt64)array[offset + 3] << 32)
                 | ((UInt64)array[offset + 4] << 24)
                 | ((UInt64)array[offset + 5] << 16)
                 | ((UInt64)array[offset + 6] << 8)
                 | array[offset + 7]);
        }
    }
    /// <summary>
    /// UInt类型扩展
    /// </summary>
    public static class UIntExtension
    {
        public static byte[] ToBytes(this UInt16 value)
        {
            return new byte[]{
                     (byte)(value >> 8),
                     (byte)value
                };
        }

        public static byte[] ToBytes(this UInt32 value)
        {
            return new byte[] {
                 (byte)(value >> 24),
                 (byte)(value >> 16),
                 (byte)(value >> 8),
                 (byte)value
                 };
        }

        public static byte[] ToBytes(this UInt64 value)
        {
            return new byte[]{
                  (byte)(value >> 56),
                  (byte)(value >> 48),
                  (byte)(value >> 40),
                  (byte)(value >> 32),
                  (byte)(value >> 24),
                  (byte)(value >> 16),
                  (byte)(value >> 8),
                  (byte)value
             };
        }
    }



}