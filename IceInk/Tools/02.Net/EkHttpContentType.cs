#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EkHttpContentType
// 创 建 者：作者名称
// 创建时间：2020/09/03 15:16:30
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

namespace IceInk
{
    /// <summary>
    /// HTTP请求中，几种常见的Content-Type类型
    /// </summary>
    public static class EkHttpContentType
    {
        /// <summary>
        /// 用来告诉服务端消息主体是序列化后的 JSON 字符串，
        /// 其中一个好处就是JSON 格式支持比键值对复杂得多的结构化数据。
        /// 由于 JSON 规范的流行，除了低版本 IE 之外的各大浏览器都原生支持
        /// </summary>
        private const string ApplicationJson = @"application/json";

        /// <summary>
        /// 浏览器的原生form表单
        /// 如果不设置 enctype 属性，默认为application/x-www-form-urlencoded 方式提交数据。
        /// 首先，Content-Type被指定为 application/x-www-form-urlencoded；
        /// 其次，提交的表单数据会转换为键值对并按照 key1=val1&key2=val2 的方式进行编码，key 和 val 都进行了 URL 转码。
        /// 大部分服务端语言都对这种方式有很好的支持。
        /// 另外，如利用AJAX 提交数据时，也可使用这种方式。
        /// 例如 jQuery，Content-Type 默认值都是”application/x-www-form-urlencoded;charset=utf-8”。
        /// </summary>
        private const string ApplicationXWwwFormUrlencoded = @"application/x-www-form-urlencoded";

        /// <summary>
        /// 使用表单上传文件时，必须让 Form 的 enctype 等于这个值
        /// 另一个常见的 POST 数据提交的方式， Form 表单的 enctype 设置为multipart/form-data，
        /// 它会将表单的数据处理为一条消息，以标签为单元，用分隔符（这就是boundary的作用）分开， 
        /// 由于这种方式将数据有很多部分，它既可以上传键值对，也可以上传文件，甚至多个文件。
        /// 当上传的字段是文件时，会有Content-Type来说明文件类型；
        /// Content-disposition， 用来说明字段的一些信息。
        /// 每部分都是以 –boundary 开始，紧接着是内容描述信息，然后是回车，
        /// 最后是字段具体内容（字段、文本或二进制等）。
        /// 如果传输的是文件，还要包含文件名和文件类型信息。
        /// 消息主体最后以 –boundary– 标示结束。
        /// </summary>
        private const string MultipartFormData = @"multipart/form-data";

        /// <summary>
        /// HTTP 作为传输协议，XML 作为编码方式的远程调用规范.
        /// XML的作用不言而喻，用于传输和存储数据，它非常适合万维网传输，
        /// 提供统一的方法来描述和交换独立于应用程序或供应商的结构化数据，
        /// 在JSON出现之前是业界一大标准（当然现在也是），
        /// 因此，在POST提交数据时，xml类型也是不可缺少的一种，虽然一般场景上使用JSON可能更轻巧、灵活。
        /// </summary>
        private const string TextXml = @"text/xml";

        /// <summary>
        /// 获取Http请求的数据提交方式
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns>返回数据提交方式(默认返回 "application/json")</returns>
        public static string ContentType(EkChangeContentType contentType = EkChangeContentType.ApplicationJson)
        {
            string str = string.Empty;
            switch (contentType)
            {
                case EkChangeContentType.ApplicationJson:
                    str = ApplicationJson;
                    break;
                case EkChangeContentType.ApplicationXWwwFormUrlencoded:
                    str = ApplicationXWwwFormUrlencoded;
                    break;
                case EkChangeContentType.MultipartFormData:
                    str = MultipartFormData;
                    break;
                case EkChangeContentType.TextXml:
                    str = TextXml;
                    break;
            }
            return str;
        }
    }
}

