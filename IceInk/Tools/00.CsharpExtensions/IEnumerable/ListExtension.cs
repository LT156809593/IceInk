/**************************************************************************
*   
*   =================================
*   CLR 版本    ：
*   命名空间    ：IceInk.Extension
*   文件名称    ：DictionaryExtension.cs
*   =================================
*   创 建 者    ：IceInk
*   创建日期    ：2019/9/10 11:46:04 
*   功能描述    ：
*           List  的扩展使用
*   使用说明    ：
*   =================================
*  
***************************************************************************/

using System;
using System.Collections.Generic;

namespace IceInk.Extension
{
    public static class EkListExtension
    {
        /// <summary>
        /// 倒序遍历
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfList"></param>
        /// <param name="action"></param>
        /// <returns>返回自己</returns>
        public static List<T> EkForEachReverse<T>(this List<T> selfList, Action<T> action)
        {
            if (action == null) throw new ArgumentException();
            var count = selfList.Count;
            for (var i = count - 1; i >= 0; --i)
                action(selfList[i]);
            return selfList;
        }

        /// <summary>
        /// 倒序遍历(可获取索引)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfList"></param>
        /// <param name="action"></param>
        /// <returns>返回自己</returns>
        public static List<T> EkForEachReverse<T>(this List<T> selfList, Action<T, int> action)
        {
            if (action == null) throw new ArgumentException();
            var count = selfList.Count;
            for (var i = count - 1; i >= 0; --i)
                action(selfList[i], i);

            return selfList;
        }

        /// <summary>
        /// 遍历列表(可获得索引）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfList"></param>
        /// <param name="action"></param>
        public static void EkForEach<T>(this List<T> selfList, Action<int, T> action)
        {
            if (selfList == null)
                return;
            var count = selfList.Count;
            for (var i = 0; i < count; i++) action(i, selfList[i]);
        }
    }
}