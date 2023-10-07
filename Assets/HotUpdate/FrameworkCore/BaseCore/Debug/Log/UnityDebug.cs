using System;
/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    UnityDebug

-----------------------*/

namespace Core
{
    public class UnityDebug: ILogger
    {
        private Type type = Type.GetType("UnityEngine.Debug, UnityEngine"); //反射获取Unity的日志输出系统

        public void Log(string msg, LogCoLor LogCoLor)
        {
            if (LogCoLor != LogCoLor.None)
                msg = ColorUnityLog(msg, LogCoLor);
            type.GetMethod("Log", new Type[] { typeof(object) })?.Invoke(null, new object[] { msg });
        }
        public void Warn(string msg)
        {
            type.GetMethod("LogWarning", new Type[] { typeof(object) })?.Invoke(null, new object[] { msg });//反射执行方法
        }
        public void Error(string msg)
        {
            type.GetMethod("LogError", new Type[] { typeof(object) })?.Invoke(null, new object[] { msg });
        }

        private string ColorUnityLog(string msg, LogCoLor color)
        {
            switch (color)
            {
                default:
                case LogCoLor.None: msg = string.Format($"<coLor=#FF000O>{msg}</coLor>"); break;
                case LogCoLor.DarkRed: msg = string.Format($"<coLor=#FF000O>{msg}</coLor>"); break;
                case LogCoLor.Green: msg = string.Format($"<coLor=#00FF00>{msg}</coLor>"); break;
                case LogCoLor.Blue: msg = string.Format($"<coLor=#0000FF>{msg}</coLor>"); break;
                case LogCoLor.Cyan: msg = string.Format($"<coLor=#00FFFF>{msg}</coLor>"); break;
                case LogCoLor.Magenta: msg = string.Format($"<coLor=#FF00FF>{msg}</coLor>"); break;
                case LogCoLor.DarkYellow: msg = string.Format($"<coLor=#FFff0O>{msg}</coLor>"); break;
            }
            return msg;
        }
    }
}
