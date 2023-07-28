﻿using Excel;
using System.Data;
using System.IO;
using UnityEditor;

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
                {
                    columnArray[j] = rows[i].ItemArray[j].ToString();
                }

                data[i] = columnArray;
            }

            return data;
        }

        /// <summary>
        /// 生成文件夹
        /// </summary>
        /// <param name="path">生成的路径</param>
        public static void GenerateDirectory(this string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 获取变量名所在行
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rowsNumber">变量名所在行</param>
        /// <returns></returns>
        public static DataRow GetVariableNameRow(this DataTable table, int nameRowNumber)
        {
            return table.Rows[nameRowNumber];
        }

        /// <summary>
        /// 获取行值
        /// </summary>
        /// <param name="table"></param>
        /// <param name="nameRowNumber"></param>
        /// <returns></returns>
        public static DataRow GetRowValue(this DataTable table, int RowNumber)
        {
            return table.Rows[RowNumber];
        }

        /// <summary>
        /// 获取变量类型所在行
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static DataRow GetVariableTypeRow(this DataTable table, int typeRowNumber)
        {
            return table.Rows[typeRowNumber];
        }

        /// <summary>
        /// 获取主键索引
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static int GetKeyIndex(this DataTable table)
        {
            DataRow row = table.Rows[2];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (row[i].ToString() == "key")
                    return i;
            }
            return 0;
        }
    }
}
