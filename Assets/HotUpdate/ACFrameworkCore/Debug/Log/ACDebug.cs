using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    日志总控

-----------------------*/

namespace ACFrameworkCore
{
    public class ACDebug
    {
        public static LogConfig cfg;//配置文件
        private static ILogger logger;//输出的日志类型
        private static StreamWriter LogFiLeWriter = null;//输出流
        private const string logLock = "PELogLock";//日志锁

        //初始化
        public static void InitSettings(LogConfig cfg = null)
        {
            cfg = ACDebug.cfg = cfg == null ? new LogConfig() : cfg;
            //日志类型
            switch (ACDebug.cfg.loggerType)
            {
                case LoggerType.Unity: logger = new UnityDebug(); break;
                case LoggerType.Console: logger = new ConsoleDebug(); break;
            }
            //是否启用日志
            if (cfg.enableSave == false) return;
            //日志覆盖
            if (cfg.enableCover)//覆盖
            {
                string path = $"{cfg.savePath}{cfg.saveName}";
                try
                {
                    if (Directory.Exists(cfg.savePath))//存在这个路径
                    {
                        if (File.Exists(path))//存在这个文件
                            File.Delete(path);
                    }
                    else
                    {
                        Directory.CreateDirectory(cfg.savePath);
                    }
                    LogFiLeWriter = File.AppendText(path);
                    LogFiLeWriter.AutoFlush = true;
                }
                catch (Exception) { LogFiLeWriter = null; }
            }
            else
            {
                string prefix = DateTime.Now.ToString("yyyyMMdd@HH-mm-s");
                string path = $"{cfg.savePath}{prefix}{cfg.saveName}";
                try
                {
                    logger.Log("主动日志的输出路径为：" + path);
                    if (Directory.Exists(cfg.savePath) == false)
                        Directory.CreateDirectory(cfg.savePath);
                    LogFiLeWriter = File.AppendText(path);
                    LogFiLeWriter.AutoFlush = true;
                }
                catch (Exception) { LogFiLeWriter = null; }
            }
        }

        //日志
        public static void Log(string msg, params object[] args)
        {
            if (cfg.enableLog == false) return;
            msg = DecorateLog(string.Format(msg, args));
            lock (logLock)
            {
                logger.Log(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[L]{msg}"));
            }
        }
        public static void Log(object obj)
        {
            if (cfg.enableLog == false)                return;
            string msg = DecorateLog(obj.ToString());
            lock (logLock)
            {
                logger.Log(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[L]{msg}"));
            }
        }
        public static void Log(LogCoLor logCoLorEnum, string msg, params object[] args)
        {
            if (cfg.enableLog == false)
                return;
            msg = DecorateLog(string.Format(msg, args));
            lock (logLock)
            {
                logger.Log(msg, logCoLorEnum);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[L]{msg}"));
            }
        }
        public static void Log(LogCoLor logCoLorEnum, object obj)
        {
            if (cfg.enableLog == false)
                return;
            string msg = DecorateLog(obj.ToString());
            lock (logLock)
            {
                logger.Log(msg, logCoLorEnum);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[L]{msg}"));
            }
        }

        //打印堆栈
        public static void Trace(string msg, params object[] args)
        {
            if (cfg.enableLog == false)
                return;
            msg = DecorateLog(string.Format(msg, args), true);
            lock (logLock)
            {
                logger.Log(msg, LogCoLor.Magenta);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[T]{msg}"));
            }
        }
        public static void Trace(object obj)
        {
            if (cfg.enableLog == false)
                return;
            string msg = DecorateLog(obj.ToString());
            lock (logLock)
            {
                logger.Log(msg, LogCoLor.Magenta, true);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[T]{msg}"));
            }
        }

        //打印警告日志
        public static void Warn(string msg, params object[] args)
        {
            if (cfg.enableLog == false)
                return;
            msg = DecorateLog(string.Format(msg, args));
            lock (logLock)
            {
                logger.Warn(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[W]{msg}"));
            }
        }
        public static void Warn(object obj)
        {
            if (cfg.enableLog == false)
                return;
            string msg = DecorateLog(obj.ToString());
            lock (logLock)
            {
                logger.Warn(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[W]{msg}"));
            }
        }

        //打印错误日志
        public static void Error(string msg, params object[] args)
        {
            if (cfg.enableLog == false)
                return;
            msg = DecorateLog(string.Format(msg, args), true);
            lock (logLock)
            {
                logger.Error(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[E]{msg}"));
            }
        }
        public static void Error(object obj)
        {
            if (cfg.enableLog == false)
                return;
            string msg = DecorateLog(obj.ToString(), true);
            lock (logLock)
            {
                logger.Error(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[E]{msg}"));
            }
        }

        #region Tool
        //日志打印
        private static string DecorateLog(string msg, bool isTrace = false)
        {
            StringBuilder sb = new StringBuilder(cfg.LogPrefix, 100);
            if (cfg.enableTime)//启用时间
                sb.AppendFormat($"时间:{DateTime.Now.ToString("hh:mm:ss--fff")}");
            if (cfg.enableThreadID)//启用线程
                sb.AppendFormat($"{GetThreadID()}");
            sb.AppendFormat($" {cfg.LogSeparate} {msg}");//日志分离
            if (isTrace)//是否追踪日志 堆栈
                sb.AppendFormat($" \nStackTrace:{GetLogTrace()}");
            return sb.ToString();
        }

        //获取日志追踪
        private static string GetLogTrace()
        {
            StackTrace st = new StackTrace(3, true);//3 跳跃3帧 true-> 获取场下文信息
            string traceInfo = string.Empty;
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                traceInfo += string.Format($"\n\t{sf.GetFileName()}::{sf.GetMethod()}line:{sf.GetFileLineNumber()}");
                //traceInfo += string.Format($"\n\t{sf.GetFileName()}::\n\t{sf.GetMethod()}\tline:{sf.GetFileLineNumber()}");
                //traceInfo += string.Format($"\n\t脚本:{sf.GetFileName()}::方法{sf.GetMethod()}行: {sf.GetFileLineNumber()}");
            }
            return traceInfo;
        }

        //获取线程Id
        private static object GetThreadID()
        {
            return string.Format($" ThreadID:{Thread.CurrentThread.ManagedThreadId}");
        }

        //日志写入文件
        private static void WriteToFile(string msg)
        {
            if (cfg.enableSave && LogFiLeWriter != null)
            {
                try
                {
                    LogFiLeWriter.WriteLine(msg);
                }
                catch
                {
                    LogFiLeWriter = null;
                    return;
                }
            }
        }

        //打印数组数据For Debug
        public static void PrintBytesArray(byte[] bytes, string prefix, Action<string> printer = null)
        {
            string str = prefix + "->\n";
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 10 == 0)
                {
                    str += bytes[i] + "\n";
                }
                str += bytes[i] + " ";
            }
            if (printer != null)
            {
                printer(str);
            }
            else
            {
                Log(str);
            }
        }
        #endregion
    }
}
