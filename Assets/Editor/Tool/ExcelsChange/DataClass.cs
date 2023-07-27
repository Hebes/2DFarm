using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
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
    public class DataClass
    {
        private static string BinaryDataPath = $"{Application.dataPath}/HotUpdate/GameMain/ExcelData/ClassConfigs/";

        /// <summary>
        /// 生成数据结构类
        /// </summary>
        /// <param name="table"></param>
        public static void GenerateExcelDataClass(DataTable table) 
        {
            //字段名行
            DataRow rowName = table.GetRowValue(ExcelConfig.startRowName);
            //字段类型行
            DataRow rowType = table.GetRowValue(ExcelConfig.startRowType);
            //字段描述
            DataRow rowDescribe = table.GetRowValue(ExcelConfig.startRowDescribe);
            //生成文件夹路径
            BinaryDataPath.GenerateDirectory();
            //生成对应的数据结构类脚本
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("/*--------脚本描述-----------\r\n\t\t\t\t\r\n电子邮箱：\r\n\t1607388033@qq.com\r\n作者:\r\n\t暗沉\r\n描述:\r\n\r\n\r\n-----------------------*/");
            sb.AppendLine($"public class {table.TableName}\n{{");
            //变量字符串拼接 列
            for (int i = ExcelConfig.startColumns; i < table.Columns.Count; i++)
                sb.AppendLine($"    /// <summary>\r\n    /// {rowDescribe[i]}\r\n    /// </summary>\r\n    public {rowType[i]} {rowName[i]};\r\n");
            sb.AppendLine("}");
            //把拼接好的字符串存到指定文件中去
            File.WriteAllText($"{BinaryDataPath}{table.TableName}.cs", sb.ToString());
            //刷新Project窗口
            AssetDatabase.Refresh();
        }
    }
}
