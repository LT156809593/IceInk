#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EkCpuInfo
// 创 建 者：作者名称
// 创建时间：2020/09/03 14:29:58
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		CPU模型
//
//----------------------------------------------------------------*/
#endregion

namespace IceInk
{
    /// <summary>
    /// CPU模型
    /// </summary>
    public class EkCpuInfo
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// CPU型号 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// CPU厂商
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// CPU最大睿频
        /// </summary>
        public string MaxClockSpeed { get; set; }

        /// <summary>
        /// CPU的时钟频率
        /// </summary>
        public string CurrentClockSpeed { get; set; }

        /// <summary>
        /// CPU核心数
        /// </summary>
        public int NumberOfCores { get; set; }

        /// <summary>
        /// 逻辑处理器核心数
        /// </summary>
        public int NumberOfLogicalProcessors { get; set; }

        /// <summary>
        /// CPU使用率
        /// </summary>
        public double CpuLoad { get; set; }

        /// <summary>
        /// CPU位宽
        /// </summary>
        public string DataWidth { get; set; }

        /// <summary>
        /// 核心温度
        /// </summary>
        public double Temperature { get; set; }
    }
#pragma warning restore 1591
}

