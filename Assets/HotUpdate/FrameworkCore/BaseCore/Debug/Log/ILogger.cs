/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    日志接口

-----------------------*/

namespace Core
{
    public interface ILogger
    {
        public void Log(string msg, LogCoLor LogCoLor = LogCoLor.None);//普通信息
        public void Warn(string msg);                                  //警告
        public void Error(string msg);                                 //异常错误
    }
}
