/**************************************************************************
*   
*   =================================
*   CLR 版本    ：4.0.30319.42000
*   命名空间    ：IceInk
*   文件名称    ：EkSerializeHelper.cs
*   =================================
*   创 建 者    ：IceInk
*   创建日期    ：2020/7/10 17:00:33 
*   功能描述    ：
*           提供 序列化 反序列化的帮助
*           json 
*           xml 
*   使用说明    ：
*       需要安装NuGet包
*           Newtonsoft.Json  用于序列化和反序列化
*   =================================
*  
***************************************************************************/


using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace IceInk
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public static class EkSerializeHelper
    {
        /// <summary>
        /// 序列化为json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="referenceLoopHandling">默认移除对象之间循环引用，避免序列化出错</param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj,
            ReferenceLoopHandling referenceLoopHandling = ReferenceLoopHandling.Ignore) where T : class
        {
            //移除循环引用
            var setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = referenceLoopHandling
            };

            return JsonConvert.SerializeObject(obj, setting);
        }

        /// <summary>
        /// 将json反序列化为实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns>返回反序列化后的实体类，如果失败则抛异常l</returns>
        public static T FromJson<T>(this string json) where T : class
        {
            T fromT;
            try
            {
                fromT = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return fromT;
        }

        /// <summary>
        /// 序列化并保存json文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="path">保存路径</param>
        /// <returns></returns>
        public static string SaveJson<T>(this T obj, string path) where T : class
        {
            var jsonContent = obj.ToJson();
            File.WriteAllText(path, jsonContent);
            return jsonContent;
        }

        /// <summary>
        /// 加载json文件并烦序列化为实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">json文件路径</param>
        /// <returns></returns>
        public static T LoadJson<T>(this string path) where T : class
        {
            using (var streamReader = new StreamReader(path))
            {
                return FromJson<T>(streamReader.ReadToEnd());
            }
        }

        public static bool SerializeBinary(string path, object obj)
        {
            if (string.IsNullOrEmpty(path))
                // Log.W("SerializeBinary Without Valid Path.");
                return false;

            if (obj == null)
                //Log.W("SerializeBinary obj is Null.");
                return false;

            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                var bf =
                    new BinaryFormatter();
                bf.Serialize(fs, obj);
                return true;
            }
        }


        public static object DeserializeBinary(Stream stream)
        {
            if (stream == null)
                //  Log.W("DeserializeBinary Failed!");
                return null;

            using (stream)
            {
                var bf =
                    new BinaryFormatter();
                var data = bf.Deserialize(stream);

                // TODO:这里没风险嘛?
                return data;
            }
        }

        public static object DeserializeBinary(string path)
        {
            if (string.IsNullOrEmpty(path))
                // Log.W("DeserializeBinary Without Valid Path.");
                return null;

            var fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
                //  Log.W("DeserializeBinary File Not Exit.");
                return null;

            using (var fs = fileInfo.OpenRead())
            {
                var bf =
                    new BinaryFormatter();
                var data = bf.Deserialize(fs);

                if (data != null) return data;
            }

            // Log.W("DeserializeBinary Failed:" + path);
            return null;
        }

        public static bool SerializeXML(string path, object obj)
        {
            if (string.IsNullOrEmpty(path))
                // Log.W("SerializeBinary Without Valid Path.");
                return false;

            if (obj == null)
                // Log.W("SerializeBinary obj is Null.");
                return false;

            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                var xmlserializer = new XmlSerializer(obj.GetType());
                xmlserializer.Serialize(fs, obj);
                return true;
            }
        }

        public static object DeserializeXML<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
                //  Log.W("DeserializeBinary Without Valid Path.");
                return null;

            var fileInfo = new FileInfo(path);

            using (var fs = fileInfo.OpenRead())
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var data = xmlserializer.Deserialize(fs);

                if (data != null) return data;
            }

            // Log.W("DeserializeBinary Failed:" + path);
            return null;
        }
    }
}