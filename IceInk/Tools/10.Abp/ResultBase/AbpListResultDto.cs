#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：ListResultDto
// 创 建 者：IceInk
// 创建时间：2021/3/26/周五 上午 10:05:46
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
    /// Implements <see cref="IAbpListResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">接收的类型<see cref="Items"/> list</typeparam>
    [Serializable]
    public class AbpListResultDto<T> : IAbpListResult<T>
    {
        /// <summary>
        /// 类型列表.
        /// </summary>
        public IReadOnlyList<T> Items
        {
            get { return _items ?? (_items = new List<T>()); }
            set { _items = value; }
        }
        private IReadOnlyList<T> _items;

        /// <summary>
        /// 创建一个新对象 <see cref="AbpListResultDto{T}"/> .
        /// </summary>
        public AbpListResultDto()
        {

        }

        /// <summary>
        /// 创建一个新对象 <see cref="AbpListResultDto{T}"/> .
        /// </summary>
        /// <param name="items">类型列表</param>
        public AbpListResultDto(IReadOnlyList<T> items)
        {
            Items = items;
        }
    }
}