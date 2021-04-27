#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EkWindowsMemoryClear
// 创 建 者：IceInk
// 创建时间：2021/4/27/周二 下午 08:48:54
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;

namespace IceInk
{
    /// <summary>
    /// 系统内存清理
    /// </summary>
    public class EkWindowsMemoryClear
    {

        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            //清理内存
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                //以下系统进程没有权限，所以跳过，防止出错影响效率。  
                if ((process.ProcessName == "System") && (process.ProcessName == "Idle"))
                    continue;
                try
                {
                    EmptyWorkingSet(process.Handle);
                }
                catch
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// 启用定时清理内存
        /// 启用一次就可以
        /// </summary>
        /// <param name="minutes">每隔多长时间清理一次 分钟</param>
        public static void StartTimeClearMemory(int minutes)
        {
            System.Timers.Timer aTimer = new System.Timers.Timer(); //初始化定时器
            aTimer.Interval = minutes * 60000;//配置定时时间 毫秒  1分钟=60000毫秒
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;//每到指定时间Elapsed事件是到时间就触发
            aTimer.Enabled = true; //指示 Timer 是否应引发 Elapsed 事件。
        }


        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            ClearMemory();
        }

        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);



    }
}

