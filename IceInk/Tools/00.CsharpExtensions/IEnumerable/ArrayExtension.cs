/**************************************************************************
*   
*   =================================
*   CLR 版本    ：
*   命名空间    ：IceInk.Extension
*   文件名称    ：ArrayExtension.cs
*   =================================
*   创 建 者    ：IceInk
*   创建日期    ：2019/4/10 11:46:04 
*   功能描述    ：
*           Array  的扩展使用
*   使用说明    ：
*   =================================
*  
***************************************************************************/

using System;
using System.Collections.Generic;

namespace IceInk.Extension
{
    public static class ArrayExtension
    {
        /// <summary>
        ///     遍历数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfArray"></param>
        /// <param name="action"></param>
        /// <returns>返回自己</returns>
        public static T[] EkForEach<T>(this T[] selfArray, Action<T> action)
        {
            foreach (var item in selfArray)
                action.Invoke(item);
            return selfArray;
        }

        /// <summary>
        ///     遍历数组 (可获取索引)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfArray"></param>
        /// <param name="action"></param>
        /// <returns>返回自己</returns>
        public static T[] EkForEach<T>(this T[] selfArray, Action<T, int> action)
        {
            if (selfArray == null) throw new ArgumentException();
            var length = selfArray.Length;
            for (var i = 0; i < length; i++)
            {
                var index = i;
                action.Invoke(selfArray[index], index);
            }

            return selfArray;
        }

        /// <summary>
        ///     倒序遍历数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfArray"></param>
        /// <param name="action"></param>
        /// <returns>返回自己</returns>
        public static T[] EkForEachReverse<T>(this T[] selfArray, Action<T> action)
        {
            if (selfArray == null) throw new ArgumentException();
            var length = selfArray.Length;
            for (var i = length - 1; i >= 0; i--)
            {
                var index = i;
                action.Invoke(selfArray[index]);
            }

            return selfArray;
        }


        /// <summary>
        ///     倒序遍历数组 (可获取索引)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfArray"></param>
        /// <param name="action"></param>
        /// <returns>返回自己</returns>
        public static T[] EkForEachReverse<T>(this T[] selfArray, Action<T, int> action)
        {
            if (selfArray == null) throw new ArgumentException();
            var length = selfArray.Length;
            for (var i = length - 1; i >= 0; i--)
            {
                var index = i;
                action.Invoke(selfArray[index], index);
            }

            return selfArray;
        }


        /// <summary>
        ///     遍历 IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfArray"></param>
        /// <param name="action"></param>
        /// <returns>返回自己</returns>
        public static IEnumerable<T> EkForEach<T>(this IEnumerable<T> selfArray, Action<T> action)
        {
            if (action == null) throw new ArgumentException();
            foreach (var item in selfArray)
                action(item);
            return selfArray;
        }
    }
}