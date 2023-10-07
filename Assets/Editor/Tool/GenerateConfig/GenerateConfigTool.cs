using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using Debug = UnityEngine.Debug;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    生成配置文件
    请遵守命名规范

-----------------------*/

namespace ACEditor
{
    public enum DataReadType
    {
        /// <summary> 普通带后缀 </summary>
        CommonSuffixation,
        /// <summary> 普通不带后缀 </summary>
        CommonNoSuffix,
        /// <summary> 全路径带后缀 </summary>
        AllPathSuffixation,
        /// <summary> 全路径不带后缀 </summary>
        AllPathNoSuffix,
    }

    public class GenerateConfigTool : EditorWindow
    {
        private static string namespaceName = "Farm2D";    //命名空间

        private static string FilterKeyword(string str, params string[] filterSuffix)
        {
            foreach (string key in filterSuffix)
                str = str.Replace(key, "");
            return str;
        }

        public static void WriteData(string content, string creatFilePath)
        {
            //删除原来的文件
            if (File.Exists(creatFilePath))
            {
                Debug.Log("文件存在开始删除!");
                File.Delete(creatFilePath);
                Debug.Log("文件删除成功!");
            }
            File.WriteAllText(creatFilePath, content.ToString());
            Debug.Log("文件写入成功!");
            AssetDatabase.Refresh();
        }

        public static string ReadDataString(string path, string ConfigName, DataReadType dataReadType, params string[] filterSuffix)
        {
            //寻找需要的文件
            List<string> pathsList = new List<string>();
            foreach (string key in filterSuffix)
            {
                string[] strings = Directory.GetFiles(path, $"*{key}", SearchOption.AllDirectories);
                pathsList.AddRange(strings.ToList());
            }
            //拼接字符串
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"namespace {namespaceName}\r\n{{");
            sb.AppendLine($"    public class {ConfigName}\r\n    {{");
            foreach (string pathTemp in pathsList)
            {
                //文件名称
                string oldFileName = Path.GetFileNameWithoutExtension(pathTemp);
                string fileName = Path.GetFileNameWithoutExtension(pathTemp).
                    Replace("@", "_").
                    Replace("(", "").
                    Replace(")", "").
                    Replace("-", "_").
                    Replace(" ", "");
                //文件路径
                string extendedName = Path.GetExtension(pathTemp);//如不需要请直接添加上去,这个是获取拓展名称
                string assetsPath = pathTemp.Replace(path, "").Replace("\\", "/");//文件路径
                string extendedNameTemp = FilterKeyword(extendedName, ".");
                switch (dataReadType)
                {
                    case DataReadType.CommonSuffixation:
                        sb.AppendLine($"        public const string {extendedNameTemp}{fileName} = \"{oldFileName}{extendedName}\";");
                        break;
                    case DataReadType.CommonNoSuffix:
                        sb.AppendLine($"        public const string {extendedNameTemp}{fileName} = \"{oldFileName}\";");
                        break;
                    case DataReadType.AllPathSuffixation:
                        sb.AppendLine($"        public const string {extendedNameTemp}{fileName} = \"{assetsPath}\";");
                        break;
                    case DataReadType.AllPathNoSuffix:
                        sb.AppendLine($"        public const string {extendedNameTemp}{fileName} = \"{assetsPath.Replace(extendedName, "")}\";");
                        break;
                }

                //文件路径

            }
            sb.AppendLine("    }\r\n}");
            return sb.ToString();
        }

        public static string ReadTagData()
        {
            string[] tags = InternalEditorUtility.tags;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ACFrameworkCore\r\n{");
            sb.AppendLine("    public class ConfigTag\r\n    {");
            foreach (string s in tags)
                sb.AppendLine($"        public const string Tag{s} = \"{s}\";");
            sb.AppendLine("    }\r\n}");
            return sb.ToString();
        }

        public static string ReadLayerData()
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
            return sb.ToString();
        }

        public static string ReadSortingLayerData()
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
            return sb.ToString();
        }
    }
}
