#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：AbpUserLoginInput
// 创 建 者：IceInk
// 创建时间：2021/3/26/周五 上午 11:39:45
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
    /// 客服端用户登录实体
    /// </summary>
    public class AbpUserLoginInput
    {
        /// <summary>
        /// 用户名或邮箱
        /// </summary>
        public string UserNameOrEmailAddress { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 动态口令
        /// </summary>
        public string DynamicToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RememberClient { get; set; } = true;

    }
}

