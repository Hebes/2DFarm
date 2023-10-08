/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    Unity系统级别异常

-----------------------*/

using System;
using System.IO;
using UnityEngine;

namespace Core
{
    //系统级别配置错误配置文件
    public class SystemDebugConfig
    {
        public string SavePath = $"{Application.dataPath}/LogOut/PassiveLog/";
    }

    public class SystemExceptionDebug
    {
        private static SystemDebugConfig systemDebugConfig;//系统级别日志配置文件

        public static void InitSystemExceptionDebug(SystemDebugConfig systemDebugConfig = null)
        {
            if (systemDebugConfig == null)
                systemDebugConfig = SystemExceptionDebug.systemDebugConfig = new SystemDebugConfig();
            else
                systemDebugConfig = SystemExceptionDebug.systemDebugConfig = systemDebugConfig;
            Application.logMessageReceived += Handler;
        }

        //private void OnDestroy()
        //{
        //    Application.logMessageReceived -= Handler;
        //}

        /// <summary>
        /// 被动日志
        /// </summary>
        /// <param name="logString"></param>
        /// <param name="stackTrace"></param>
        /// <param name="type"></param>
        private static void Handler(string logString, string stackTrace, LogType type)
        {
            if (type != LogType.Error || type != LogType.Exception || type != LogType.Assert) return;
            UnityEngine.Debug.Log("显示堆栈调用：" + new System.Diagnostics.StackTrace().ToString());
            UnityEngine.Debug.Log("接收到异常信息" + logString);
            string Time = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            //string logPath = Path.Combine(systemDebugConfig.SavePath, $"Passive_{Time}.txt");
            string logPath = $"{systemDebugConfig.SavePath}Passive_{Time}.txt";

            if (!Directory.Exists(systemDebugConfig.SavePath))
                Directory.CreateDirectory(systemDebugConfig.SavePath);

            File.AppendAllText(logPath, $"==============================================\r\n");
            File.AppendAllText(logPath, $"[时间]:{Time}\r\n");
            File.AppendAllText(logPath, $"[类型]:{type}\r\n");
            File.AppendAllText(logPath, $"[报错信息]:{logString}\r\n");
            File.AppendAllText(logPath, $"[堆栈跟踪]:{stackTrace}\r\n");
            Debug.Log($"被动日志生成路径:{logPath}");
        }
    }
}
