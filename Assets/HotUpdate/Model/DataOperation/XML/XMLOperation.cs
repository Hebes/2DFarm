using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    XML文件保存

-----------------------*/


namespace Farm2D
{
    public class XMLOperation : IDataHandle
    {
        public string xmlSavePath = $"{Application.dataPath}/Excel2Script/XML";

        /// <summary>
        /// 保存数据到xml文件中
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <param name="fileName">文件名</param>
        public void SaveData(object data, string fileName)
        {
            //1.得到存储路径
            string path = Application.persistentDataPath + "/" + fileName + ".xml";
            //2.存储文件
            using (StreamWriter writer = new StreamWriter(path))
            {
                //3.序列化
                XmlSerializer s = new XmlSerializer(data.GetType());
                s.Serialize(writer, data);
            }
        }

        /// <summary>
        /// 从xml文件中读取内容 
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        //public object LoadData(Type type, string fileName)
        //{
        //    //1。首先要判断文件是否存在
        //    string path = Application.persistentDataPath + "/" + fileName + ".xml";
        //    if (!File.Exists(path))
        //    {
        //        path = Application.streamingAssetsPath + "/" + fileName + ".xml";
        //        if (!File.Exists(path))
        //        {
        //            //如果根本不存在文件 两个路径都找过了
        //            //那么直接new 一个对象 返回给外部 无非 里面都是默认值
        //            return Activator.CreateInstance(type);
        //        }
        //    }
        //    //2.存在就读取
        //    using (StreamReader reader = new StreamReader(path))
        //    {
        //        //3.反序列化 取出数据
        //        XmlSerializer s = new XmlSerializer(type);
        //        return s.Deserialize(reader);
        //    }
        //}

        public void Save(object obj, string fileName)
        {
        }

        public T Load<T>(string fileName) where T : class
        {
            Type type = typeof(T);
            //1。首先要判断文件是否存在
            string path = Application.persistentDataPath + "/" + fileName + ".xml";
            if (!File.Exists(path))
            {
                path = Application.streamingAssetsPath + "/" + fileName + ".xml";
                if (!File.Exists(path))
                {
                    //如果根本不存在文件 两个路径都找过了
                    //那么直接new 一个对象 返回给外部 无非 里面都是默认值
                    return Activator.CreateInstance(type) as T;
                }
            }
            //2.存在就读取
            using (StreamReader reader = new StreamReader(path))
            {
                //3.反序列化 取出数据
                XmlSerializer s = new XmlSerializer(type);
                return s.Deserialize(reader) as T;
            }
        }
    }
}
