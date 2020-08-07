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
    public static partial class ArrayExtension
    {
        public static T[] ForEach<T>(this T[] selfArray, Action<T> action)
        {
            Array.ForEach(selfArray, action);
            return selfArray;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> selfArray, Action<T> action)
        {
            if (action == null) throw new ArgumentException();
            foreach (var item in selfArray)
            {
                action(item);
            }
            return selfArray;
        }
    }
}
