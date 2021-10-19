#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：AbpTokenMgr
// 创 建 者：IceInk
// 创建时间：2021/3/30/周二 下午 03:33:12
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

namespace IceInkAbp.Login
{
    /// <summary>
    /// Token 管理
    /// </summary>
    public static class AbpTokenMgr
    {
        /// <summary>
        /// 用户登录返回的结果Token
        /// </summary>
        public static AbpAuthenticateResult TokenResult { get; set; } = new AbpAuthenticateResult();

    }
}

