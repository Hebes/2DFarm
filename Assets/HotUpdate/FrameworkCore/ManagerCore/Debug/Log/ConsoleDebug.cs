/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    服务器Debug

-----------------------*/

using System;

namespace Core
{
    public class ConsoleDebug : ILogger
    {
        public void Log(string msg, LogCoLor logCoLor = LogCoLor.None)
        {
            WriteConsoleLog(msg, logCoLor);
        }
        public void Warn(string msg)
        {
            WriteConsoleLog(msg, LogCoLor.DarkYellow);
        }
        public void Error(string msg)
        {
            WriteConsoleLog(msg, LogCoLor.DarkRed);
        }

        private void WriteConsoleLog(string msg, LogCoLor color)
        {
            switch (color)
            {
                default:
                case LogCoLor.None:
                    Console.WriteLine(msg);
                    break;
                case LogCoLor.DarkRed:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogCoLor.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogCoLor.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogCoLor.Cyan:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogCoLor.Magenta:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogCoLor.DarkYellow:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
        }
    }
}
