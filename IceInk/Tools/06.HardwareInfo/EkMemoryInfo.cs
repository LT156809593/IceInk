#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：EkMemoryInfo
// 创 建 者：作者名称
// 创建时间：2020/09/03 14:31:26
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		定义内存的信息结构
//
//----------------------------------------------------------------*/

#endregion

using System.Runtime.InteropServices;

namespace IceInk
{
    /// <summary>
    /// 定义内存的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct EkMemoryInfo
    {
#pragma warning disable 1591
        public uint dwLength;
        public uint dwMemoryLoad;
        public uint dwTotalPhys;
        public uint dwAvailPhys;
        public uint dwTotalPageFile;
        public uint dwAvailPageFile;
        public uint dwTotalVirtual;
        public uint dwAvailVirtual;
#pragma warning restore 1591
    }
}