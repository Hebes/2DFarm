using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    二进制数据操作

-----------------------*/


namespace Farm2D
{
    public class BinaryOperation : IDataHandle
    {
        private static string SAVE_PATH = $"{Application.dataPath}/Excel2Script/Byte/";

        /// <summary>
        /// 读取2进制数据转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T Load<T>(string fileName) where T : class
        {
            string filePath = $"{SAVE_PATH}{fileName}.bytes";
            //如果不存在这个文件 就直接返回泛型对象的默认值
            if (!File.Exists(filePath))
                return default(T);
            T obj;
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                obj = bf.Deserialize(fs) as T;
                fs.Close();
            }
            return obj;
        }

        /// <summary>
        /// 存储类对象数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        public void Save(object obj, string fileName)
        {
            string filePath = $"{SAVE_PATH}{fileName}.bytes";
            //先判断路径文件夹有没有
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(SAVE_PATH);

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
                fs.Close();
            }
        }
    }
}
