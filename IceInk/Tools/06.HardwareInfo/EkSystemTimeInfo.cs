#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：EkSystemTimeInfo
// 创 建 者：作者名称
// 创建时间：2020/09/03 14:32:50
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		定义系统时间的信息结构
//
//----------------------------------------------------------------*/

#endregion

using System.Runtime.InteropServices;

namespace IceInk
{
    /// <summary>
    ///     定义系统时间的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct EkSystemTimeInfo
    {
#pragma warning disable 1591
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
#pragma warning restore 1591
    }
}