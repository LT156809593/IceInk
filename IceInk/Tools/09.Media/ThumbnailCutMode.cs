#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：ThumbnailCutMode
// 创 建 者：IceInk
// 创建时间：2021/4/12/周一 下午 07:54:02
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace IceInk.Media
{
    /// <summary>
    /// 图像裁剪模式
    /// </summary>
    public enum ThumbnailCutMode
    {
        /// <summary>
        /// 锁定高度
        /// </summary>
        LockHeight,

        /// <summary>
        /// 锁定宽度
        /// </summary>
        LockWidth,

        /// <summary>
        /// 固定宽高
        /// </summary>
        Fixed,

        /// <summary>
        /// 裁剪
        /// </summary>
        Cut
    }
}

