using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
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
        private static string CommonPath = $"{Application.dataPath}\\AssetsPackage\\";
        private static string GenerateConfigPath = $"{Application.dataPath}/HotUpdate/GameMain/Config/Common/";


        [MenuItem("Tool/GenerateConfig/生成Prefab配置文件")]//#E
        public static void GeneratePrefabConfig()
        {
            string Path = $"{CommonPath}Prefab\\";
            string[] strings = Directory.GetFiles(Path, "*.prefab", SearchOption.AllDirectories);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigPrefab\r\n    {");

            foreach (string s in strings)
            {
                string[] strPath = s.Split('\\');
                string tempstr = strPath[strPath.Length - 1].Replace(".prefab", "");
                sb.AppendLine($"        public const string {tempstr}Prefab = \"{tempstr}\";");
            }
            sb.AppendLine("    }\r\n}");
            string classPath = $"{GenerateConfigPath}ConfigPrefab.cs";
            if (File.Exists(classPath))
                File.Delete(classPath);
            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        [MenuItem("Tool/GenerateConfig/生成UIPanel配置文件")]//#E
        public static void GenerateUIPanelConfig()
        {
            string Path = $"{CommonPath}UIPanel/";
            string[] strings = Directory.GetFiles(Path, "*.prefab", SearchOption.AllDirectories);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigUIPanel\r\n    {");

            foreach (string s in strings)
            {
                string[] strPath = s.Split('\\');
                string tempstr = strPath[strPath.Length - 1].Replace(".prefab", "");
                sb.AppendLine($"        public const string {tempstr}Panel = \"{tempstr}\";");
            }
            sb.AppendLine("    }\r\n}");
            string classPath = $"{GenerateConfigPath}ConfigUIPanel.cs";
            if (File.Exists(classPath))
                File.Delete(classPath);
            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        [MenuItem("Tool/GenerateConfig/生成Scenes配置文件")]//#E
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
            string classPath = $"{GenerateConfigPath}ConfigScenes.cs";
            if (File.Exists(classPath))
                File.Delete(classPath);
            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        [MenuItem("Tool/GenerateConfig/生成Tag配置文件")]//#E
        public static void GenerateTagConfig()
        {
            var tags = InternalEditorUtility.tags;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigTag\r\n    {");

            foreach (string s in tags)
            {
                string tempstr = s;
                sb.AppendLine($"        public const string Tag{tempstr} = \"{tempstr}\";");
            }
            sb.AppendLine("    }\r\n}");
            string classPath = $"{GenerateConfigPath}ConfigTag.cs";
            if (File.Exists(classPath))
                File.Delete(classPath);
            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        [MenuItem("Tool/GenerateConfig/生成Layer配置文件")]//#E
        public static void GenerateLayerConfig()
        {
            var tags = InternalEditorUtility.layers;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigLayer\r\n    {");

            foreach (string s in tags)
            {
                string tempstr = s;
                sb.AppendLine($"        public const string Layer{tempstr.Replace(" ", "").Trim()} = \"{tempstr}\";");
            }
            sb.AppendLine("    }\r\n}");
            string classPath = $"{GenerateConfigPath}ConfigLayer.cs";
            if (File.Exists(classPath))
            {
                Debug.Log("文件存在开始删除!");
                File.Delete(classPath);
            }

            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        [MenuItem("Tool/GenerateConfig/生成SortingLayer配置文件")]//#E
        public static void GenerateSortingLayerConfig()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            string[] sortingLayers = (string[])sortingLayersProperty.GetValue(null, new object[0]);


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigSortingLayer\r\n    {");

            foreach (string s in sortingLayers)
            {
                string tempstr = s;
                sb.AppendLine($"        public const string SortingLayer{tempstr.Replace(" ", "").Trim()} = \"{tempstr}\";");
            }
            sb.AppendLine("    }\r\n}");
            string classPath = $"{GenerateConfigPath}ConfigSortingLayer.cs";
            if (File.Exists(classPath))
            {
                Debug.Log("文件存在开始删除!");
                File.Delete(classPath);
            }

            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        [MenuItem("Tool/GenerateConfig/生成bytes配置文件")]//#E
        public static void GeneratebytesConfig()
        {
            string Path = $"{CommonPath}BinaryData/";
            string[] strings = Directory.GetFiles(Path, "*.bytes", SearchOption.AllDirectories);


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigBytes\r\n    {");

            foreach (string s in strings)
            {
                string tempstr = s.Replace(Path, "").Replace(".bytes", "");
                sb.AppendLine($"        public const string Bytes{tempstr} = \"{tempstr}\";");
            }
            sb.AppendLine("    }\r\n}");
            string classPath = $"{GenerateConfigPath}ConfigBytes.cs";
            if (File.Exists(classPath))
            {
                Debug.Log("文件存在开始删除!");
                File.Delete(classPath);
            }

            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        [MenuItem("Tool/GenerateConfig/生成Sprites配置文件")]//#E
        public static void GenerateSpritesConfig()
        {
            string Path = $"{CommonPath}Sprites/";
            string[] strings = Directory.GetFiles(Path, "*.png", SearchOption.AllDirectories);


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigSprites\r\n    {");

            foreach (string s in strings)
            {
                string[] strPath = s.Split('\\');
                string tempstr = strPath[strPath.Length - 1].Replace(".png", "").Replace("@", "_").Replace("-", "_").Replace(" ", "");
                string tempstr1 = strPath[strPath.Length - 1].Replace(".png", "");
                sb.AppendLine($"        public const string Sprites{tempstr} = \"{tempstr1}\";");
            }
            sb.AppendLine("    }\r\n}");
            string classPath = $"{GenerateConfigPath}ConfigSprites.cs";
            if (File.Exists(classPath))
            {
                Debug.Log("文件存在开始删除!");
                File.Delete(classPath);
            }

            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        [MenuItem("Tool/GenerateConfig/生成Animations配置文件")]//#E
        public static void GenerateAnimationsConfig()
        {
            string Path = $"{CommonPath}Animations/";
            string[] strings = Directory.GetFiles(Path, "*.overrideController", SearchOption.AllDirectories);


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigAnimations\r\n    {");

            foreach (string s in strings)
            {
                string[] strPath = s.Split('\\');
                string tempstr = strPath[strPath.Length - 1].Replace(".overrideController", "").Replace("@", "_").Replace("-", "_").Replace(" ", "");
                string tempstr1 = strPath[strPath.Length - 1].Replace(".overrideController", "");
                sb.AppendLine($"        public const string Animations{tempstr} = \"{tempstr1}\";");
            }
            sb.AppendLine("    }\r\n}");
            string classPath = $"{GenerateConfigPath}ConfigAnimations.cs";
            if (File.Exists(classPath))
            {
                Debug.Log("文件存在开始删除!");
                File.Delete(classPath);
            }

            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }

    }
}
