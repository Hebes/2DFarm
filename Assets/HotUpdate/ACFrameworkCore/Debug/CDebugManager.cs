using System;
using System.IO;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    日志模块

-----------------------*/


namespace ACFrameworkCore
{
    public class CDebugManager : ICore
    {
        public void ICroeInit()
        {
            //InitiativeLog();
            PassivityLog();
            DLog.Log("日志模块初始化完毕!");
        }

        private string path { get; set; }

        /// <summary>
        /// 主动日志
        /// </summary>
        public void InitiativeLog()
        {
            bool isLogPrint = PlayerPrefs.GetInt("设置日志开启") == 0;

            //主动日志模块
            DLog.InitSettings(new LogConfig()
            {
                enableSave = isLogPrint,
                eLoggerType = LoggerType.Unity,
#if !UNITY_EDITOR
                //savePath = $"{Application.persistentDataPath}/LogOut/ActiveLog/",
#endif
                savePath = $"{Application.dataPath}/LogOut/ActiveLog/",
                saveName = "Debug主动输出日志.txt",
            });
        }
        //被动消息
        public void PassivityLog()
        {
            //path = $"{Application.dataPath}/LogOut/PassiveLog/";
            path = $"{Application.dataPath}/LogOut/PassiveLog/";// "Assets/LogOut/PassiveLog";
            DLog.Log($"被动日志输出路径：{path}");
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
        private void Handler(string logString, string stackTrace, LogType type)
        {
            if (type != LogType.Error || type != LogType.Exception || type != LogType.Assert) return;
            //UnityEngine.Debug.Log("显示堆栈调用：" + new System.Diagnostics.StackTrace().ToString());
            //UnityEngine.Debug.Log("接收到异常信息" + logString);
            string logPath = Path.Combine(path, $"Passive_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.txt");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.AppendAllText(logPath, "==============================================\r\n");
            File.AppendAllText(logPath, "[时间]:" + DateTime.Now.ToString() + "\r\n");
            File.AppendAllText(logPath, "[类型]:" + type.ToString() + "\r\n");
            File.AppendAllText(logPath, "[报错信息]:" + logString + "\r\n");
            File.AppendAllText(logPath, "[堆栈跟踪]:" + stackTrace + "\r\n");
        }
    }
}
