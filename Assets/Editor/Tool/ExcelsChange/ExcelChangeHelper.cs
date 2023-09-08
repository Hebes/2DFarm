//using Excel;
using System.Data;
using System.IO;
using UnityEditor;
using ExcelDataReader;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    表格转换帮助类

-----------------------*/

namespace ACFrameworkCore
{
    public static class ExcelChangeHelper
    {
        /// <summary>
        /// 读取Excel数据并保存为字符串锯齿数组
        // </summary>
        public static string[][] LoadExcel(this string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            DataSet dataSet = fileInfo.Extension == ".xlsx"
                ? ExcelReaderFactory.CreateOpenXmlReader(stream).AsDataSet()
                : ExcelReaderFactory.CreateBinaryReader(stream).AsDataSet();

            DataRowCollection rows = dataSet.Tables[0].Rows;
            string[][] data = new string[rows.Count][];
            for (int i = 0; i < rows.Count; ++i)
            {
                int columnCount = rows[i].ItemArray.Length;
                string[] columnArray = new string[columnCount];
                for (int j = 0; j < columnArray.Length; ++j)
                    columnArray[j] = rows[i].ItemArray[j].ToString();
                data[i] = columnArray;
            }
            stream.Close();

            return data;
        }

        /// <summary>
        /// 生成文件夹
        /// </summary>
        /// <param name="path">生成的路径</param>
        public static void GenerateDirectory(this string path)
        {
            //if (Directory.Exists(path))
            //    Directory.Delete(path, true);
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
    }
}
