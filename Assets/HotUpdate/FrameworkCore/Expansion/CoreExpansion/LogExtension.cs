namespace Core
{
    public static class LogExtension
    {
        //打印日志
        public static void Log(this object obj, string Log, params object[] args)
        {
            Debug.Log(string.Format(Log, args));
        }
        public static void Log(this object obj, object Log)
        {
            Debug.Log(Log);
        }
        public static void Log(this object obj, LogCoLor logCoLorEnum, string Log, params object[] args)
        {
            Debug.Log(logCoLorEnum, string.Format(Log, args));
        }
        public static void Log(this object obj, LogCoLor logCoLorEnum, object Log)
        {
            Debug.Log(logCoLorEnum, Log);
        }

        //打印堆栈
        public static void Trace(this object obj, string Log, params object[] args)
        {
            Debug.Trace(string.Format(Log, args));
        }
        public static void Trace(this object obj, object Log)
        {
            Debug.Trace(Log);
        }

        //打印警告日志
        public static void Warn(this object obj, string Log, params object[] args)
        {
            Debug.Warn(string.Format(Log, args));
        }
        public static void Warn(this object obj, object Log)
        {
            Debug.Warn(Log);
        }

        //打印错误日志
        public static void Error(this object obj, string Log, params object[] args)
        {
            Debug.Error(string.Format(Log, args));
        }
        public static void Error(this object obj, object Log)
        {
            Debug.Error(Log);
        }
    }
}
