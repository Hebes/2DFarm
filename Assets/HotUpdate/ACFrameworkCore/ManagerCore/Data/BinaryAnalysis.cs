/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据解析

-----------------------*/

using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System;

namespace ACFrameworkCore
{
    public static class BinaryAnalysis
    {
        /// <summary>
        /// 循环获取数据
        /// </summary>
        public static List<IData> GetData<T>(this byte[] bytes) where T : IData
        {
            List<IData> data = new List<IData>();
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
                        fi.SetValue(dataObj, BitConverter.ToSingle(bytes, pointer));
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

        public static List<T> GetDataAsT<T>(this byte[] bytes)
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
