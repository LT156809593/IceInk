#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EkEventSystem
// 创 建 者：IceInk
// 创建时间：2020/7/5 22:19:43
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		事件系统
//          使用 枚举 注册
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;

namespace IceInk
{
    public interface ITypeEventSystem : IDisposable
    {
        IDisposable RegisterEvent<T>(Action<T> onReceive);
        void UnRegisterEvent<T>(Action<T> onReceive);

        void SendEvent<T>() where T : new();

        void SendEvent<T>(T e);
        void Clear();
    }


    public class TypeEventUnRegister<T> : IDisposable
    {
        public Action<T> OnReceive;

        public void Dispose()
        {
            EkTypeEventSystem.UnRegister(OnReceive);
        }
    }

    public class EkTypeEventSystem : ITypeEventSystem
    {
        /// <summary>
        /// 接口 只负责存储在字典中
        /// </summary>
        interface IRegisterAtions : IDisposable
        {

        }


        /// <summary>
        /// 多个注册
        /// </summary>
        class RegisterAtions<T> : IRegisterAtions
        {
            /// <summary>
            /// 接收Action
            /// 因为委托本身就可以一对多注册
            /// </summary>
            public Action<T> OnReceives = obj => { };

            public void Dispose()
            {
                OnReceives = null;
            }
        }

        /// <summary>
        /// 全局注册事件
        /// </summary>
        private static readonly ITypeEventSystem mGlobalEventSystem = new EkTypeEventSystem();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<Type, IRegisterAtions> mTypeEventDic = new Dictionary<Type, IRegisterAtions>();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="onReceive"></param>
        /// <typeparam name="T"></typeparam>
        public static IDisposable Register<T>(System.Action<T> onReceive)
        {
            return mGlobalEventSystem.RegisterEvent<T>(onReceive);
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <param name="onReceive"></param>
        /// <typeparam name="T"></typeparam>
        public static void UnRegister<T>(System.Action<T> onReceive)
        {
            mGlobalEventSystem.UnRegisterEvent<T>(onReceive);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        public static void Send<T>(T t)
        {
            mGlobalEventSystem.SendEvent<T>(t);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Send<T>() where T : new()
        {
            mGlobalEventSystem.SendEvent<T>();
        }


        public IDisposable RegisterEvent<T>(Action<T> onReceive)
        {
            var type = typeof(T);

            IRegisterAtions registerAtions;

            if (mTypeEventDic.TryGetValue(type, out registerAtions))
            {
                var reg = registerAtions as RegisterAtions<T>;
                reg.OnReceives += onReceive;
            }
            else
            {
                RegisterAtions<T> reg = new RegisterAtions<T>();
                reg.OnReceives += onReceive;
                mTypeEventDic.Add(type, reg);
            }

            return new TypeEventUnRegister<T> { OnReceive = onReceive };
        }

        public void UnRegisterEvent<T>(Action<T> onReceive)
        {
            var type = typeof(T);

            IRegisterAtions registerAtions;

            if (mTypeEventDic.TryGetValue(type, out registerAtions))
            {
                RegisterAtions<T> reg = registerAtions as RegisterAtions<T>;
                if (reg != null) reg.OnReceives -= onReceive;
            }
        }

        public void SendEvent<T>() where T : new()
        {
            var type = typeof(T);

            IRegisterAtions registerAtions;

            if (mTypeEventDic.TryGetValue(type, out registerAtions))
            {
                RegisterAtions<T> reg = registerAtions as RegisterAtions<T>;
                reg?.OnReceives(new T());
            }
        }

        public void SendEvent<T>(T e)
        {
            var type = typeof(T);

            IRegisterAtions registerAtions;

            if (mTypeEventDic.TryGetValue(type, out registerAtions))
            {
                RegisterAtions<T> reg = registerAtions as RegisterAtions<T>;
                reg?.OnReceives(e);
            }
        }

        public void Clear()
        {
            foreach (var keyValuePair in mTypeEventDic)
            {
                keyValuePair.Value.Dispose();
            }

            mTypeEventDic.Clear();
        }

        public void Dispose()
        {

        }
    }

    public interface IDisposableList : IDisposable
    {
        void Add(IDisposable disposable);
    }

    public class DisposableList : IDisposableList
    {
        List<IDisposable> mDisposableList = new List<IDisposable>();

        public void Add(IDisposable disposable)
        {
            mDisposableList.Add(disposable);
        }

        public void Dispose()
        {
            foreach (var disposable in mDisposableList)
            {
                disposable.Dispose();
            }

            mDisposableList = null;
        }
    }

    public static class DisposableExtensions
    {
        public static void AddTo(this IDisposable self, IDisposableList component)
        {
            component.Add(self);
        }
    }
}
