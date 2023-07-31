using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ACFrameworkCore
{
    internal class RunBat
    {
        [MenuItem("Tool/执行bat")]
        private static void Run()
        {
            string path = Application.dataPath.Replace("Assets","");
            //// 执行bat脚本
            //RunMyBat("http资源包测试启动.bat", path);

            string cmd = "/c http资源包测试启动.bat /path:\"{0}\" /closeonend 2";
            //var path = Application.dataPath + "/../";
            cmd = string.Format(cmd, path);
            //UnityEngine.Debug.LogError(cmd);
            ProcessStartInfo proc = new ProcessStartInfo("cmd.exe", cmd);
            proc.WindowStyle = ProcessWindowStyle.Normal;
            Process.Start(proc);
        }
    }
}
