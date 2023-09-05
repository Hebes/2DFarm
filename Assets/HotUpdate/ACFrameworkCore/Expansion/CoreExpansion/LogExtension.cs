namespace ACFrameworkCore
{
    public static class LogExtension
    {
        //打印日志
        public static void Log(this object obj, string Log, params object[] args)
        {
            ACDebug.Log(string.Format(Log, args));
        }
        public static void Log(this object obj, object Log)
        {
            ACDebug.Log(Log);
        }
        public static void Log(this object obj, LogCoLor logCoLorEnum, string Log, params object[] args)
        {
            ACDebug.Log(logCoLorEnum, string.Format(Log, args));
        }
        public static void Log(this object obj, LogCoLor logCoLorEnum, object Log)
        {
            ACDebug.Log(logCoLorEnum, Log);
        }

        //打印堆栈
        public static void Trace(this object obj, string Log, params object[] args)
        {
            ACDebug.Trace(string.Format(Log, args));
        }
        public static void Trace(this object obj, object Log)
        {
            ACDebug.Trace(Log);
        }

        //打印警告日志
        public static void Warn(this object obj, string Log, params object[] args)
        {
            ACDebug.Warn(string.Format(Log, args));
        }
        public static void Warn(this object obj, object Log)
        {
            ACDebug.Warn(Log);
        }

        //打印错误日志
        public static void Error(this object obj, string Log, params object[] args)
        {
            ACDebug.Error(string.Format(Log, args));
        }
        public static void Error(this object obj, object Log)
        {
            ACDebug.Error(Log);
        }
    }
}
