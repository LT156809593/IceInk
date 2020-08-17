/**************************************************************************
*   
*   =================================
*   CLR 版本    ：
*   命名空间    ：IceInk.Extension
*   文件名称    ：DictionaryExtension.cs
*   =================================
*   创 建 者    ：IceInk
*   创建日期    ：2019/1/10 11:46:04 
*   功能描述    ：
*           Dictionary  的扩展使用
*   使用说明    ：
*   =================================
*  
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
namespace IceInk.Extension
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// 合并字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="dictionaries"></param>
        /// <returns>返回合并后后的字典</returns>
        public static Dictionary<TKey, TValue> EkMerge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            params Dictionary<TKey, TValue>[] dictionaries)
        {
            return dictionaries.Aggregate(dictionary,
                (current, self) 
                    => current.Union(self)
                        .ToDictionary(kv => kv.Key, kv => kv.Value));
        }

        /// <summary>
        /// 遍历字典
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="self"></param>
        /// <param name="action"></param>
        public static void EkForEach<K, V>(this Dictionary<K, V> self, Action<K, V> action)
        {
            var dictE = self.GetEnumerator();
            while (dictE.MoveNext())
            {
                var current = dictE.Current;
                action(current.Key, current.Value);
            }

            dictE.Dispose();
        }

        /// <summary>
        /// 字典添加新的词典
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="self"></param>
        /// <param name="addInDict"></param>
        /// <param name="isOverride"></param>
        public static void EkAddRange<K, V>(this Dictionary<K, V> self, Dictionary<K, V> addInDict, bool isOverride = false)
        {
            var dictE = addInDict.GetEnumerator();

            while (dictE.MoveNext())
            {
                var current = dictE.Current;
                if (self.ContainsKey(current.Key))
                {
                    if (isOverride)
                        self[current.Key] = current.Value;
                    continue;
                }

                self.Add(current.Key, current.Value);
            }

            dictE.Dispose();
        }

    }
}