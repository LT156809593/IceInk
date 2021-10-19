﻿#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：ResultBase
// 创 建 者：IceInk
// 创建时间：2021/3/26/周五 上午 10:39:21
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
    /// 返回单个数据Base
    /// </summary>
    public class AbpResultBase<T> : ABPResultRoot
    {

        /// <summary>
        /// 接收到的数据
        /// </summary>
        public T Result { get; set; }
    }

}

