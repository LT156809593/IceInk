#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EkHttpStatusCodeDescriptionAttribute
// 创 建 者：作者名称
// 创建时间：2020/09/03 15:14:12
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		用于标记 HttpStatusCode
//
//----------------------------------------------------------------*/
#endregion

using System;

namespace IceInk
{
    /// <summary>
    /// 用于标记 HttpStatusCode
    /// 添加描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EkHttpStatusCodeDescriptionAttribute : Attribute
    {
        /// <summary>
        /// 状态描述
        /// </summary>
        public string Description { get; private set; }

        public EkHttpStatusCodeDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}

