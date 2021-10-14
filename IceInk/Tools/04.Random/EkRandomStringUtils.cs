#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：RandomStringUtils1
// 创 建 者：IceInk
// 创建时间：2021年06月09日 星期三 14:43
// 文件版本：V1.0.0
//===============================================================
// 功能描述：
//		
//      随机字符串生气器
//----------------------------------------------------------------*/

#endregion

using System;

namespace IceInk
{
    /// <summary>
    /// 随机字符串生成器
    /// </summary>
    public static class EkRandomStringUtils
    {
        /// <summary>
        /// 代表伪随机数生成器，一种产生满足某些随机性统计要求的数字序列的设备。
        /// </summary>
        /// <remarks>
        /// 如果您需要强大的随机数生成器，请使用 RNGCryptoServiceProvider 类。
        /// </remarks>
        private static readonly Random Rand = new Random();

        /// <summary>
        /// 创建一个随机字符串，其长度为指定的字符数。
        ///</summary>
        /// <param name="count">要创建的随机字符串的长度</param>
        /// <returns>随机字符串</returns>
        public static string Random(int count)
        {
            return Random(count, false, false);
        }

        /// <summary>
        /// 创建一个随机字符串，其长度为指定的字符数。字符将从参数指示的字母数字字符集中选择。
        /// </summary>
        /// <param name="count">要创建的随机字符串的长度</param>
        /// <param name="letters">如果为 true，生成的字符串将包含字母字符</param>
        /// <param name="numbers">如果为 true，生成的字符串将包含数字字符 </param>
        /// <returns>随机字符串</returns>
        public static string Random(int count, bool letters, bool numbers)
        {
            return Random(count, 0, 0, letters, numbers);
        }

        /// <summary>
        /// 创建一个随机字符串，其长度为指定的字符数。将从指定的字符集中选择字符。
        /// </summary>
        /// <param name="count">要创建的随机字符串的长度</param>
        /// <param name="chars">包含要使用的字符集的字符串，可能为空</param>
        /// <returns>随机字符串</returns>
        /// <exception cref="ArgumentException">if count is lower than 0</exception>
        public static string Random(int count, char[] chars)
        {
            return Random(count, new string(chars));
        }

        /// <summary>
        /// 创建一个随机字符串，其长度为指定的字符数。字符将从参数指示的字母数字字符集中选择。
        /// </summary>
        /// <param name="count">要创建的随机字符串的长度</param>
        /// <param name="start">字符集中开始的位置</param>
        /// <param name="end">在字符集中结束之前的位置</param>
        /// <param name="letters">如果为 true，生成的字符串将包含字母字符</param>
        /// <param name="numbers">如果为 true，生成的字符串将包含数字字符</param>
        /// <returns>随机字符串</returns>
        public static string Random(int count, int start, int end, bool letters, bool numbers)
        {
            return Random(count, start, end, letters, numbers, null, Rand);
        }

        /// <summary>
        /// 使用提供的随机源根据各种选项创建随机字符串。此方法与 Random(int,int,int,bool,bool,char[],Random) 具有完全相同的语义，但它使用内部静态 Random 实例，而不是使用外部提供的随机源。
        /// </summary>
        /// <param name="count">要创建的随机字符串的长度</param>
        /// <param name="start">字符集中开始的位置</param>
        /// <param name="end">在字符集中结束之前的位置</param>
        /// <param name="letters">如果为 true，生成的字符串将包含字母字符</param>
        /// <param name="numbers">如果为 true，生成的字符串将包含数字字符</param>
        /// <param name="chars">从中选择随机数的字符集。如果为空，那么它将使用所有字符的集合.</param>
        /// <returns>随机字符串</returns>
        public static string Random(int count, int start, int end, bool letters, bool numbers, char[] chars)
        {
            return Random(count, start, end, letters, numbers, chars, Rand);
        }

        /// <summary>
        /// 基于各种选项创建一个随机字符串，使用默认的随机源。
        /// </summary>
        /// <param name="count">要创建的随机字符串的长度</param>
        /// <param name="start">字符集中开始的位置</param>
        /// <param name="end">在字符集中结束之前的位置</param>
        /// <param name="letters">如果为 true，生成的字符串将包含字母字符</param>
        /// <param name="numbers">如果为 true，生成的字符串将包含数字字符</param>
        /// <param name="chars">从中选择随机数的字符集。如果为空，那么它将使用所有字符的集合.</param>
        /// <param name="random">随机性的来源.</param>
        /// <returns>随机字符串</returns>
        public static string Random(int count, int start, int end, bool letters, bool numbers, char[] chars, Random random)
        {
            if (count == 0)
                return String.Empty;
            if (count < 0)
                throw new ArgumentException("请求的随机字符串长度 " + count + " 小于 0.");
            if (start == 0 && end == 0)
            {
                if (!letters && !numbers)
                {
                    end = Int32.MaxValue;
                    start = 0;
                }
                else
                {
                    end = 'z' + 1;
                    start = ' ';
                }
            }

            var buffer = new char[count];
            var gap = end - start;

            while (count-- != 0)
            {
                var ch = chars?[random.Next(gap) + start] ?? (char) (random.Next(gap) + start);
                if ((letters && char.IsLetter(ch)) || (numbers && char.IsDigit(ch)) || (!letters && !numbers))
                {
                    if (ch >= 56320 && ch <= 57343)
                    {
                        if (count == 0) count++;
                        else
                        {
                            // 低代理，放入后插入高代理
                            buffer[count] = ch;
                            count--;
                            buffer[count] = (char) (55296 + random.Next(128));
                        }
                    }
                    else if (ch >= 55296 && ch <= 56191)
                    {
                        if (count == 0) count++;
                        else
                        {
                            // 高代理，在放入之前插入低代理
                            buffer[count] = (char) (56320 + random.Next(128));
                            count--;
                            buffer[count] = ch;
                        }
                    }
                    else if (ch >= 56192 && ch <= 56319)
                        // 私人高代理，没有影响线索，所以跳过它
                        count++;
                    else
                        buffer[count] = ch;
                }
                else
                    count++;
            }

            return new string(buffer);
        }

        /// <summary>
        /// 创建一个随机字符串，其长度为指定的字符数。
        /// </summary>
        /// <param name="count">要创建的随机字符串的长度</param>
        /// <param name="chars"> 包含要使用的字符集的字符串，可能为空 </param>
        /// <returns>随机字符串</returns>
        /// <exception cref="ArgumentException">如果计数小于 0</exception>
        public static string Random(int count, string chars)
        {
            return chars == null
                ? Random(count, 0, 0, false, false, null, Rand)
                : Random(count, chars.ToCharArray());
        }

        /// <summary>
        /// 创建一个随机【字母】字符串，其长度为指定的字符数。
        /// </summary>
        /// <param name="count">要创建的随机字符串的长度</param>
        /// <returns>随机字符串</returns>
        public static string RandomAlphabetic(int count)
        {
            return Random(count, true, false);
        }

        /// <summary>
        /// 创建一个随机【字母数字】字符串，其长度为指定的字符数。字符将从字母数字字符集中选择。
        /// </summary>
        /// <param name="count">要创建的随机字符串的长度</param>
        /// <returns>随机字符串</returns>
        public static string RandomAlphanumeric(int count)
        {
            return Random(count, true, true);
        }

        /// <summary>
        /// 创建一个随机【ASCII】字符串，其长度为指定的字符数。将从 ASCII 值介于 32 和 126（含）之间的字符集中选择字符。
        /// </summary>
        /// <see cref="https://duckduckgo.com/?q=ascii+table&t=ffab"/>
        /// <param name="count">要创建的随机字符串的长度</param>
        /// <returns>随机字符串</returns>
        public static string RandomAscii(int count)
        {
            return Random(count, 32, 127, false, false);
        }

        /// <summary>
        /// 创建一个随机【数字】字符串，其长度为指定的字符数。将从数字字符集中选择字符。
        /// </summary>
        /// <param name="count">要创建的随机字符串的长度</param>
        /// <returns>随机字符串</returns>
        public static string RandomNumeric(int count)
        {
            return Random(count, false, true);
        }
    }
}