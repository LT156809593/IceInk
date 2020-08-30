/**************************************************************************
*   
*   =================================
*   CLR 版本    ：4.0.30319.42000
*   命名空间    ：IceInk
*   文件名称    ：EkHttpClientHelper.cs
*   =================================
*   创 建 者    ：IceInk
*   创建日期    ：2020/7/9 20:40:10 
*   功能描述    ：
*       HttpClient
*              Get Post Put Delete 的封装
*              目前仅支持 "application/json" 请求方式
*   使用说明    ：
*           需要引用 通过NuGet搜索安装即可
*               Microsoft.AspNet.WebApi.Client 使用HttpClientFactory来创建HttpClient,
*               SharpZipLib  压缩
*
*
*               EkSerializeHelper 序列化帮助类(自定义的如果不需要则把序列化和反序列化去掉)
*   =================================
*  
***************************************************************************/


using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;

namespace IceInk
{
    /// <summary>
    /// 基于HttpClient封装的 帮助类
    /// </summary>
    public static class EkHttpClientHelper
    {
        //为了避免创建大量的HttpClient而出现的性能问题，
        //可以使用HttpClientFactory.Create()创建HttpClient。需要包Microsoft.AspNet.WebApi.Client
        private static readonly HttpClient mHttpClient = new HttpClient()
        {
            Timeout = new TimeSpan(0, 0, 10),
            DefaultRequestHeaders =
            {
                Connection =
                {
                    "keep-alive"
                }
            }
        };


        #region Get 请求

        /// <summary>
        /// 使用Get方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="header"></param>
        /// <param name="gzip"></param>
        /// <param name="contentType">Http请求数据提交类型</param>
        /// <param name="callBackAction">请求回调</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> GetAsync(string url, Dictionary<string, string> header = null,
            bool gzip = false, EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Action<bool, string> callBackAction = null)
        {
            HttpClient client = HttpClientFactory.Create(new HttpClientHandler() { UseCookies = false });
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            HttpResponseMessage response = null;
            bool isOk = false;
            string responseBody;
            try
            {
                response = await client.GetAsync(url);
                isOk = response.IsSuccessStatusCode;

                if (gzip)
                {
                    GZipInputStream inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
                    responseBody = await new StreamReader(inputStream).ReadToEndAsync();
                }
                else
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }

                if (string.IsNullOrEmpty(responseBody))
                    responseBody = GetHttpStatusCodeDes(response.StatusCode.ToString());
                // response.EnsureSuccessStatusCode();//用来抛异常的
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    responseBody = e.InnerException.Message;
                else if (response != null)
                    responseBody = GetHttpStatusCodeDes(response.StatusCode.ToString());
                else
                    responseBody = e.Message;
            }

            callBackAction?.Invoke(isOk, responseBody);

            return responseBody;
        }

        /// <summary>
        /// 使用Get方法异步请求，并返回反序列化对象
        /// </summary>
        /// <typeparam name="T">反序列化对象</typeparam>
        /// <param name="url">请求路径</param>
        /// <param name="header"></param>
        /// <param name="gzip"></param>
        /// <param name="contentType"></param>
        /// <param name="callBackAction"></param>
        /// <returns>返回反序列化对象</returns>
        public static async Task<T> GetAsync<T>(string url, Dictionary<string, string> header = null,
            bool gzip = false, EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Action<bool, string> callBackAction = null)
            where T : class
        {
            string responseBody = await GetAsync(url, header, gzip, contentType, callBackAction);
            return string.IsNullOrEmpty(responseBody) ? null : responseBody.FromJson<T>();
        }

        #endregion


        #region Post 请求

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="data">发送的参数字符串</param>
        /// <param name="contentType">Http请求的数据提交方式(默认 "application/json" )</param>
        /// <param name="header">相应头</param>
        /// <param name="gzip"></param>
        /// <param name="callBackAction">请求回调</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> PostAsync(string url, string data,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null, bool gzip = false,
            Action<bool, string> callBackAction = null)
        {
            HttpClient client = HttpClientFactory.Create(new HttpClientHandler()
            {
                UseCookies = false,
            });
            //client.Timeout = TimeSpan.FromSeconds(10);
            HttpContent content = CreateHttpContent(data, contentType);
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            HttpResponseMessage response = null;
            bool isOk = false;
            string responseBody = string.Empty;
            try
            {
                response = await client.PostAsync(url, content);
                isOk = response.IsSuccessStatusCode;

                if (gzip)
                {
                    GZipInputStream inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
                    responseBody = await new StreamReader(inputStream).ReadToEndAsync();
                }
                else
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }

                if (string.IsNullOrEmpty(responseBody))
                    responseBody = GetHttpStatusCodeDes(response.StatusCode.ToString());
                //  response.EnsureSuccessStatusCode();//抛异常用的
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    responseBody = e.InnerException.Message;
                else if (response != null)
                    responseBody = GetHttpStatusCodeDes(response.StatusCode.ToString());
                else
                    responseBody = e.Message;
            }

            callBackAction?.Invoke(isOk, responseBody);
            return responseBody;
        }

        /// <summary>
        /// 使用Post方法异步请求
        /// </summary>
        /// <typeparam name="T">请求对象类型,将会被序列化为Json字符串</typeparam>
        /// <param name="url">请求链接</param>
        /// <param name="postObj">请求对象</param>
        /// <param name="contentType">Http请求的数据提交方式(默认 "application/json" )</param>
        /// <param name="header">请求头</param>
        /// <param name="gzip">压缩</param>
        /// <param name="callBackAction">请求回调</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> PostAsync<T>(string url, T postObj,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null, bool gzip = false,
            Action<bool, string> callBackAction = null)
            where T : class
        {
            string jsonStr = postObj.ToJson(); //将postObj序列化为json字符串
            if (string.IsNullOrEmpty(jsonStr))
                return null;
            return await PostAsync(url, jsonStr, contentType, header, gzip, callBackAction);
        }

        /// <summary>
        /// 使用post返回异步请求直接返回对象
        /// </summary>
        /// <typeparam name="T1">请求对象类型</typeparam>
        /// <typeparam name="T2">返回对象类型</typeparam>
        /// <param name="url">请求链接</param>
        /// <param name="obj">请求对象数据</param>
        /// <param name="contentType">Http请求的数据提交方式(默认 "application/json" )</param>
        ///  <param name="header">请求头</param>
        /// <param name="gzip">压缩</param>
        /// <param name="callBackAction">请求回调</param>
        /// <returns>返回请求信息反序列化的对象</returns>
        public static async Task<T2> PostAsync<T1, T2>(string url, T1 obj,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null, bool gzip = false,
            Action<bool, string> callBackAction = null)
            where T1 : class
            where T2 : class
        {
            string json = obj.ToJson();
            string responseBody = await PostAsync(url, json, contentType, header, gzip, callBackAction); //请求当前信息
            return string.IsNullOrEmpty(responseBody) ? default : responseBody.FromJson<T2>();
        }

        #endregion


        #region Put 请求

        /// <summary>
        /// Put 请求
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="putData">请求数据(json)</param>
        /// <param name="contentType">Http请求的数据提交方式(默认 "application/json" )</param>
        /// <param name="header"></param>
        /// /// <param name="callbackAction">请求回调</param>
        /// <returns>返回请求结果信息</returns>
        public static async Task<string> PutAsync(string url, string putData,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null,
            Action<bool, string> callbackAction = null)
        {
            HttpClient httpClient = HttpClientFactory.Create();
            HttpContent httpContent = CreateHttpContent(putData, contentType);
            HttpResponseMessage response = null;
            bool isOk = false;
            string responseBody;
            if (header != null)
            {
                httpClient.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }


            try
            {
                response = await httpClient.PutAsync(url, httpContent);
                isOk = response.IsSuccessStatusCode;
                responseBody = response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(responseBody))
                    responseBody = GetHttpStatusCodeDes(response.StatusCode.ToString());
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    responseBody = e.InnerException.Message;
                else if (response != null)
                    responseBody = GetHttpStatusCodeDes(response.StatusCode.ToString());
                else
                    responseBody = e.Message;
            }

            callbackAction?.Invoke(isOk, responseBody);
            return responseBody;
        }

        /// <summary>
        /// Put请求
        ///  自动将类型转换为json文件
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="url">请求路径</param>
        /// <param name="obj"></param>
        /// <param name="contentType">Http请求的数据提交方式(默认 "application/json" )</param>
        /// <param name="header"></param>
        /// <param name="callbackAction">请求回调函数</param>
        /// <returns>返回请求结果信息</returns>
        public static async Task<string> PutAsync<T>(string url, T obj,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null,
            Action<bool, string> callbackAction = null)
            where T : class
        {
            string jsonData = obj.ToJson();
            string result = await PutAsync(url, jsonData, contentType, header, callbackAction);
            return result;
        }

        /// <summary>
        /// Put请求。
        ///     自动将输入类型序列化为json文件。
        ///     并且把请求返回数据反序列化为对应的类型。
        /// </summary>
        /// <typeparam name="T1">输入类型</typeparam>
        /// <typeparam name="T2">反序列化类型</typeparam>
        /// <param name="url">请求路径</param>
        /// <param name="obj">输入类型对象</param>
        /// <param name="contentType">Http请求的数据提交方式(默认 "application/json" )</param>
        /// <param name="header"></param>
        /// <param name="callbackAction">请求回调函数</param>
        /// <returns>返回反序列化后的结果</returns>
        public static async Task<T2> PutAsync<T1, T2>(string url, T1 obj,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null,
            Action<bool, string> callbackAction = null)
            where T1 : class
            where T2 : class
        {
            string jsonData = obj.ToJson();
            string result = await PutAsync(url, jsonData, contentType, header, callbackAction);
            return result.FromJson<T2>();
        }

        #endregion

        #region Delete 请求  ----不能用

        public static async Task DeleteAsync(string url, string jsonContext)
        {
            HttpClient httpClient = HttpClientFactory.Create();
            await httpClient.DeleteAsync(url);
        }

        #endregion

        /// <summary>
        /// 创建一个HttpContent
        /// </summary>
        /// <param name="stringContent"></param>
        /// <param name="contentType">Http请求的数据提交方式(默认 "application/json" )</param>
        /// <returns></returns>
        private static HttpContent CreateHttpContent(string stringContent,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson)
        {
            HttpContent content = new StringContent(stringContent);
            content.Headers.ContentType = new System.Net.Http.Headers
                .MediaTypeHeaderValue(EkHttpContentType.ContentType(contentType))
            {
                CharSet = "utf-8"
            };
            return content;
        }

        public static string CommonHttpRequest(string data, string serviceUrl, string type)
        {
            //构造http请求的对象
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
            //转成网络流
            byte[] buf = Encoding.GetEncoding("UTF-8").GetBytes(data);
            //设置
            myRequest.Method = type;
            myRequest.ContentLength = buf.Length;
            myRequest.ContentType = "application/json";
            myRequest.MaximumAutomaticRedirections = 1;
            myRequest.AllowAutoRedirect = true;
            // 发送请求
            Stream newStream = myRequest.GetRequestStream();
            newStream.Write(buf, 0, buf.Length);
            newStream.Close();
            // 获得接口返回值
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            reader.Close();
            myResponse.Close();
            return result;
        }


        /// <summary>
        /// 获取状态码的描述
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetHttpStatusCodeDes(int status)
        {
            if (!Enum.IsDefined(typeof(EkHttpStatusCode), status))
                return string.Format("{0}", status);
            EkHttpStatusCode statusCode = (EkHttpStatusCode)status;
            Type type = statusCode.GetType();
            FieldInfo fileInfo = type.GetField(statusCode.ToString());
            if (!fileInfo.IsDefined(typeof(EkHttpStatusCodeDescriptionAttribute)))
                return string.Format("{0}", status);

            var statusCodeDescription = fileInfo.GetCustomAttribute<EkHttpStatusCodeDescriptionAttribute>();
            return string.Format(format: "{0} , ：{1}", status, statusCodeDescription.Description);
        }

        /// <summary>
        /// 获取状态码的描述
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetHttpStatusCodeDes(string status)
        {
            if (!Enum.IsDefined(typeof(EkHttpStatusCode), status))
                return status;
            if (Enum.TryParse(status, out EkHttpStatusCode statusCode))
            {
                Type type = statusCode.GetType();
                FieldInfo fileInfo = type.GetField(statusCode.ToString());
                if (!fileInfo.IsDefined(typeof(EkHttpStatusCodeDescriptionAttribute)))
                    return string.Format(format: "{0} , {1}", status, (int)statusCode);

                var statusCodeDescription = fileInfo.GetCustomAttribute<EkHttpStatusCodeDescriptionAttribute>();
                return string.Format(format: "{0} , {1} ：{2}", status, (int)statusCode,
                    statusCodeDescription.Description);
            }

            return status;
        }
    }


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