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
using System.Linq;
namespace IceInk.Extension
{
    public static partial class ListExtension
    {
        public static List<T> ReverseForEach<T>(this List<T> selfList, Action<T> action)
        {
            if (action == null) throw new ArgumentException();

            for (var i = selfList.Count - 1; i >= 0; --i)
                action(selfList[i]);
            return selfList;
        }
        public static void ForEach<T>(this List<T> list, Action<int, T> action)
        {
            for (var i = 0; i < list.Count; i++)
            {
                action(i, list[i]);
            }
        }
        public static void CopyTo<T>(this List<T> from, List<T> to, int begin = 0, int end = -1)
        {
            if (begin < 0) begin = 0;
            var endIndex = Math.Min(from.Count, to.Count) - 1;
            if (end != -1 && end < endIndex) endIndex = end;
            for (var i = begin; i < end; i++)
            {
                to[i] = from[i];
            }
        }
        public static T Dequeue<T>(this List<T> list)
        {
            if (list == null || list.Count <= 0) throw new Exception("Null List");
            T t = list[0];
            list.Remove(t);
            return t;
        }
        public static void Enqueue<T>(this List<T> list, T t)
        {
            if (list == null) throw new Exception("Null List");
            list.Add(t);
        }
        public static void Push<T>(this List<T> list, T t)
        {
            if (list == null) throw new Exception("Null List");
            list.Add(t);
        }
        public static T Pop<T>(this List<T> list)
        {
            if (list == null || list.Count <= 0) throw new Exception("Null List");
            T t = list[list.Count - 1];
            list.Remove(t);
            return t;
        }
        public static T QueuePeek<T>(this List<T> list)
        {
            if (list == null || list.Count <= 0) throw new Exception("Null List");
            return list[0];
        }
        public static T StackPeek<T>(this List<T> list)
        {
            if (list == null || list.Count <= 0) throw new Exception("Null List");
            return list[list.Count - 1];
        }
        //0,1,3,4=>2
        public static int GetEmptyInt(this List<int> list, int minVal = 0)
        {
            list.Sort();
            list = list.Distinct().ToList();
            switch (list.Count)
            {
                case 0: return minVal;
                case 1: return list[0] == minVal ? minVal + 1 : minVal;
                default:
                    int tempIndex = 0;
                    if (IsExistEmptyIndex(list, 0, list.Count - 1, ref tempIndex))
                    {
                        return tempIndex;
                    }
                    return list[0] > minVal ? minVal : list[list.Count - 1] + 1;
            }
        }
        private static bool IsExistEmptyIndex(List<int> list, int startIndex, int endIndex, ref int index)
        {
            if (Math.Abs(list[startIndex] - list[endIndex]) == endIndex - startIndex) return false;
            if (endIndex - startIndex == 1)
            {
                index = list[startIndex] + 1;
                return true;
            }
            int midIndex = (endIndex - startIndex) / 2 + startIndex;
            //   if (midIndex < startIndex || midIndex > endIndex) return false;
            return IsExistEmptyIndex(list, startIndex, midIndex, ref index) ||
                IsExistEmptyIndex(list, midIndex, endIndex, ref index);
        }
    }
}