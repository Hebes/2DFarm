using System;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    日志配置

-----------------------*/

namespace Core
{
    /// <summary>
    /// 日志配置
    /// </summary>
    public class LogConfig
    {
        public bool enableLog = true;   //启用日志
        public string LogPrefix = "#";  //日志前缀
        public bool enableTime = true;  //启用时间
        public string LogSeparate = ">>";//日志分离
        public bool enableThreadID = true;//启用线程ID
        public bool enableTrace = true;//启用跟踪
        public bool enableSave = true;//启用保存
        public bool enableCover = false;//日志覆盖
        public string _savePath;//保存路径
        public string saveName = "PELog.txt";//保存名称
        public LoggerType loggerType = LoggerType.Unity;//日志类型

        public string savePath
        {
            get
            {
                if (_savePath == null)
                {
                    switch (loggerType)
                    {
                        case LoggerType.Unity:
                            Type type = Type.GetType("UnityEngine.Application, UnityEngine");
                            _savePath = type.GetProperty("persistentDataPath").GetValue(null).ToString() + "/PELog/";
                            break;
                        case LoggerType.Console:
                            _savePath = string.Format($"{AppDomain.CurrentDomain.BaseDirectory}Logs\\");
                            break;
                    }
                }
                return _savePath;
            }
            set
            {
                _savePath = value;
            }
        }
    }
}
