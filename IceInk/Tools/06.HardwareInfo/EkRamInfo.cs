#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EkRamInfo
// 创 建 者：作者名称
// 创建时间：2020/09/03 14:32:02
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		EkRamInfo
//
//----------------------------------------------------------------*/
#endregion


namespace IceInk
{
    /// <summary>
    /// 内存条模型
    /// </summary>
    public class EkRamInfo
    {
        /// <summary>
        /// 可用物理内存
        /// </summary>
        public double MemoryAvailable { get; set; }

        /// <summary>
        /// 物理总内存
        /// </summary>
        public double PhysicalMemory { get; set; }

        /// <summary>
        /// 分页内存总数
        /// </summary>
        public double TotalPageFile { get; set; }

        /// <summary>
        /// 分页内存可用
        /// </summary>
        public double AvailablePageFile { get; set; }

        /// <summary>
        /// 虚拟内存总数
        /// </summary>
        public double TotalVirtual { get; set; }

        /// <summary>
        /// 虚拟内存可用
        /// </summary>
        public double AvailableVirtual { get; set; }

        /// <summary>
        /// 内存使用率
        /// </summary>
        public double MemoryUsage => (1 - MemoryAvailable / PhysicalMemory) * 100;
    }
}

