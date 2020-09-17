#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：CPU_INFO
// 创 建 者：作者名称
// 创建时间：2020/09/03 14:15:17
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/

#endregion

using System.Runtime.InteropServices;

namespace IceInk
{
    public static partial class SystemInfo
    {
        #region 定义CPU的信息结构

        /// <summary>
        ///     定义CPU的信息结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct CPU_INFO
        {
#pragma warning disable 1591
            public uint dwOemId;
            public uint dwPageSize;
            public uint lpMinimumApplicationAddress;
            public uint lpMaximumApplicationAddress;
            public uint dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public uint dwProcessorLevel;
            public uint dwProcessorRevision;
#pragma warning restore 1591
        }
#pragma warning restore 1591

        #endregion
    }
}