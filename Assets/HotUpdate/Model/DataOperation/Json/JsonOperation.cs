using LitJson;
using System.IO;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    Json操作

-----------------------*/

namespace Farm2D
{
    /// <summary>
    /// 序列化和反序列化Json时  使用的是哪种方案
    /// </summary>
    public enum JsonType
    {
        JsonUtlity,
        LitJson,
    }

    public class JsonOperation : IDataHandle
    {
        private JsonType jsonType = JsonType.LitJson;
        public T Load<T>(string fileName) where T : class
        {
            //确定从哪个路径读取
            //首先先判断 默认数据文件夹中是否有我们想要的数据 如果有 就从中获取
            string path = $"{Application.streamingAssetsPath}/{fileName}.json";
            //先判断 是否存在这个文件
            //如果不存在默认文件 就从 读写文件夹中去寻找
            if (!File.Exists(path))
                path = $"{Application.persistentDataPath}/{fileName}.json";
            //如果读写文件夹中都还没有 那就返回一个默认对象
            if (!File.Exists(path))
                return null;

            //进行反序列化
            string jsonStr = File.ReadAllText(path);
            //数据对象
            T data = default(T);
            switch (jsonType)
            {
                case JsonType.JsonUtlity:
                    data = JsonUtility.FromJson<T>(jsonStr);
                    break;
                case JsonType.LitJson:
                    data = JsonMapper.ToObject<T>(jsonStr);
                    break;
            }

            //把对象返回出去
            return data;
        }

        public void Save(object data, string fileName)
        {
            //确定存储路径
            string path = Application.persistentDataPath + "/" + fileName + ".json";
            //序列化 得到Json字符串
            string jsonStr = "";
            switch (jsonType)
            {
                case JsonType.JsonUtlity: jsonStr = JsonUtility.ToJson(data); break;
                case JsonType.LitJson: jsonStr = JsonMapper.ToJson(data); break;
            }
            //把序列化的Json字符串 存储到指定路径的文件中
            File.WriteAllText(path, jsonStr);
        }
    }
}
