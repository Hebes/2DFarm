using System.IO;
using System.Text;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据结构类

-----------------------*/

namespace ACFrameworkCore
{
    public class ClassData
    {
        private static string BinaryDataPath = $"{Application.dataPath}/HotUpdate/GameMain/ExcelData/ClassConfigs/";
        private static string DataClassPath = $"{Application.dataPath}/Excel2Script/Script/";


        /// <summary>
        /// 通过Excel数据生成脚本文件
        /// </summary>
        public static void CreateScript(string filePath, string[][] data)
        {
            StringBuilder sb = new StringBuilder();
            string className = new FileInfo(filePath).Name.Split('.')[0];
            sb.AppendLine("using System.Collections.Generic;\n");
            sb.AppendLine($"public class {className}");
            sb.AppendLine("{");
            string[] filedTypeArray = data[(int)RowType.FIELD_TYPE];
            string[] filedNameArray = data[(int)RowType.FIELD_NAME];
            for (int i = 0; i < filedTypeArray.Length; ++i)
            {
                sb.AppendLine($"\tpublic {filedTypeArray[i].PadRight(10, ' ')}\t{filedNameArray[i]};");
            }

            sb.AppendLine("}");
            DataClassPath.GenerateDirectory();
            string path = $"{DataClassPath}/{className}.cs";
            File.Delete(path);
            File.WriteAllText(path, sb.ToString());
        }
    }
}
