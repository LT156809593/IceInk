#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：ResultRoot
// 创 建 者：IceInk
// 创建时间：2021/3/30/周二 下午 03:50:49
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
    /// 返回结果根
    /// </summary>
    public abstract class ABPResultRoot
    {
        public string TargetUrl { get; set; }

        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 错误
        /// </summary>
        public AbpResultErrorMsg Error { get; set; }

        public bool UnAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }
}

