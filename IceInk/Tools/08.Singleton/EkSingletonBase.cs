#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：EkSingletonBase
// 创 建 者：IceInk
// 创建时间：2020年09月14日 星期一 09:29
// 文件版本：V1.0.0
//===============================================================
// 功能描述：
//		单例模式父类，需要使用单利模式的类继承此类即可
//  说明：
//      单例模式可以说是使用到的最频繁的设计模式之一，单例模式（Singleton）是几个创建模式中最对立的一个，
//      它的主要特点不是根据用户程序调用生成一个新的实例，而是控制某个类型的实例唯一性。
//----------------------------------------------------------------*/

#endregion

using System;

namespace IceInk
{
    /// <summary>
    ///     单例模式父类，需要使用单利模式的类继承此类即可
    /// </summary>
    public class EkSingletonBase<T> where T : class, new()
    {
        #region 方法1：使用System.Lazy<T>延迟加载【推荐】

        //Lazy 对象初始化默认是线程安全的，在多线程环境下，
        //第一个访问 Lazy 对象的 Value 属性的线程将初始化 Lazy 对象，
        //以后访问的线程都将使用第一次初始化的数据。
        private static Lazy<T> _lazy;

        /// <summary>
        ///     单例
        /// </summary>
        /// <param name="forceInit">强制更新实例</param>
        /// <returns></returns>
        public static T Instance(bool forceInit = false)
        {
            if (_lazy == null || forceInit)
                Create();
            return _lazy.Value;
        }

        private static void Create()
        {
            _lazy = new Lazy<T>(() => new T());

            #region 控制台输出创建单例的名称

            Console.WriteLine("----------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"创建单例: {typeof(T).FullName}");
            Console.ResetColor();
            Console.WriteLine("----------------------------------------------------------------");

            #endregion
        }

        #endregion


        #region 方法2：双重检查锁定

        /*
        protected static T _instance = null;
        private static readonly object obj_lock = new object();
        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance
        {//双重检查锁定【适用与作为缓存存在的单例模式】
            get
            {
                if (_instance == null)
                {//此处判断是用于优化，避免每次调用都要lock等待，
                    lock (obj_lock)
                    {//加锁  避免多线程调用而多次创建
                        if (_instance == null)
                        {//创建实例
                            _instance = new T();
                        }
                    }
                }

                return _instance;
            }
        }
        */

        #endregion
    }
}