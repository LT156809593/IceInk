#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EkChangeContentType
// 创 建 者：作者名称
// 创建时间：2020/09/03 15:14:51
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

namespace IceInk
{
    /// <summary>
    /// Http请求的数据提交类型
    /// </summary>
    public enum EkChangeContentType
    {
        /// <summary>
        /// 告诉服务端消息主体是序列化后的 JSON 字符串
        /// </summary>
        ApplicationJson,

        /// <summary>
        /// 浏览器的原生Form表单
        /// </summary>
        ApplicationXWwwFormUrlencoded,

        /// <summary>
        /// 使用表单上传文件时，必须让 Form 的 enctype 等于这个值
        /// </summary>
        MultipartFormData,

        /// <summary>
        /// XML 作为编码方式的远程调用规范
        /// </summary>
        TextXml,
    }

}

