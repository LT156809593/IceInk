#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：AbpCompanyLiteDto
// 创 建 者：IceInk
// 创建时间：2021/4/2/周五 上午 10:31:01
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using System;

namespace IceInkAbp.Login
{
    /// <summary>
    /// 企业信息轻量输出模型
    /// </summary>
    public class AbpCompanyLiteDto
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司logo
        /// </summary>
        public string LogoPath { get; set; } = "";
    }
}

