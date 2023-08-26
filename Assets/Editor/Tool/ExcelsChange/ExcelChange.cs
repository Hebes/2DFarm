using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    Excel转换工具

-----------------------*/


namespace ACFrameworkCore
{
    /// <summary>
    /// 每行类型
    /// </summary>
    public enum RowType : byte
    {
        FIELD_NAME = 0,//名称
        FIELD_TYPE = 1,//类型
        BEGIN_INDEX = 3//开始行数
    }

    public class ExcelChange : EditorWindow
    {
        /// <summary>
        /// excel文件存放的路径
        /// </summary>
        public static string EXCEL_PATH = Application.dataPath + "/Editor/Excels/";

        //[MenuItem("Tool/字节测试")]
        //public static void TestByte()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("有符号");
        //    sb.AppendLine("sbyte" + sizeof(sbyte) + "字节");
        //    sb.AppendLine("int" + sizeof(int) + "字节");
        //    sb.AppendLine("short" + sizeof(short) + "字节");
        //    sb.AppendLine("long" + sizeof(long) + "字节");
        //    sb.AppendLine("无符号");
        //    sb.AppendLine("byte" + sizeof(byte) + "字节");
        //    sb.AppendLine("uint" + sizeof(uint) + "字节");
        //    sb.AppendLine("ushort" + sizeof(ushort) + "字节");
        //    sb.AppendLine("ulong" + sizeof(ulong) + "字节");
        //    sb.AppendLine("浮点");
        //    sb.AppendLine("float" + sizeof(float) + "字节");
        //    sb.AppendLine("double" + sizeof(double) + "字节");
        //    sb.AppendLine("decimal" + sizeof(decimal) + "字节");
        //    sb.AppendLine("特殊");
        //    sb.AppendLine("bool" + sizeof(bool) + "字节");
        //    sb.AppendLine("char" + sizeof(char) + "字节");
        //    Debug.Log(sb.ToString());
        //}

        [MenuItem("Tool/Excel转换")]//#E
        public static void GenerateExcelInfo()
        {
            IEnumerable<string> paths = Directory.EnumerateFiles(EXCEL_PATH, "*.xlsx");
            foreach (string filePath in paths)
            {
                //读取Excel数据
                string[][] data = filePath.LoadExcel();
                //生成C#文件
                ClassData.CreateScript(filePath, data);
                Debug.Log("C#文件生成完毕");
                //生成二进制文件
                BinaryData.CreateByte(filePath, data);
                Debug.Log("二进制文件生成完毕");
            }
            //刷新Project窗口
            AssetDatabase.Refresh();
        }


        private static string DATA_BINARY_PATH = $"{Application.dataPath}/AssetsPackage/BinaryData/ItemDetails.bytes";//Assets/AssetsPackage/BinaryData/ItemDetails.bytes
        //[MenuItem("Tool/Bytes数据读取")]//#E
        public static void ReaDData()
        {
            //using (FileStream fs = File.Open(DATA_BINARY_PATH, FileMode.Open, FileAccess.Read))
            //{
            //    //读取全都的字节
            //    byte[] bytes = new byte[fs.Length];
            //    fs.Read(bytes, 0, bytes.Length);
            //    fs.Close();
            //    List<ItemDetails> towerInfos = LoopGetData<ItemDetails>(bytes);
            //    foreach (ItemDetails info in towerInfos)
            //    {
            //        Debug.Log(info.name);
            //    }
            //}


            //ItemDetails towerInfo = Load<ItemDetails>();
            //ItemDetails towerInfo = new ItemDetails()
            //{
            //    atk = 30,
            //    atkRange = 30,
            //    atkType = 30,
            //    eff = "测试1",
            //    id = 30,
            //    imgRes = "测试1",
            //};
        }

        /// <summary>
        /// 循环获取数据
        /// </summary>
        public static List<T> LoopGetData<T>(byte[] bytes)
        {
            List<T> data = new List<T>();
            //指针下标
            int pointer = 0;
            //得到数据结构类的Type
            Type classType = typeof(T);
            //通过反射 得到数据结构类 所有字段的信息
            FieldInfo[] infos = classType.GetFields();
            //循环读取数据
            while (bytes.Length != pointer)
            {
                //创建新的类
                object dataObj = Activator.CreateInstance(classType);
                foreach (FieldInfo fi in infos)
                {

                    if (fi.FieldType == typeof(int))
                    {
                        fi.SetValue(dataObj, BitConverter.ToInt32(bytes, pointer));
                        pointer += 4;
                    }
                    else if (fi.FieldType == typeof(bool))
                    {
                        fi.SetValue(dataObj, BitConverter.ToBoolean(bytes, pointer));
                        pointer += 1;
                    }
                    else if (fi.FieldType == typeof(string))
                    {
                        //读取字符串字节数组的长度
                        int length = BitConverter.ToInt32(bytes, pointer);
                        pointer += 4;
                        fi.SetValue(dataObj, Encoding.UTF8.GetString(bytes, pointer, length));
                        pointer += length;
                    }
                    else if (fi.FieldType == typeof(float))
                    {
                        fi.SetValue(dataObj, BitConverter.ToInt32(bytes, pointer));
                        pointer += 4;
                    }
                    else if (fi.FieldType == typeof(List<int>))
                    {
                        //读取字符串字节数组的长度
                        int length = BitConverter.ToInt32(bytes, pointer);
                        pointer += 4;
                        string str = Encoding.UTF8.GetString(bytes, pointer, length);
                        pointer += length;
                        string[] dataArray = str.Split('|');
                        List<int> intList = new List<int>();
                        foreach (var item in dataArray)
                            intList.Add(int.Parse(item));
                        fi.SetValue(dataObj, intList);
                    }
                    else if (fi.FieldType == typeof(List<string>))
                    {
                        //读取字符串字节数组的长度
                        int length = BitConverter.ToInt32(bytes, pointer);
                        pointer += 4;
                        string str = Encoding.UTF8.GetString(bytes, pointer, length);
                        pointer += length;
                        string[] dataArray = str.Split('|');
                        List<string> stringList = new List<string>();
                        foreach (var item in dataArray)
                            stringList.Add(item);
                        fi.SetValue(dataObj, stringList);
                    }
                    else if (fi.FieldType == typeof(List<float>))
                    {
                        //读取字符串字节数组的长度
                        int length = BitConverter.ToInt32(bytes, pointer);
                        pointer += 4;
                        string str = Encoding.UTF8.GetString(bytes, pointer, length);
                        pointer += length;
                        string[] dataArray = str.Split('|');
                        List<float> floatList = new List<float>();
                        foreach (var item in dataArray)
                            floatList.Add(float.Parse(item));
                        fi.SetValue(dataObj, floatList);
                    }
                }
                data.Add((T)dataObj);
            }
            return data;
        }
    }
}
