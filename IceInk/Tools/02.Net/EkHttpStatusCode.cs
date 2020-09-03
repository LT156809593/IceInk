#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：EkHttpStatusCode
// 创 建 者：作者名称
// 创建时间：2020/09/03 15:13:23
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		包含为 HTTP 定义的状态代码的值
//
//----------------------------------------------------------------*/
#endregion

namespace IceInk
{
    /// <summary>包含为 HTTP 定义的状态代码的值。</summary>
    public enum EkHttpStatusCode
    {
        /// <summary>
        ///   等效于 HTTP 状态 100。
        ///    指示客户端可以继续其请求。
        /// </summary>
        [EkHttpStatusCodeDescription("可以继续其请求")]
        Continue = 100, // 0x00000064

        /// <summary>
        ///   等效于 HTTP 状态为 101。
        ///    指示正在更改的协议版本或协议。
        /// </summary>
        [EkHttpStatusCodeDescription("正在更改的协议版本或协议")]
        SwitchingProtocols = 101, // 0x00000065

        /// <summary>
        ///   等效于 HTTP 状态 200。
        ///    指示请求成功，且请求的信息包含在响应中。
        ///    这是要接收的最常见状态代码。
        /// </summary>
        [EkHttpStatusCodeDescription("请求成功")] OK = 200, // 0x000000C8

        /// <summary>
        ///   等效于 HTTP 状态 201。
        ///    指示请求导致已发送响应之前创建一个新的资源。
        /// </summary>
        [EkHttpStatusCodeDescription("请求导致已发送响应之前创建一个新的资源")]
        Created = 201, // 0x000000C9

        /// <summary>
        ///   等效于 HTTP 状态 202。
        ///    指示请求已被接受进行进一步处理。
        /// </summary>
        [EkHttpStatusCodeDescription("请求已被接受进行进一步处理")]
        Accepted = 202, // 0x000000CA

        /// <summary>
        ///   等效于 HTTP 状态 203。
        ///    指示返回的元信息来自而不是原始服务器的缓存副本，因此可能不正确。
        /// </summary>
        [EkHttpStatusCodeDescription("返回的元信息来自而不是原始服务器的缓存副本，因此可能不正确")]
        NonAuthoritativeInformation = 203, // 0x000000CB

        /// <summary>
        ///   等效于 HTTP 状态 204。
        ///    指示已成功处理请求和响应是有意留为空白。
        /// </summary>
        [EkHttpStatusCodeDescription("已成功处理请求和响应是有意留为空白")]
        NoContent = 204, // 0x000000CC

        /// <summary>
        ///   等效于 HTTP 状态 205。
        ///    指示客户端应重置 （而不是重新加载） 的当前资源。
        /// </summary>
        [EkHttpStatusCodeDescription("客户端应重置 （而不是重新加载） 的当前资源")]
        ResetContent = 205, // 0x000000CD

        /// <summary>
        ///   等效于 HTTP 206 状态。
        ///    指示根据包括字节范围的 GET 请求的请求的响应是部分响应。
        /// </summary>
        [EkHttpStatusCodeDescription("根据包括字节范围的 GET 请求的请求的响应是部分响应")]
        PartialContent = 206, // 0x000000CE

        /// <summary>
        ///   等效于 HTTP 状态 300。
        ///    指示所需的信息有多种表示形式。
        ///    默认操作是将此状态视为一个重定向，并按照与此响应关联的位置标头的内容。
        /// </summary>
        [EkHttpStatusCodeDescription("所需的信息有多种表示形式(默认操作是将此状态视为一个重定向，并按照与此响应关联的位置标头的内容)")]
        Ambiguous = 300, // 0x0000012C

        /// <summary>
        ///   等效于 HTTP 状态 300。
        ///   指示所需的信息有多种表示形式。
        ///   默认操作是将此状态视为一个重定向，并按照与此响应关联的位置标头的内容。
        /// </summary>
        [EkHttpStatusCodeDescription("所需的信息有多种表示形式(默认操作是将此状态视为一个重定向，并按照与此响应关联的位置标头的内容)")]
        MultipleChoices = 300, // 0x0000012C

        /// <summary>
        ///   等效于 HTTP 状态 301。
        ///    指示已将所需的信息移动到的位置标头中指定的 URI。
        ///    当收到此状态时的默认操作是遵循与响应关联的位置标头。
        ///    当原始请求方法是 POST 时，重定向的请求将使用 GET 方法。
        /// </summary>
        [EkHttpStatusCodeDescription("已将所需的信息移动到的位置标头中指定的 URI")]
        Moved = 301, // 0x0000012D

        /// <summary>
        ///   等效于 HTTP 状态 301。
        ///    指示已将所需的信息移动到的位置标头中指定的 URI。
        ///    当收到此状态时的默认操作是遵循与响应关联的位置标头。
        /// </summary>
        [EkHttpStatusCodeDescription("已将所需的信息移动到的位置标头中指定的 URI")]
        MovedPermanently = 301, // 0x0000012D

        /// <summary>
        ///   等效于 HTTP 状态 302。
        ///    指示所需的信息位于的位置标头中指定的 URI。
        ///    当收到此状态时的默认操作是遵循与响应关联的位置标头。
        ///    当原始请求方法是 POST 时，重定向的请求将使用 GET 方法。
        /// </summary>
        [EkHttpStatusCodeDescription("已将所需的信息移动到的位置标头中指定的 URI")]
        Found = 302, // 0x0000012E

        /// <summary>
        ///   等效于 HTTP 状态 302。
        ///    指示所需的信息位于的位置标头中指定的 URI。
        ///    当收到此状态时的默认操作是遵循与响应关联的位置标头。
        ///    当原始请求方法是 POST 时，重定向的请求将使用 GET 方法。
        /// </summary>
        [EkHttpStatusCodeDescription("已将所需的信息移动到的位置标头中指定的 URI")]
        Redirect = 302, // 0x0000012E

        /// <summary>
        ///   等效于 HTTP 状态 303。
        ///    自动将客户端重定向到的位置标头中指定作为公告的结果的 URI。
        ///    对指定的位置标头的资源的请求将会执行与 GET。
        /// </summary>
        [EkHttpStatusCodeDescription("自动将客户端重定向到的位置标头中指定作为公告的结果的 URI")]
        RedirectMethod = 303, // 0x0000012F

        /// <summary>
        ///   等效于 HTTP 状态 303。
        ///    自动将客户端重定向到的位置标头中指定作为公告的结果的 URI。
        ///    对指定的位置标头的资源的请求将会执行与 GET。
        /// </summary>
        [EkHttpStatusCodeDescription("自动将客户端重定向到的位置标头中指定作为公告的结果的 URI")]
        SeeOther = 303, // 0x0000012F

        /// <summary>
        ///   等效于 HTTP 状态 304。
        ///   指示客户端的缓存的副本是最新。
        ///    不会传输资源的内容。
        /// </summary>
        [EkHttpStatusCodeDescription("客户端的缓存的副本是最新")]
        NotModified = 304, // 0x00000130

        /// <summary>
        ///   等效于 HTTP 状态 305。
        ///    指示该请求应使用的位置标头中指定的 uri 的代理服务器。
        /// </summary>
        [EkHttpStatusCodeDescription("该请求应使用的位置标头中指定的 uri 的代理服务器")]
        UseProxy = 305, // 0x00000131

        /// <summary>
        ///   等效于 HTTP 状态 306。
        ///    是对未完全指定的 HTTP/1.1 规范建议的扩展。
        /// </summary>
        [EkHttpStatusCodeDescription("是对未完全指定的 HTTP/1.1 规范建议的扩展")]
        Unused = 306, // 0x00000132

        /// <summary>
        ///   等效于 HTTP 状态 307。
        ///    指示请求信息位于的位置标头中指定的 URI。
        ///    当收到此状态时的默认操作是遵循与响应关联的位置标头。
        ///    当原始请求方法是 POST 时，重定向的请求还将使用 POST 方法。
        /// </summary>
        [EkHttpStatusCodeDescription("请求信息位于的位置标头中指定的 URI")]
        RedirectKeepVerb = 307, // 0x00000133

        /// <summary>
        ///   等效于 HTTP 状态 307。
        ///    指示请求信息位于的位置标头中指定的 URI。
        ///    当收到此状态时的默认操作是遵循与响应关联的位置标头。
        ///    当原始请求方法是 POST 时，重定向的请求还将使用 POST 方法。
        /// </summary>
        [EkHttpStatusCodeDescription("请求信息位于的位置标头中指定的 URI")]
        TemporaryRedirect = 307, // 0x00000133

        /// <summary>
        ///   等效于 HTTP 状态 400。
        ///    指示无法由服务器理解此请求。
        ///    如果没有其他错误适用，或者如果具体的错误是未知的或不具有其自己的错误代码发送。
        /// </summary>
        [EkHttpStatusCodeDescription("无法由服务器理解此请求")]
        BadRequest = 400, // 0x00000190

        /// <summary>
        ///   等效于 HTTP 状态 401。
        ///    指示所请求的资源需要身份验证。
        ///    Www-authenticate 标头包含如何执行身份验证的详细信息。
        /// </summary>
        [EkHttpStatusCodeDescription("所请求的资源需要身份验证")]
        Unauthorized = 401, // 0x00000191

        /// <summary>
        ///   等效于 HTTP 状态 402。
        ///   已保留供将来使用。
        /// </summary>
        [EkHttpStatusCodeDescription("已保留供将来使用")]
        PaymentRequired = 402, // 0x00000192

        /// <summary>
        ///   等效于 HTTP 状态 403。
        ///   指示服务器拒绝无法完成请求。
        /// </summary>
        [EkHttpStatusCodeDescription("服务器拒绝无法完成请求")]
        Forbidden = 403, // 0x00000193

        /// <summary>
        ///   等效于 HTTP 状态 404。
        ///   指示所请求的资源不存在服务器上。
        /// </summary>
        [EkHttpStatusCodeDescription("所请求的资源不存在服务器上")]
        NotFound = 404, // 0x00000194

        /// <summary>
        ///   等效于 HTTP 状态 405。
        ///   指示请求方法 （POST 或 GET） 不允许对所请求的资源。
        /// </summary>
        [EkHttpStatusCodeDescription("请求方法 （POST 或 GET） 不允许对所请求的资源")]
        MethodNotAllowed = 405, // 0x00000195

        /// <summary>
        ///   等效于 HTTP 状态 406。
        ///   表示客户端已指定使用 Accept 标头，它将不接受任何可用的资源表示。
        /// </summary>
        [EkHttpStatusCodeDescription("客户端已指定使用 Accept 标头，它将不接受任何可用的资源表示")]
        NotAcceptable = 406, // 0x00000196

        /// <summary>
        ///   等效于 HTTP 状态 407。
        ///   指示请求的代理要求身份验证。
        ///    代理服务器进行身份验证标头包含如何执行身份验证的详细信息。
        /// </summary>
        [EkHttpStatusCodeDescription("请求的代理要求身份验证,代理服务器进行身份验证标头包含如何执行身份验证的详细信息")]
        ProxyAuthenticationRequired = 407, // 0x00000197

        /// <summary>
        ///   等效于 HTTP 状态 408。
        ///   指示客户端在服务器预期请求的时间内没有未发送请求。
        /// </summary>
        [EkHttpStatusCodeDescription("客户端在服务器预期请求的时间内没有未发送请求")]
        RequestTimeout = 408, // 0x00000198

        /// <summary>
        ///   等效于 HTTP 状态 409。
        ///   指示该请求可能不会执行由于在服务器上发生冲突。
        /// </summary>
        [EkHttpStatusCodeDescription("该请求可能不会执行由于在服务器上发生冲突")]
        Conflict = 409, // 0x00000199

        /// <summary>
        ///   等效于 HTTP 状态 410。
        ///   指示所请求的资源不再可用。
        /// </summary>
        [EkHttpStatusCodeDescription("所请求的资源不再可用")]
        Gone = 410, // 0x0000019A

        /// <summary>
        ///   等效于 HTTP 状态 411。
        ///   指示缺少必需的内容长度标头。
        /// </summary>
        [EkHttpStatusCodeDescription("缺少必需的内容长度标头")]
        LengthRequired = 411, // 0x0000019B

        /// <summary>
        ///   等效于 HTTP 状态 412。
        ///   表示失败，此请求的设置的条件，无法执行请求。
        ///    使用条件请求标头，如果匹配项，如设置条件无-If-match，或如果-修改-自从。
        /// </summary>
        [EkHttpStatusCodeDescription("失败，此请求设置的条件，无法执行请求")]
        PreconditionFailed = 412, // 0x0000019C

        /// <summary>
        ///   等效于 HTTP 状态 413。
        ///   指示请求太大服务器不能能够处理。
        /// </summary>
        [EkHttpStatusCodeDescription("请求太大服务器不能能够处理")]
        RequestEntityTooLarge = 413, // 0x0000019D

        /// <summary>
        ///   等效于 HTTP 状态 414。
        ///   指示 URI 太长。
        /// </summary>
        [EkHttpStatusCodeDescription("URI 太长")]
        RequestUriTooLong = 414, // 0x0000019E

        /// <summary>
        ///   等效于 HTTP 状态 415。
        ///   指示该请求是不受支持的类型。
        /// </summary>
        [EkHttpStatusCodeDescription("该请求是不受支持的类型")]
        UnsupportedMediaType = 415, // 0x0000019F

        /// <summary>
        ///   等效于 HTTP 416 状态。
        ///   指示从资源请求的数据范围不能返回，或者因为范围的开始处，然后该资源的开头或范围的末尾后在资源的结尾。
        /// </summary>
        [EkHttpStatusCodeDescription("所请求的范围无法满足 ")]
        RequestedRangeNotSatisfiable = 416, // 0x000001A0

        /// <summary>
        ///   等效于 HTTP 状态 417。
        ///   指示无法由服务器满足 Expect 标头中给定。
        /// </summary>
        [EkHttpStatusCodeDescription("无法由服务器满足 Expect 标头中给定 ")]
        ExpectationFailed = 417, // 0x000001A1

        /// <summary>
        ///   等效于 HTTP 状态 426。
        ///   指示客户端应切换到不同的协议，例如 TLS/1.0。
        /// </summary>
        [EkHttpStatusCodeDescription("客户端应切换到不同的协议，例如 TLS/1.0")]
        UpgradeRequired = 426, // 0x000001AA

        /// <summary>
        ///   等效于 HTTP 状态 500。
        ///   表示在服务器上发生一般性错误。
        /// </summary>
        [EkHttpStatusCodeDescription("服务器发生错误")]
        InternalServerError = 500, // 0x000001F4

        /// <summary>
        ///   等效于 HTTP 状态 501。
        ///   指示服务器不支持所请求的功能。
        /// </summary>
        [EkHttpStatusCodeDescription("服务器不支持所请求的功能")]
        NotImplemented = 501, // 0x000001F5

        /// <summary>
        ///   等效于 HTTP 状态 502。
        ///   指示中间代理服务器从另一个代理或原始服务器接收到错误响应。
        /// </summary>
        [EkHttpStatusCodeDescription("中间代理服务器从另一个代理或原始服务器接收到错误响应")]
        BadGateway = 502, // 0x000001F6

        /// <summary>
        ///   等效于 HTTP 状态 503。
        ///   指示将服务器暂时不可用，通常是由于高负载或维护。
        /// </summary>
        [EkHttpStatusCodeDescription("服务器暂时不可用")]
        ServiceUnavailable = 503, // 0x000001F7

        /// <summary>
        ///   等效于 HTTP 状态 504。
        ///   表示扮演网关或者代理的服务器无法在规定的时间内获得想要的响应。
        /// </summary>
        [EkHttpStatusCodeDescription("响应超时")] GatewayTimeout = 504, // 0x000001F8

        /// <summary>
        ///   等效于 HTTP 状态 505。
        ///   指示服务器不支持请求的 HTTP 版本。
        /// </summary>
        [EkHttpStatusCodeDescription("服务器不支持请求的 HTTP 版本")]
        HttpVersionNotSupported = 505, // 0x000001F9
    }
}

