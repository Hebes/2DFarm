﻿using System;
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

namespace ACFrameworkCore
{
    public class PlayerPrefsOperation : IData
    {

        #region 知识点一 PlayerPrefs存储的数据存在哪里？
        //不同平台存储位置不一样

        #region Windows
        //PlayerPrefs 存储在 
        //HKCU\Software\[公司名称]\[产品名称] 项下的注册表中
        //其中公司和产品名称是 在“Project Settings”中设置的名称。

        //运行 regedit
        //HKEY_CURRENT_USER
        //SOFTWARE
        //Unity
        //UnityEditor
        //公司名称
        //产品名称
        #endregion

        #region Android
        // /data/data/包名/shared_prefs/pkg-name.xml 
        #endregion

        #region IOS
        // /Library/Preferences/[应用ID].plist
        #endregion

        #endregion

        #region 知识点二 PlayerPrefs数据唯一性
        //PlayerPrefs中不同数据的唯一性
        //是由key决定的，不同的key决定了不同的数据
        //同一项目中 如果不同数据key相同 会造成数据丢失
        //要保证数据不丢失就要建立一个保证key唯一的规则
        #endregion

        public T Load<T>(string fileName) where T : class
        {
            throw new NotImplementedException();
        }

        public void Save(object value, string keyName)
        {
            //直接通过PlayerPrefs来进行存储了
            //就是根据数据类型的不同 来决定使用哪一个API来进行存储
            //PlayerPrefs只支持3种类型存储 
            //判断 数据类型 是什么类型 然后调用具体的方法来存储
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
            //就是要通过 Type 得到传入数据对象的所有的 字段
            //然后结合 PlayerPrefs来进行存储

            #region 第一步 获取传入数据对象的所有字段
            Type dataType = data.GetType();
            //得到所有的字段
            FieldInfo[] infos = dataType.GetFields();
            #endregion

            #region 第二步 自己定义一个key的规则 进行数据存储
            //我们存储都是通过PlayerPrefs来进行存储的
            //保证key的唯一性 我们就需要自己定一个key的规则

            //我们自己定一个规则
            // keyName_数据类型_字段类型_字段名
            #endregion

            #region 第三步 遍历这些字段 进行数据存储
            string saveKeyName = "";
            FieldInfo info;
            for (int i = 0; i < infos.Length; i++)
            {
                //对每一个字段 进行数据存储
                //得到具体的字段信息
                info = infos[i];
                //通过FieldInfo可以直接获取到 字段的类型 和字段的名字
                //字段的类型 info.FieldType.Name
                //字段的名字 info.Name;

                //要根据我们定的key的拼接规则 来进行key的生成
                //Player1_PlayerInfo_Int32_age
                saveKeyName = keyName + "_" + dataType.Name +
                    "_" + info.FieldType.Name + "_" + info.Name;

                //现在得到了Key 按照我们的规则
                //接下来就要来通过PlayerPrefs来进行存储
                //如何获取值
                //info.GetValue(data)
                //封装了一个方法 专门来存储值 
                SaveValue(info.GetValue(data), saveKeyName);
            }
            #endregion
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <param name="keyName">数据对象的唯一key 自己控制</param>
        public void SaveData1(object data, string keyName)
        {
            //就是要通过 Type 得到传入数据对象的所有的 字段
            //然后结合 PlayerPrefs来进行存储

            #region 第一步 获取传入数据对象的所有字段
            Type dataType = data.GetType();
            //得到所有的字段
            FieldInfo[] infos = dataType.GetFields();
            #endregion

            #region 第二步 自己定义一个key的规则 进行数据存储
            //我们存储都是通过PlayerPrefs来进行存储的
            //保证key的唯一性 我们就需要自己定一个key的规则

            //我们自己定一个规则
            // keyName_数据类型_字段类型_字段名
            #endregion

            #region 第三步 遍历这些字段 进行数据存储
            string saveKeyName = "";
            FieldInfo info;
            for (int i = 0; i < infos.Length; i++)
            {
                //对每一个字段 进行数据存储
                //得到具体的字段信息
                info = infos[i];
                //通过FieldInfo可以直接获取到 字段的类型 和字段的名字
                //字段的类型 info.FieldType.Name
                //字段的名字 info.Name;

                //要根据我们定的key的拼接规则 来进行key的生成
                //Player1_PlayerInfo_Int32_age
                saveKeyName = keyName + "_" + dataType.Name +
                    "_" + info.FieldType.Name + "_" + info.Name;

                //现在得到了Key 按照我们的规则
                //接下来就要来通过PlayerPrefs来进行存储
                //如何获取值
                //info.GetValue(data)
                //封装了一个方法 专门来存储值 
                SaveValue(info.GetValue(data), saveKeyName);
            }

            PlayerPrefs.Save();
            #endregion
        }

        private void SaveValue(object value, string keyName)
        {
            //直接通过PlayerPrefs来进行存储了
            //就是根据数据类型的不同 来决定使用哪一个API来进行存储
            //PlayerPrefs只支持3种类型存储 
            //判断 数据类型 是什么类型 然后调用具体的方法来存储
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
            //假设现在你要 读取一个Player类型的数据 如果是object 你就必须在外部new一个对象传入
            //现在有Type的 你只用传入 一个Type typeof(Player) 然后我在内部动态创建一个对象给你返回出来
            //达到了 让你在外部 少写一行代码的作用

            //根据你传入的类型 和 keyName
            //依据你存储数据时  key的拼接规则 来进行数据的获取赋值 返回出去

            //根据传入的Type 创建一个对象 用于存储数据
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


            return null;
        }
    }
}
