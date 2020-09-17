#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：IntExtension
// 创 建 者：IceInk
// 创建时间：2020/8/17 14:01:46
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		Int类型扩展
//
//----------------------------------------------------------------*/

#endregion

using System;

namespace IceInk
{
    /// <summary>
    ///     Int类型扩展
    /// </summary>
    public static class IntExtension
    {
        /// <summary>
        ///     转换为枚举类型
        /// </summary>
        /// <typeparam name="TEnum">目标枚举类型</typeparam>
        /// <param name="number"></param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this int number) where TEnum : struct
        {
            return Enum.TryParse(number.ToString(), out TEnum result) ? result : default;
        }

        /// <summary>
        ///     转换为Byte
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static byte ToByte(this int number)
        {
            return Convert.ToByte(number);
        }

        #region 判断是否 质数 奇数 偶数

        /// <summary>
        ///     是否为奇数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsOdd(this int number)
        {
            return (number & 1) == 1;
        }

        /// <summary>
        ///     是否为偶数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsEven(this int number)
        {
            return (number & 1) != 1;
        }

        /// <summary>
        ///     是否为素数(质数)
        /// </summary>
        /// <param name="number">(非负数)</param>
        /// <returns></returns>
        public static bool IsPrime(this int number)
        {
            if (number <= 3) return number > 1;

            // 不在6的倍数两侧的一定不是质数
            if (number % 6 != 1 && number % 6 != 5) return false;

            var sqrt = (int) Math.Sqrt(number);
            for (var i = 5; i <= sqrt; i += 6)
                if (number % i == 0 || number % (i + 2) == 0)
                    return false;

            return true;
        }

        #endregion
    }
}