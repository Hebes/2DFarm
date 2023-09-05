using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
    请遵守命名规范

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
            WriteData("Prefab", string.Empty, ".prefab");
        }

        [MenuItem("Tool/GenerateConfig/生成UIPanel配置文件")]//#E
        public static void GenerateUIPanelConfig()
        {
            WriteData("UIPanel", string.Empty, ".prefab");
        }

        [MenuItem("Tool/GenerateConfig/生成Scenes配置文件")]//#E
        public static void GenerateScenesConfig()
        {
            WriteData("Scenes", string.Empty, ".unity");
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
            WriteData("ConfigData", string.Empty, ".bytes");
        }

        [MenuItem("Tool/GenerateConfig/生成Sprites配置文件")]//#E
        public static void GenerateSpritesConfig()
        {
            WriteData("Sprites", string.Empty, ".png");
        }

        [MenuItem("Tool/GenerateConfig/生成Animations配置文件")]//#E
        public static void GenerateAnimationsConfig()
        {
            WriteData("Animations", string.Empty, ".overrideController");
        }

        [MenuItem("Tool/GenerateConfig/生成Effects配置文件")]//#E
        public static void GenerateEffectsConfig()
        {
            WriteData("Effects",string.Empty, ".prefab");
        }

        [MenuItem("Tool/GenerateConfig/生成Sound配置文件")]//#E
        public static void GenerateSoundConfig()
        {
            WriteData("Sound", string.Empty, ".wav", ".ogg");
        }

        /// <summary>
        /// 写入内容 
        /// </summary>
        /// <param name="dirName">文件夹名称兼文件名称</param>
        /// <param name="filterSuffix">过滤后缀</param>
        /// <param name="classPath"></param>
        /// <param name="sb"></param>
        private static void WriteData(string dirName,string ValueSuffix, params string[] filterSuffix)
        {
            string Path = $"{CommonPath}{dirName}/";
            List<string> stringsTemp = new List<string>();
            foreach (var item in filterSuffix)
            {
                string[] strings = Directory.GetFiles(Path, searchPattern: $"*{item}", SearchOption.AllDirectories);
                stringsTemp.AddRange(strings.ToList());
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine($"    public class Config{dirName}\r\n    {{");
            foreach (string s in stringsTemp)
            {
                string sTemp = s.Replace("\\", "/");
                string[] strPath = sTemp.Split('/');
                string[] fileNameTemp = strPath[strPath.Length - 1].Split('.');

                //过滤文件名称的特殊符号和执行后缀名
                string OldfileName = fileNameTemp[0].
                    Replace($"*.{filterSuffix}", "");
                string fileName = fileNameTemp[0].
                    Replace($"*.{filterSuffix}", "").
                    Replace("@", "_").
                    Replace("(", "").
                    Replace(")", "").
                    Replace("-", "_").
                    Replace(" ", "");

                string fileSuffix = fileNameTemp[1];
                //首字母大写
                //string fileSuffixTemp = $"{char.ToUpper(fileSuffix[0])}{fileSuffix.Substring(startIndex: 1, fileSuffix.Length - 1)}";

                sb.AppendLine($"        public const string {fileName}{ValueSuffix} = \"{OldfileName}\";");//{fileSuffixTemp}
            }
            sb.AppendLine("    }\r\n}");

            string classPath = $"{Application.dataPath}/HotUpdate/GameMain/Config/Common/Config{dirName}.cs";
            string classPathMeta = $"{Application.dataPath}/HotUpdate/GameMain/Config/Common/Config{dirName}{filterSuffix}.meta";

            if (File.Exists(classPath))
            {
                Debug.Log("文件存在开始删除!");
                File.Delete(classPath);
                File.Delete(classPathMeta);
            }

            File.WriteAllText(classPath, sb.ToString());
            AssetDatabase.Refresh();
        }
    }
}
