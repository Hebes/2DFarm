using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    PlayerPrefs操作

-----------------------*/

namespace Farm2D
{
    public class PlayerPrefsOperation : IDataHandle
    {
        public T Load<T>(string fileName) where T : class
        {
            throw new NotImplementedException();
        }

        public void Save(object value, string keyName)
        {
            //直接通过PlayerPrefs来进行存储
            Type fieldType = value.GetType();

            //类型判断
            //是不是int
            if (fieldType == typeof(int))
            {
                Debug.Log("存储int" + keyName);
                PlayerPrefs.SetInt(keyName, (int)value);
            }
            else if (fieldType == typeof(float))
            {
                Debug.Log("存储float" + keyName);
                PlayerPrefs.SetFloat(keyName, (float)value);
            }
            else if (fieldType == typeof(string))
            {
                Debug.Log("存储string" + keyName);
                PlayerPrefs.SetString(keyName, value.ToString());
            }
            else if (fieldType == typeof(bool))
            {
                Debug.Log("存储bool" + keyName);
                //自己顶一个存储bool的规则
                PlayerPrefs.SetInt(keyName, (bool)value ? 1 : 0);
            }
            //如何判断 泛型类的类型呢
            //通过反射 判断 父子关系
            //这相当于是判断 字段是不是IList的子类
            else if (typeof(IList).IsAssignableFrom(fieldType))
            {
                Debug.Log("存储List" + keyName);
                //父类装子类
                IList list = value as IList;
                //先存储 数量 
                PlayerPrefs.SetInt(keyName, list.Count);
                int index = 0;
                foreach (object obj in list)
                {
                    //存储具体的值
                    Save(obj, keyName + index);
                    ++index;
                }
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <param name="keyName">数据对象的唯一key 自己控制</param>
        public void SaveData(object data, string keyName)
        {
            
            //获取传入数据对象的所有字段
            Type dataType = data.GetType();
            //得到所有的字段
            FieldInfo[] infos = dataType.GetFields();

            string saveKeyName = "";
            FieldInfo info;
            for (int i = 0; i < infos.Length; i++)
            {
                info = infos[i];
                saveKeyName = keyName + "_" + dataType.Name +
                    "_" + info.FieldType.Name + "_" + info.Name;
                SaveValue(info.GetValue(data), saveKeyName);
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <param name="keyName">数据对象的唯一key 自己控制</param>
        public void SaveData1(object data, string keyName)
        {
            Type dataType = data.GetType();
            //得到所有的字段
            FieldInfo[] infos = dataType.GetFields();
            string saveKeyName = "";
            FieldInfo info;
            for (int i = 0; i < infos.Length; i++)
            {
                info = infos[i];
                saveKeyName = keyName + "_" + dataType.Name +
                    "_" + info.FieldType.Name + "_" + info.Name;
                SaveValue(info.GetValue(data), saveKeyName);
            }

            PlayerPrefs.Save();
        }

        private void SaveValue(object value, string keyName)
        {
            Type fieldType = value.GetType();

            //类型判断
            //是不是int
            if (fieldType == typeof(int))
            {
                Debug.Log("存储int" + keyName);
                //为int数据加密
                int rValue = (int)value;
                rValue += 10;
                PlayerPrefs.SetInt(keyName, rValue);
            }
            else if (fieldType == typeof(float))
            {
                Debug.Log("存储float" + keyName);
                PlayerPrefs.SetFloat(keyName, (float)value);
            }
            else if (fieldType == typeof(string))
            {
                Debug.Log("存储string" + keyName);
                PlayerPrefs.SetString(keyName, value.ToString());
            }
            else if (fieldType == typeof(bool))
            {
                Debug.Log("存储bool" + keyName);
                //自己顶一个存储bool的规则
                PlayerPrefs.SetInt(keyName, (bool)value ? 1 : 0);
            }
            //如何判断 泛型类的类型呢
            //通过反射 判断 父子关系
            //这相当于是判断 字段是不是IList的子类
            else if (typeof(IList).IsAssignableFrom(fieldType))
            {
                Debug.Log("存储List" + keyName);
                //父类装子类
                IList list = value as IList;
                //先存储 数量 
                PlayerPrefs.SetInt(keyName, list.Count);
                int index = 0;
                foreach (object obj in list)
                {
                    //存储具体的值
                    SaveValue(obj, keyName + index);
                    ++index;
                }
            }
            //判断是不是Dictionary类型 通过Dictionary的父类来判断
            else if (typeof(IDictionary).IsAssignableFrom(fieldType))
            {
                Debug.Log("存储Dictionary" + keyName);
                //父类装自来
                IDictionary dic = value as IDictionary;
                //先存字典长度
                PlayerPrefs.SetInt(keyName, dic.Count);
                //遍历存储Dic里面的具体值
                //用于区分 表示的 区分 key
                int index = 0;
                foreach (object key in dic.Keys)
                {
                    SaveValue(key, keyName + "_key_" + index);
                    SaveValue(dic[key], keyName + "_value_" + index);
                    ++index;
                }
            }
            //基础数据类型都不是 那么可能就是自定义类型
            else
            {
                SaveData(value, keyName);
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="type">想要读取数据的 数据类型Type</param>
        /// <param name="keyName">数据对象的唯一key 自己控制</param>
        /// <returns></returns>
        public object LoadData(Type type, string keyName)
        {
            //不用object对象传入 而使用 Type传入
            //主要目的是节约一行代码（在外部）
            object data = Activator.CreateInstance(type);
            //要往这个new出来的对象中存储数据 填充数据
            //得到所有字段
            FieldInfo[] infos = type.GetFields();
            //用于拼接key的字符串
            string loadKeyName = "";
            //用于存储 单个字段信息的 对象
            FieldInfo info;
            for (int i = 0; i < infos.Length; i++)
            {
                info = infos[i];
                //key的拼接规则 一定是和存储时一模一样 这样才能找到对应数据
                loadKeyName = keyName + "_" + type.Name +
                    "_" + info.FieldType.Name + "_" + info.Name;

                //有key 就可以结合 PlayerPrefs来读取数据
                //填充数据到data中 
                info.SetValue(data, LoadValue(info.FieldType, loadKeyName));
            }
            return data;
        }

        /// <summary>
        /// 得到单个数据的方法
        /// </summary>
        /// <param name="fieldType">字段类型 用于判断 用哪个api来读取</param>
        /// <param name="keyName">用于获取具体数据</param>
        /// <returns></returns>
        private object LoadValue(Type fieldType, string keyName)
        {
            //根据 字段类型 来判断 用哪个API来读取
            if (fieldType == typeof(int))
            {
                //解密 减10
                return PlayerPrefs.GetInt(keyName, 0) - 10;
            }
            else if (fieldType == typeof(float))
            {
                return PlayerPrefs.GetFloat(keyName, 0);
            }
            else if (fieldType == typeof(string))
            {
                return PlayerPrefs.GetString(keyName, "");
            }
            else if (fieldType == typeof(bool))
            {
                //根据自定义存储bool的规则 来进行值的获取
                return PlayerPrefs.GetInt(keyName, 0) == 1 ? true : false;
            }
            else if (typeof(IList).IsAssignableFrom(fieldType))
            {
                //得到长度
                int count = PlayerPrefs.GetInt(keyName, 0);
                //实例化一个List对象 来进行赋值
                //用了反射中双A中 Activator进行快速实例化List对象
                IList list = Activator.CreateInstance(fieldType) as IList;
                for (int i = 0; i < count; i++)
                {
                    //目的是要得到 List中泛型的类型 
                    list.Add(LoadValue(fieldType.GetGenericArguments()[0], keyName + i));
                }
                return list;
            }
            else if (typeof(IDictionary).IsAssignableFrom(fieldType))
            {
                //得到字典的长度
                int count = PlayerPrefs.GetInt(keyName, 0);
                //实例化一个字典对象 用父类装子类
                IDictionary dic = Activator.CreateInstance(fieldType) as IDictionary;
                Type[] kvType = fieldType.GetGenericArguments();
                for (int i = 0; i < count; i++)
                {
                    dic.Add(LoadValue(kvType[0], keyName + "_key_" + i),
                             LoadValue(kvType[1], keyName + "_value_" + i));
                }
                return dic;
            }
            else
            {
                return LoadData(fieldType, keyName);
            }
            //return null;
        }
    }
}
