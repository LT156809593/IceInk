#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：ResultErrorMsg
// 创 建 者：IceInk
// 创建时间：2021/3/26/周五 上午 10:53:24
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

namespace IceInk.ABP
{
    /// <summary>
    /// 接收到的错误信息
    /// </summary>
    public class AbpResultErrorMsg
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误详细信息
        /// </summary>
        public object Details { get; set; }

        /// <summary>
        /// 验证错误
        /// </summary>
        public object ValidationErrors { get; set; }
    }
}

