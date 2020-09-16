#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：EkLogManager
// 创 建 者：作者名称
// 创建时间：2020/09/03 14:24:16
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		日志管理
//      默认日志放在当前应用程序运行目录下的logs文件夹中
//        
//        日志文件后缀为.log   日志文件名称为当天的日期+编号。比如 20200806(0).log 日期为2020年8月6日 编号为0；
//        每个日志文件大小不超过 1M
//        如果日志文件大小超过 1M。则会创建一个新的日志文件。编号自增1。
//----------------------------------------------------------------*/

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.DateTime;

namespace IceInk
{
    /// <summary>
    /// 日志组件
    /// 默认日志放在当前应用程序运行目录下的logs文件夹中
    /// </summary>
    public static class EkLogManager
    {
        /// <summary>
        /// 日志队列
        /// (日志路径，日志内容)
        /// </summary>
        private static readonly ConcurrentQueue<Tuple<string, string>> LogQueue =
            new ConcurrentQueue<Tuple<string, string>>();

        /// <summary>
        /// 自定义事件
        /// </summary>
        public static event Action<EkLogInfo> Event;

        /// <summary>
        /// 静态构造函数只会被执行一次
        /// </summary>
        static EkLogManager()
        {
            Task writeTask = new Task(obj =>
            {
                //开启线程写入日志
                while (true)
                {
                    Pause.WaitOne(1000, true); //阻塞当前线程执行
                    List<string[]> temp = new List<string[]>();//temp[0] 存放日志路径，temp[1]存放日志内容
                    foreach (var logItem in LogQueue)
                    {
                        string logPath = logItem.Item1;//日志路径
                        string logMergeContent = string.Concat(logItem.Item2, Environment.NewLine,
                            "----------------------------------------------------------------------------------------------------------------------",
                            Environment.NewLine);//日志内容
                        string[] logArr = temp.FirstOrDefault(d => d[0].Equals(logPath));
                        if (logArr != null)
                        {
                            logArr[1] = string.Concat(logArr[1], logMergeContent);
                        }
                        else
                        {
                            logArr = new[]
                            {
                                logPath,
                                logMergeContent
                            };
                            temp.Add(logArr);
                        }

                        LogQueue.TryDequeue(out Tuple<string, string> _);//出队列
                    }

                    //日志写入
                    foreach (var item in temp)
                    {
                        WriteText(item[0], item[1]);
                    }
                }
            }, null, TaskCreationOptions.LongRunning);
            writeTask.Start();
        }

        /// <summary>
        /// AutoResetEvent 常常被用来在两个线程之间进行信号发送
        /// </summary>
        private static AutoResetEvent Pause => new AutoResetEvent(false);

        /// <summary>
        /// 日志存放目录，默认日志放在当前应用程序运行目录下的logs文件夹中
        /// </summary>
        public static string LogDirectory
        {
            get => Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory).Any(s => s.Contains("Web.config"))
                ? AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Logs\"
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            set { }
        }

        #region 写入Info级别日志

        /// <summary>
        /// 写入Info级别的日志
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(info).ToUpper()}  {info}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Info,
                Message = info,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入Info级别的日志
        /// </summary>
        /// <param name="source"></param>
        /// <param name="info"></param>
        public static void Info(string source, string info)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(info).ToUpper()}   {source}  {info}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Info,
                Message = info,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入Info级别的日志
        /// </summary>
        /// <param name="source"></param>
        /// <param name="info"></param>
        public static void Info(Type source, string info)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(info).ToUpper()}   {source.FullName}  {info}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Info,
                Message = info,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source.FullName
            };
            Event?.Invoke(log);
        }

        #endregion


        #region 写入debug级别日志

        /// <summary>
        /// 写入debug级别日志
        /// </summary>
        /// <param name="debug">异常对象</param>
        public static void Debug(string debug)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(debug).ToUpper()}   {debug}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Debug,
                Message = debug,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入debug级别日志
        /// </summary>
        /// <param name="source">异常源的类型</param>
        /// <param name="debug">异常对象</param>
        public static void Debug(string source, string debug)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(debug).ToUpper()}   {source}  {debug}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Debug,
                Message = debug,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入debug级别日志
        /// </summary>
        /// <param name="source">异常源的类型</param>
        /// <param name="debug">异常对象</param>
        public static void Debug(Type source, string debug)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(debug).ToUpper()}   {source.FullName}  {debug}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Debug,
                Message = debug,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source.FullName
            };
            Event?.Invoke(log);
        }

        #endregion


        #region 写入error级别日志

        /// <summary>
        /// 写入error级别日志
        /// </summary>
        /// <param name="error">异常对象</param>
        public static void Error(Exception error)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(error).ToUpper()}   {error.Source}  {error.Message}{Environment.NewLine}{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(error).ToUpper()}   {error.Source}  {error.StackTrace}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Error,
                Message = error.Message,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = error.Source,
                Exception = error,
                ExceptionType = error.GetType().Name
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入error级别日志
        /// </summary>
        /// <param name="source">异常源的类型</param>
        /// <param name="error">异常对象</param>
        public static void Error(Type source, Exception error)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(error).ToUpper()}   {source.FullName}  {error.Message}{Environment.NewLine}{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(error).ToUpper()}   {source.FullName}  {error.StackTrace}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Error,
                Message = error.Message,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source.FullName,
                Exception = error,
                ExceptionType = error.GetType().Name
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入error级别日志
        /// </summary>
        /// <param name="source">异常源的类型</param>
        /// <param name="error">异常信息</param>
        public static void Error(Type source, string error)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(error).ToUpper()}   {source.FullName}  {error}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Error,
                Message = error,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source.FullName,
                //Exception = error,
                ExceptionType = error.GetType().Name
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入error级别日志
        /// </summary>
        /// <param name="source">异常源的类型</param>
        /// <param name="error">异常对象</param>
        public static void Error(string source, Exception error)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(error).ToUpper()}   {source}  {error.Message}{Environment.NewLine}{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(error).ToUpper()}   {source}  {error.StackTrace}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Error,
                Message = error.Message,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source,
                Exception = error,
                ExceptionType = error.GetType().Name
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入error级别日志
        /// </summary>
        /// <param name="source">异常源的类型</param>
        /// <param name="error">异常信息</param>
        public static void Error(string source, string error)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(error).ToUpper()}   {source}  {error}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Error,
                Message = error,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source,
                //Exception = error,
                ExceptionType = error.GetType().Name
            };
            Event?.Invoke(log);
        }

        #endregion


        #region 写入fatal级别日志

        /// <summary>
        /// 写入fatal级别日志
        /// </summary>
        /// <param name="fatal">异常对象</param>
        public static void Fatal(Exception fatal)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(fatal).ToUpper()}   {fatal.Source}  {fatal.Message}{Environment.NewLine}{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(fatal).ToUpper()}   {fatal.Source}  {fatal.StackTrace}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Fatal,
                Message = fatal.Message,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = fatal.Source,
                Exception = fatal,
                ExceptionType = fatal.GetType().Name
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入fatal级别日志
        /// </summary>
        /// <param name="source">异常源的类型</param>
        /// <param name="fatal">异常对象</param>
        public static void Fatal(Type source, Exception fatal)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(fatal).ToUpper()}   {source.FullName}  {fatal.Message}{Environment.NewLine}{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(fatal).ToUpper()}   {source.FullName}  {fatal.StackTrace}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Fatal,
                Message = fatal.Message,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source.FullName,
                Exception = fatal,
                ExceptionType = fatal.GetType().Name
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入fatal级别日志
        /// </summary>
        /// <param name="source">异常源的类型</param>
        /// <param name="fatal">异常对象</param>
        public static void Fatal(Type source, string fatal)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(fatal).ToUpper()}   {source.FullName}  {fatal}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Fatal,
                Message = fatal,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source.FullName,
                //Exception = fatal,
                ExceptionType = fatal.GetType().Name
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入fatal级别日志
        /// </summary>
        /// <param name="source">异常源的类型</param>
        /// <param name="fatal">异常对象</param>
        public static void Fatal(string source, Exception fatal)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(fatal).ToUpper()}   {source}  {fatal.Message}{Environment.NewLine}{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(fatal).ToUpper()}   {source}  {fatal.StackTrace}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Fatal,
                Message = fatal.Message,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source,
                Exception = fatal,
                ExceptionType = fatal.GetType().Name
            };
            Event?.Invoke(log);
        }

        /// <summary>
        /// 写入fatal级别日志
        /// </summary>
        /// <param name="source">异常源的类型</param>
        /// <param name="fatal">异常对象</param>
        public static void Fatal(string source, string fatal)
        {
            LogQueue.Enqueue(new Tuple<string, string>(GetLogPath(),
                $"{Now}   [{Thread.CurrentThread.ManagedThreadId}]   {nameof(fatal).ToUpper()}   {source}  {fatal}"));
            EkLogInfo log = new EkLogInfo()
            {
                EkLogLevel = EkLogLevel.Fatal,
                Message = fatal,
                Time = Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source,
                ExceptionType = fatal.GetType().Name
            };
            Event?.Invoke(log);
        }

        #endregion


        /// <summary>
        /// 日志文件大小
        /// </summary>
        private const int logFileInfoLen = 1 * 1024 * 1024;

        /// <summary>
        /// 获取Log日志文件路径
        /// </summary>
        /// <returns></returns>
        private static string GetLogPath()
        {
            string newFilePath;
            string logDir = string.IsNullOrEmpty(LogDirectory)
                ? Path.Combine(Environment.CurrentDirectory, "logs")
                : LogDirectory;
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            string extension = ".log"; //文件后缀 .log
            string fileNameNotExt = Now.ToString("yyyyMMdd");//文件名称 当天
            string fileNamePattern = string.Concat(fileNameNotExt, "(*)", extension);
            //搜索当前目录下日天为当天的所有日志文件路径
            List<string> filePaths =
                Directory.GetFiles(logDir, fileNamePattern, SearchOption.TopDirectoryOnly).ToList();

            if (filePaths.Count > 0)//如果有日志文件
            {
                int fileMaxLen = filePaths.Max(d => d.Length); //获取日志文件的最长文件名，
                string lastFilePath = filePaths.Where(d => d.Length == fileMaxLen)
                    .OrderByDescending(d => d)
                    .FirstOrDefault(); //从文件名最长的日志文件中找到最新创建的
                //判断日志文件大小是否超出规定大小
                if (new FileInfo(lastFilePath).Length > logFileInfoLen)//如果超出规定大小，则添加一个新的日志文件
                {
                    //获取日志文件编号 比如：20200716(10).log  则编号为 10；
                    string no = new Regex(@"(?is)(?<=\()(.*)(?=\))").Match(Path.GetFileName(lastFilePath)).Value;
                    bool parse = int.TryParse(no, out int tempno);//转换日志文件编号为int
                    string formatno = $"({(parse ? (tempno + 1) : tempno)})";//日志文件编号增加1
                    string newFileName = string.Concat(fileNameNotExt, formatno, extension);
                    newFilePath = Path.Combine(logDir, newFileName);//新日志文件路径
                }
                else
                {
                    newFilePath = lastFilePath;
                }
            }
            else
            {
                //没有日志文件，则创建新的文件路径
                string newFileName = string.Concat(fileNameNotExt, $"({0})", extension);
                newFilePath = Path.Combine(logDir, newFileName);
            }

            return newFilePath;
        }

        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="logPath">路径</param>
        /// <param name="logContent">写入内容</param>
        private static void WriteText(string logPath, string logContent)
        {
            try
            {
                if (!File.Exists(logPath))
                    File.CreateText(logPath).Close();
                using var sw = File.AppendText(logPath);
                sw.Write(logContent);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}