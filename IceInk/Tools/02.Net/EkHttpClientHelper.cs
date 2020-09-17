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
using System.Net.Http.Headers;
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
        private static readonly HttpClient mHttpClient = new HttpClient
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

        #region Delete 请求  ----不能用

        public static async Task DeleteAsync(string url, string jsonContext)
        {
            var httpClient = HttpClientFactory.Create();
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
            content.Headers.ContentType = new MediaTypeHeaderValue(EkHttpContentType.ContentType(contentType))
            {
                CharSet = "utf-8"
            };
            return content;
        }

        public static string CommonHttpRequest(string data, string serviceUrl, string type)
        {
            //构造http请求的对象
            var myRequest = (HttpWebRequest) WebRequest.Create(serviceUrl);
            //转成网络流
            var buf = Encoding.GetEncoding("UTF-8").GetBytes(data);
            //设置
            myRequest.Method = type;
            myRequest.ContentLength = buf.Length;
            myRequest.ContentType = "application/json";
            myRequest.MaximumAutomaticRedirections = 1;
            myRequest.AllowAutoRedirect = true;
            // 发送请求
            var newStream = myRequest.GetRequestStream();
            newStream.Write(buf, 0, buf.Length);
            newStream.Close();
            // 获得接口返回值
            var myResponse = (HttpWebResponse) myRequest.GetResponse();
            var reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            var result = reader.ReadToEnd();
            reader.Close();
            myResponse.Close();
            return result;
        }


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
            var client = HttpClientFactory.Create(new HttpClientHandler {UseCookies = false});
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header) client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            HttpResponseMessage response = null;
            var isOk = false;
            string responseBody;
            try
            {
                response = await client.GetAsync(url);
                isOk = response.IsSuccessStatusCode;

                if (gzip)
                {
                    var inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
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
            var responseBody = await GetAsync(url, header, gzip, contentType, callBackAction);
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
            var client = HttpClientFactory.Create(new HttpClientHandler
            {
                UseCookies = false
            });
            //client.Timeout = TimeSpan.FromSeconds(10);
            var content = CreateHttpContent(data, contentType);
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header) client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            HttpResponseMessage response = null;
            var isOk = false;
            string responseBody;
            try
            {
                response = await client.PostAsync(url, content);
                isOk = response.IsSuccessStatusCode;

                if (gzip)
                {
                    var inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
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
            var jsonStr = postObj.ToJson(); //将postObj序列化为json字符串
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
        /// <param name="header">请求头</param>
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
            var json = obj.ToJson();
            var responseBody = await PostAsync(url, json, contentType, header, gzip, callBackAction); //请求当前信息
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
        /// ///
        /// <param name="callbackAction">请求回调</param>
        /// <returns>返回请求结果信息</returns>
        public static async Task<string> PutAsync(string url, string putData,
            EkChangeContentType contentType = EkChangeContentType.ApplicationJson,
            Dictionary<string, string> header = null,
            Action<bool, string> callbackAction = null)
        {
            var httpClient = HttpClientFactory.Create();
            var httpContent = CreateHttpContent(putData, contentType);
            HttpResponseMessage response = null;
            var isOk = false;
            string responseBody;
            if (header != null)
            {
                httpClient.DefaultRequestHeaders.Clear();
                foreach (var item in header) httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
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
        /// 自动将类型转换为json文件
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
            var jsonData = obj.ToJson();
            var result = await PutAsync(url, jsonData, contentType, header, callbackAction);
            return result;
        }

        /// <summary>
        /// Put请求。
        /// 自动将输入类型序列化为json文件。
        /// 并且把请求返回数据反序列化为对应的类型。
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
            var jsonData = obj.ToJson();
            var result = await PutAsync(url, jsonData, contentType, header, callbackAction);
            return result.FromJson<T2>();
        }

        #endregion

        #region 获取返回状态码的描述

        /// <summary>
        /// 获取状态码的描述
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetHttpStatusCodeDes(int status)
        {
            if (!Enum.IsDefined(typeof(EkHttpStatusCode), status))
                return string.Format("{0}", status);
            var statusCode = (EkHttpStatusCode) status;
            var type = statusCode.GetType();
            var fileInfo = type.GetField(statusCode.ToString());
            if (!fileInfo.IsDefined(typeof(EkHttpStatusCodeDescriptionAttribute)))
                return string.Format("{0}", status);

            var statusCodeDescription = fileInfo.GetCustomAttribute<EkHttpStatusCodeDescriptionAttribute>();
            return string.Format("{0} , ：{1}", status, statusCodeDescription.Description);
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
                var type = statusCode.GetType();
                var fileInfo = type.GetField(statusCode.ToString());
                if (!fileInfo.IsDefined(typeof(EkHttpStatusCodeDescriptionAttribute)))
                    return string.Format("{0} , {1}", status, (int) statusCode);

                var statusCodeDescription = fileInfo.GetCustomAttribute<EkHttpStatusCodeDescriptionAttribute>();
                return string.Format("{0} , {1} ：{2}", status, (int) statusCode,
                    statusCodeDescription.Description);
            }

            return status;
        }

        #endregion
    }
}