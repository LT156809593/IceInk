#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：PagedResultDto
// 创 建 者：IceInk
// 创建时间：2021/3/26/周五 上午 10:03:55
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;

namespace IceInk.ABP
{
    /// <summary>
    /// 接收项的dto
    /// </summary>
    /// <summary>
    /// Implements <see cref="IAbpPagedResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">接收的类型 <see cref="AbpListResultDto{T}ems"/> list</typeparam>
    [Serializable]
    public class AbpResult<T> : AbpListResultDto<T>, IAbpPagedResult<T>
    {
        /// <summary>
        /// 项目总数.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 创建一个新对象 <see cref="AbpResult{T}"/> .
        /// </summary>
        public AbpResult()
        {

        }

        /// <summary>
        /// 创建一个新对象<see cref="AbpResult{T}"/> .
        /// </summary>
        /// <param name="totalCount">项目总数</param>
        /// <param name="items">当前页面的项目列表</param>
        public AbpResult(int totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }
}

