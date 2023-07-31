using System.IO;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    清空日志

-----------------------*/

namespace ACFrameworkCore
{
    internal class LogTool : EditorWindow
    {
        public static string LogPath = $"{Application.dataPath}/LogOut";

        [MenuItem("Tool/清空日志")]//#E
        public static void ClearLog()
        {
            if (!Directory.Exists(LogPath)) return;
            File.Delete($"{LogPath}.meta");
            Directory.Delete(LogPath, true );
            AssetDatabase.Refresh();
        }
    }
}
