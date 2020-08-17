
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

        #region  Get 请求


        /// <summary>
        /// 使用Get方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> GetAsync(string url, Dictionary<string, string> header = null,
                                                   bool Gzip = false, Action<string> callBack = null)
        {
            string responseBody;
            try
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
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();//用来抛异常的

                if (Gzip)
                {
                    GZipInputStream inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
                    responseBody = await new StreamReader(inputStream).ReadToEndAsync();
                }
                else
                {
                    responseBody = await response.Content.ReadAsStringAsync();

                }

            }
            catch (Exception e)
            {

                responseBody = e.Message;
            }
            callBack?.Invoke(responseBody);

            return responseBody;
        }

        /// <summary>
        /// 使用Get方法异步请求，并返回反序列化对象
        /// </summary>
        /// <typeparam name="T">反序列化对象</typeparam>
        /// <param name="url">请求路径</param>
        /// <param name="header"></param>
        /// <param name="Gzip"></param>
        /// <returns>返回反序列化对象</returns>
        public static async Task<T> GetAsync<T>(string url, Dictionary<string, string> header = null,
                                                    bool Gzip = false)
            where T : class
        {
            string responseBody = await GetAsync(url, header, Gzip);
            return string.IsNullOrEmpty(responseBody) ? null : responseBody.FromJson<T>();
        }


        #endregion


        #region  Post 请求

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="json">发送的参数字符串，只能用json</param>
        /// <param name="contentType">Http请求的数据提交方式(默认 "application/json" )</param>
        /// <returns>返回的字符串</returns>
        //public static async Task<string> PostAsync(string url, string json,
        //    EkChangeContentType contentType = EkChangeContentType.ApplicationJson)
        //{
        //    string responseBody;
        //    try
        //    {
        //        HttpClient client = HttpClientFactory.Create();
        //        HttpContent content = CreateHttpContent(json, contentType);
        //        HttpResponseMessage response = await client.PostAsync(url, content);
        //        response.EnsureSuccessStatusCode();

        //        responseBody = await response.Content.ReadAsStringAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        responseBody = e.Message;
        //    }

        //    return responseBody;
        //}

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="data">发送的参数字符串</param>
        /// <param name="contentType">Http请求的数据提交方式(默认 "application/json" )</param>
        /// <param name="header"></param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> PostAsync(string url, string data,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null, bool Gzip = false)
        {
            HttpClient client = HttpClientFactory.Create(new HttpClientHandler() { UseCookies = false });
            HttpContent content = CreateHttpContent(data, contentType);

            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody;
            if (Gzip)
            {
                GZipInputStream inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
                responseBody = await new StreamReader(inputStream).ReadToEndAsync();
            }
            else
            {
                responseBody = await response.Content.ReadAsStringAsync();

            }
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
        /// <returns>返回的字符串</returns>
        public static async Task<string> PostAsync<T>(string url, T postObj,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null, bool gzip = false)
            where T : class
        {
            string jsonStr = postObj.ToJson(); ;//将postObj序列化为json字符串
            if (string.IsNullOrEmpty(jsonStr))
                return null;
            return await PostAsync(url, jsonStr, contentType, header, gzip);
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
        /// <returns>返回请求信息反序列化的对象</returns>
        public static async Task<T2> PostAsync<T1, T2>(string url, T1 obj,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null, bool gzip = false)
            where T1 : class
            where T2 : class
        {
            string json = obj.ToJson();
            string responseBody = await PostAsync(url, json, contentType, header, gzip); //请求当前信息
            return string.IsNullOrEmpty(responseBody) ? default : responseBody.FromJson<T2>();
        }

        #endregion


        #region  Put 请求

        /// <summary>
        /// Put 请求
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="putData">请求数据(json)</param>
        /// <param name="contentType">Http请求的数据提交方式(默认 "application/json" )</param>
        /// <param name="header"></param>
        /// <returns>返回请求结果信息</returns>
        public static async Task<string> PutAsync(string url, string putData,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null)
        {
            string result = string.Empty;
            HttpClient httpClient = HttpClientFactory.Create();
            HttpContent httpContent = CreateHttpContent(putData, contentType);
            if (header != null)
            {
                httpClient.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            HttpResponseMessage response = await httpClient.PutAsync(url, httpContent);
            // statusCode = response.StatusCode.ToString();//状态码
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }


            return result;
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
        /// <returns>返回请求结果信息</returns>
        public static async Task<string> PutAsync<T>(string url, T obj,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null)
            where T : class
        {
            string jsonData = obj.ToJson();
            string result = await PutAsync(url, jsonData, contentType, header);
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
        /// <returns>返回反序列化后的结果</returns>
        public static async Task<T2> PutAsync<T1, T2>(string url, T1 obj,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null)
            where T1 : class
            where T2 : class
        {
            string jsonData = obj.ToJson();
            string result = await PutAsync(url, jsonData, contentType, header);
            return result.FromJson<T2>();
        }

        #endregion

        #region  Delete 请求  ----暂时不能用

        public static async Task DeleteAsync(string url)
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
        private static HttpContent CreateHttpContent(string stringContent, EkChangeContentType contentType = EkChangeContentType.ApplicationJson)
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
}
