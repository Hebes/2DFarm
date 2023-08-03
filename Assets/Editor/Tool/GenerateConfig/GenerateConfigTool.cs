using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    生成配置文件

-----------------------*/

namespace ACFrameworkCore
{
    public class GenerateConfigTool : EditorWindow
    {
        private static string CommonPath = $"{Application.dataPath}/AssetsPackage/";

        [MenuItem("Tool/生成Prefab配置文件")]//#E
        public static void GeneratePrefabConfig()
        {
            string Path = $"{CommonPath}Prefab/";
            string[] strings = Directory.GetFiles(Path, "*.prefab", SearchOption.AllDirectories);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigPrefab\r\n    {");

            foreach (string s in strings)
            {
                string tempstr = s.Replace(Path, "").Replace(".prefab", "");
                sb.AppendLine($"        public const string {tempstr}Prefab = \"{tempstr}\";");
            }
            sb.AppendLine("    }\r\n}");
            string classPath = $"{Application.dataPath}/HotUpdate/GameMain/Config/PrefabConfig.cs";
            if (File.Exists(classPath))
                File.Delete(classPath);
            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        [MenuItem("Tool/生成UIPanel配置文件")]//#E
        public static void GenerateUIPanelConfig()
        {
            string Path = $"{CommonPath}UIPanel/";
            string[] strings = Directory.GetFiles(Path, "*.prefab", SearchOption.AllDirectories);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigUIPanel\r\n    {");

            foreach (string s in strings)
            {
                string tempstr = s.Replace(Path, "").Replace(".prefab", "");
                sb.AppendLine($"        public const string {tempstr}Panel = \"{tempstr}\";");
            }
            sb.AppendLine("    }\r\n}");
            string classPath = $"{Application.dataPath}/HotUpdate/GameMain/Config/UIPanelConfig.cs";
            if (File.Exists(classPath))
                File.Delete(classPath);
            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        [MenuItem("Tool/生成Scenes配置文件")]//#E
        public static void GenerateScenesConfig()
        {
            string Path = $"{CommonPath}Scenes/";
            string[] strings = Directory.GetFiles(Path, "*.unity", SearchOption.AllDirectories);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigScenes\r\n    {");

            foreach (string s in strings)
            {
                string tempstr = s.Replace(Path, "").Replace(".unity", "");
                sb.AppendLine($"        public const string {tempstr}Scenes = \"{tempstr}\";");
            }
            sb.AppendLine("    }\r\n}");
            string classPath = $"{Application.dataPath}/HotUpdate/GameMain/Config/ScenesConfig.cs";
            if (File.Exists(classPath))
                File.Delete(classPath);
            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }
    }
}
