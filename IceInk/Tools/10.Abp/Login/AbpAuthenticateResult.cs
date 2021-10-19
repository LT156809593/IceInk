#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：AbpAuthenticateResult
// 创 建 者：IceInk
// 创建时间：2021/3/26/周五 下午 01:56:48
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
    /// 用户登录认证返回结果和Token
    /// </summary>
    public class AbpAuthenticateResult
    {
        /// <summary>
        /// 登录成功返回的Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Token在几秒钟内到期
        /// </summary>
        public int ExpireInSeconds { get; set; }

        public string OpenId { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>

        public string RefreshToken { get; set; }
    }
}

