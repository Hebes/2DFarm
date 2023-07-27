using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    二进制文件生成

-----------------------*/

namespace ACFrameworkCore
{
    public static class BinaryData
    {
        private static string DATA_BINARY_PATH = $"{Application.dataPath}/HotUpdate/GameMain/ExcelData/Binary/";

        /// <summary>
        /// 生成excel2进制数据
        /// </summary>
        /// <param name="table"></param>
        public static void GenerateExcelBinary(this DataTable table)
        {
            //没有路径创建路径
            DATA_BINARY_PATH.GenerateDirectory();
            //创建一个2进制文件进行写入
            using (FileStream fs = new FileStream(DATA_BINARY_PATH + table.TableName + ".Bytes", FileMode.OpenOrCreate, FileAccess.Write))
            {
                //存储具体的excel对应的2进制信息
                //1.先要存储我们需要写多少行的数据 方便我们读取
                //-4的原因是因为 前面4行是配置规则 并不是我们需要记录的数据内容
                fs.Write(BitConverter.GetBytes(table.Rows.Count - 4), 0, 4);
                //2.存储主键的变量名
                string keyName = table.GetVariableNameRow(0)[table.GetKeyIndex()].ToString();
                byte[] bytes = Encoding.UTF8.GetBytes(keyName);
                //存储字符串字节数组的长度
                fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
                //存储字符串字节数组
                fs.Write(bytes, 0, bytes.Length);

                //遍历所有内容的行 进行2进制的写入
                DataRow row;
                //得到类型行 根据类型来决定应该如何写入数据
                DataRow rowType = table.GetVariableTypeRow(1);
                for (int i = ExcelConfig.BEGIN_INDEX; i < table.Rows.Count; i++)
                {
                    //得到一行的数据
                    row = table.Rows[i];
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        switch (rowType[j].ToString())
                        {
                            case "int":
                                fs.Write(BitConverter.GetBytes(int.Parse(row[j].ToString())), 0, 4);
                                break;
                            case "float":
                                fs.Write(BitConverter.GetBytes(float.Parse(row[j].ToString())), 0, 4);
                                break;
                            case "bool":
                                fs.Write(BitConverter.GetBytes(bool.Parse(row[j].ToString())), 0, 1);
                                break;
                            case "string":
                                bytes = Encoding.UTF8.GetBytes(row[j].ToString());
                                //写入字符串字节数组的长度
                                fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
                                //写入字符串字节数组
                                fs.Write(bytes, 0, bytes.Length);
                                break;
                        }
                    }
                }
                fs.Close();
            }

            AssetDatabase.Refresh();
        }
    }
}
