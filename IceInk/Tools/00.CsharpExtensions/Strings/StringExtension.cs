#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：StringExtension
// 创 建 者：IceInk
// 创建时间：2020/8/17 11:10:17
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		string类型扩展
//
//----------------------------------------------------------------*/

#endregion

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace IceInk
{
    /// <summary>
    /// string类型扩展
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private static readonly char[] mCachedSplitCharArray = {'.'};

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
        /// 判断字符串compare 在 input字符串中出现的次数
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="compare">用于比较的字符串</param>
        /// <returns>字符串compare 在 input字符串中出现的次数</returns>
        private static int GetStringCount(this string input, string compare)
        {
            var index = input.IndexOf(compare, StringComparison.Ordinal);
            if (index != -1)
                return 1 + GetStringCount(input.Substring(index + compare.Length), compare);
            return 0;
        }


        /// <summary>
        /// 生成唯一短字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chars">可用字符数数量，0-9,a-z,A-Z</param>
        /// <returns></returns>
        public static string CreateShortToken(this string str, byte chars = 36)
        {
            var nf = new NumberFormater(chars);
            return nf.ToString((DateTime.Now.Ticks - 630822816000000000) * 100 + Stopwatch.GetTimestamp() % 100);
        }

        /// <summary>
        /// 将"\r\n  \r" 转换为Unix换行符 "\n"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceUnixLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        /// <summary>
        /// 根据正则替换
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex">正则表达式</param>
        /// <param name="replacement">新内容</param>
        /// <returns></returns>
        public static string Replace(this string input, Regex regex, string replacement)
        {
            return regex.Replace(input, replacement);
        }

        /// <summary>
        /// 字符串掩码
        /// <code>
        /// 例如：12345678910 转换成 123****8910
        /// </code>
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="mask">掩码符</param>
        /// <returns></returns>
        public static string Mask(this string s, char mask = '*')
        {
            if (string.IsNullOrWhiteSpace(s?.Trim()))
                return s;
            s = s.Trim();
            var masks = mask.ToString().PadLeft(4, mask);
            return s.Length switch
            {
                _ when s.Length >= 11 => Regex.Replace(s, @"(\w{3})\w*(\w{4})", $"$1{masks}$2"),
                _ when s.Length == 10 => Regex.Replace(s, @"(\w{3})\w*(\w{3})", $"$1{masks}$2"),
                _ when s.Length == 9 => Regex.Replace(s, @"(\w{2})\w*(\w{3})", $"$1{masks}$2"),
                _ when s.Length == 8 => Regex.Replace(s, @"(\w{2})\w*(\w{2})", $"$1{masks}$2"),
                _ when s.Length == 7 => Regex.Replace(s, @"(\w{1})\w*(\w{2})", $"$1{masks}$2"),
                _ when s.Length >= 2 && s.Length < 7 => Regex.Replace(s, @"(\w{1})\w*(\w{1})", $"$1{masks}$2"),
                _ => s + masks
            };
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


        #region 两个字符串相似程度查询

        /// <summary>
        /// 字符串相似程度查询(结果供参考)
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="otherStr"></param>
        /// <returns>相似程度</returns>
        public static float EkSimilarity(this string selfStr, string otherStr)
        {
            //计算两个字符串的长度。  
            var len1 = selfStr.Length;
            var len2 = otherStr.Length;
            //建立上面说的数组，比字符长度大一个空间  
            var dif = new int[len1 + 1, len2 + 1];
            //赋初值，步骤B。  
            for (var a = 0; a <= len1; a++) dif[a, 0] = a;

            for (var a = 0; a <= len2; a++) dif[0, a] = a;

            //计算两个字符是否一样，计算左上的值  
            for (var i = 1; i <= len1; i++)
            for (var j = 1; j <= len2; j++)
            {
                var temp = selfStr[i - 1] == otherStr[j - 1] ? 0 : 1;

                //取三个值中最小的  
                dif[i, j] = Math.Min(Math.Min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1), dif[i - 1, j] + 1);
            }

            //计算相似度  
            var similarity = 1 - (float) dif[len1, len2] / Math.Max(selfStr.Length, otherStr.Length);

            //在Unity 中打印
#if UNITY_EDITOR
            Debug.Log("字符串\"" + selfStr + "\"与\"" + otherStr + "\"的比较");

            //取数组右下角的值，同样不同位置代表不同字符串的比较  
            Debug.Log("差异步骤：" + dif[len1, len2]);

            Debug.Log("相似度：" + similarity);
#endif
            return similarity;
        }

        #endregion

        #region 转换到其他类型

        /// <summary>
        /// 解析到 Int 类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static int ToInt(this string selfStr)
        {
            return int.TryParse(selfStr, out var retValue)
                ? retValue
                : throw new Exception("该字符串不能转换为int类型");
        }

        /// <summary>
        /// 解析到 Float 类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static float ToFloat(this string selfStr)
        {
            return float.TryParse(selfStr, out var retValue)
                ? retValue
                : throw new Exception("该字符串不能转换为float类型");
        }

        /// <summary>
        /// 解析到 Double 类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static double ToDouble(this string selfStr)
        {
            return double.TryParse(selfStr, out var retValue)
                ? retValue
                : throw new Exception("该字符串不能转换为double类型");
        }


        /// <summary>
        /// 解析到时间类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string selfStr)
        {
            return DateTime.TryParse(selfStr, out var retValue)
                ? retValue
                : throw new Exception("该字符串不能转换为DateTime类型");
        }

        /// <summary>
        /// 字符串转Guid
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string s)
        {
            return Guid.TryParse(s, out Guid retValue)
                ? retValue
                : throw new Exception("该字符串不能转换为Guid类型");
        }

        /// <summary>
        /// 任意进制转十进制
        /// </summary>
        /// <param name="str"></param>
        /// <param name="bin">进制</param>
        /// <returns></returns>
        public static long ToBinary(this string str, int bin)
        {
            var nf = new NumberFormater(bin);
            return nf.FromString(str);
        }

        /// <summary>
        /// 任意进制转大数十进制
        /// </summary>
        /// <param name="str"></param>
        /// <param name="bin">进制</param>
        /// <returns></returns>
        public static BigInteger ToBinaryBig(this string str, int bin)
        {
            var nf = new NumberFormater(bin);
            return nf.FromStringBig(str);
        }

        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string @this)
        {
            return Activator.CreateInstance<ASCIIEncoding>().GetBytes(@this);
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

        /// <summary>
        /// 转换为CSV数组
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string[] ToArrayFromCsv(this string values)
        {
            return values
                .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(value => value.Trim())
                .ToArray();
        }

        public static string ToSpacedCamelCase(this string text)
        {
            var sb = new StringBuilder(text.Length * 2);
            sb.Append(char.ToUpper(text[0]));
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ') sb.Append(' ');

                sb.Append(text[i]);
            }

            return sb.ToString();
        }

        #endregion


        #region 权威校验身份证号码

        private const string cardAddress =
            "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

        /// <summary>
        /// 根据GB11643-1999标准权威校验中国身份证号码的合法性
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns>是否匹配成功</returns>
        public static bool IsIdentifyCard(this string s)
        {
            if (s.Length == 18)
            {
                if (long.TryParse(s.Remove(17), out var n) == false || n < Math.Pow(10, 16) ||
                    long.TryParse(s.Replace('x', '0').Replace('X', '0'), out n) == false) return false; //数字验证

                if (cardAddress.IndexOf(s.Remove(2), StringComparison.Ordinal) == -1) return false; //省份验证

                var birth = s.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                if (!DateTime.TryParse(birth, out _)) return false; //生日验证

                var arrVarifyCode = "1,0,x,9,8,7,6,5,4,3,2".Split(',');
                var wi = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(',');
                var ai = s.Remove(17).ToCharArray();
                var sum = 0;
                for (var i = 0; i < 17; i++) sum += wi[i].ToInt() * ai[i].ToString().ToInt();

                Math.DivRem(sum, 11, out var y);
                return arrVarifyCode[y] == s.Substring(17, 1).ToLower();
            }

            if (s.Length == 15)
            {
                if (long.TryParse(s, out var n) == false || n < Math.Pow(10, 14)) return false; //数字验证

                if (cardAddress.IndexOf(s.Remove(2), StringComparison.Ordinal) == -1) return false; //省份验证

                var birth = s.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                return DateTime.TryParse(birth, out _);
            }

            return false;
        }

        #endregion


        #region IP地址

        /// <summary>
        /// 校验IP地址的正确性，同时支持IPv4和IPv6
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <param name="isMatch">是否匹配成功，若返回true，则会得到一个Match对象，否则为null</param>
        /// <returns>匹配对象</returns>
        public static IPAddress IsIpAddress(this string s, out bool isMatch)
        {
            isMatch = IPAddress.TryParse(s, out var ip);
            return ip;
        }

        /// <summary>
        /// 校验IP地址的正确性，同时支持IPv4和IPv6
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns>是否匹配成功</returns>
        public static bool IsIpAddress(this string s)
        {
            IsIpAddress(s, out var success);
            return success;
        }

        /// <summary>
        /// IP地址转换成数字
        /// </summary>
        /// <param name="addr">IP地址</param>
        /// <returns>数字,输入无效IP地址返回0</returns>
        public static uint IPToID(this string addr)
        {
            if (!IPAddress.TryParse(addr, out var ip)) return 0;

            var bInt = ip.GetAddressBytes();
            if (BitConverter.IsLittleEndian) Array.Reverse(bInt);

            return BitConverter.ToUInt32(bInt, 0);
        }

        /// <summary>
        /// 判断IP地址在不在某个IP地址段
        /// </summary>
        /// <param name="input">需要判断的IP地址</param>
        /// <param name="begin">起始地址</param>
        /// <param name="ends">结束地址</param>
        /// <returns></returns>
        public static bool IpAddressInRange(this string input, string begin, string ends)
        {
            var current = input.IPToID();
            return current >= begin.IPToID() && current <= ends.IPToID();
        }

        #endregion IP地址

        #region 校验电话号码的正确性

        /// <summary>
        /// 匹配手机号码
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <param name="isMatch">是否匹配成功，若返回true，则会得到一个Match对象，否则为null</param>
        /// <returns>匹配对象</returns>
        public static Match IsPhoneNumber(this string s, out bool isMatch)
        {
            if (string.IsNullOrEmpty(s))
            {
                isMatch = false;
                return null;
            }

            var match = Regex.Match(s, @"^((1[3,5,6,8][0-9])|(14[5,7])|(17[0,1,3,6,7,8])|(19[8,9]))\d{8}$");
            isMatch = match.Success;
            return isMatch ? match : null;
        }

        /// <summary>
        /// 匹配手机号码
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns>是否匹配成功</returns>
        public static bool IsPhoneNumber(this string s)
        {
            IsPhoneNumber(s, out var success);
            return success;
        }

        /// <summary>
        /// 匹配3位或4位区号的电话号码，其中区号可以用小括号括起来，
        /// 也可以不用，区号与本地号间可以用连字号或空格间隔，
        /// 也可以没有间隔
        /// \(0\d{2}\)[- ]?\d{8}|0\d{2}[- ]?\d{8}|\(0\d{3}\)[- ]?\d{7}|0\d{3}[- ]?\d{7}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhone(this string input)
        {
            var pattern =
                "^\\(0\\d{2}\\)[- ]?\\d{8}$|^0\\d{2}[- ]?\\d{8}$|^\\(0\\d{3}\\)[- ]?\\d{7}$|^0\\d{3}[- ]?\\d{7}$";
            var regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        #endregion 校验手机号码的正确性

        #region 校验邮箱的正确性

        private const string emailPattern =
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        /// <summary>
        /// 判断输入的字符串是否是一个合法的Email地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(this string input)
        {
            var regex = new Regex(emailPattern);
            return regex.IsMatch(input);
        }

        #endregion

        #region 包含中文判断

        /// <summary>
        /// 是否存在中文字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsContainsChinese(this string input)
        {
            return Regex.IsMatch(input, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 判断输入的字符串只包含汉字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsContainsOnlyChinese(this string input)
        {
            Regex regex = new Regex("^[\u4e00-\u9fa5]+$");
            return regex.IsMatch(input);
        }

        #endregion

        #region 其他判断

        /// <summary>
        /// 是否存在空格
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsContainsSpace(this string input)
        {
            return input.Contains(" ");
        }

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
        /// 检查字符串移除空字符后是否不为空
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static bool IsTrimNotNullAndEmpty(this string selfStr)
        {
            if (string.IsNullOrEmpty(selfStr))
                return false;
            return !string.IsNullOrEmpty(selfStr.Trim());
        }


        /// <summary>
        /// 判断输入的字符串只包含数字
        /// 可以匹配整数和浮点数
        /// ^-?\d+$|^(-?\d+)(\.\d+)?$
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(this string input)
        {
            string pattern = "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 是否正整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUint(this string input)
        {
            Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 判断输入的字符串只包含英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEnglishCh(this string input)
        {
            Regex regex = new Regex("^[A-Za-z]+$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 判断输入的字符串是否只包含数字和英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumAndEnCh(this string input)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        #endregion
    }
}