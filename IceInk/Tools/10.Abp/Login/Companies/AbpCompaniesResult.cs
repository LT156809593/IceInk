using System.Collections.Generic;

namespace IceInkAbp.Login
{
    /// <summary>
    /// ABP 登录获取公司列表信息 输出
    /// </summary>
    public class AbpCompaniesResult
    {
        /// <summary>
        /// 公司列表
        /// </summary>
        public List<AbpCompanyLiteDto> Items { get; set; }
    }
}