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
    public class DebugManager : ICore
    {
        public static DebugManager Instance;
        public void ICroeInit()
        {
            Instance = this;
            //主动日志
            ACDebug.InitSettings(new LogConfig()
            {
                enableSave = true,
                loggerType = LoggerType.Unity,
#if !UNITY_EDITOR
                //savePath = $"{Application.persistentDataPath}/LogOut/ActiveLog/",
#endif
                savePath = $"{Application.dataPath}/LogOut/ActiveLog/",
                saveName = "Debug主动输出日志.txt",
            });
            //被动日志
            SystemExceptionDebug.InitSystemExceptionDebug();
            //屏幕显示日志
            //GameObject debugGo = new GameObject("DebugGo");
            //debugGo.AddComponent<Debugger>();
            //GameObject.DontDestroyOnLoad(debugGo);
            ACDebug.Log("日志模块初始化完毕!");
        }
    }
}
