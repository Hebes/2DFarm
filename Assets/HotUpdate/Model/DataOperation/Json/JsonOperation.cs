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

namespace ACFrameworkCore
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
        #region 知识点二 JsonUtlity和LitJson不同点
        //1.JsonUtlity是Unity自带，LitJson是第三方需要引用命名空间
        //2.JsonUtlity使用时自定义类需要加特性,LitJson不需要
        //3.JsonUtlity支持私有变量(加特性),LitJson不支持
        //4.JsonUtlity不支持字典,LitJson支持(但是键只能是字符串)
        //5.JsonUtlity不能直接将数据反序列化为数据集合(数组字典),LitJson可以
        //6.JsonUtlity对自定义类不要求有无参构造，LitJson需要
        //7.JsonUtlity存储空对象时会存储默认值而不是null，LitJson会存null
        #endregion

        #region 知识点三 如何选择两者
        //根据实际需求
        //建议使用LitJson
        //原因：LitJson不用加特性，支持字典，支持直接反序列化为数据集合，存储null更准确
        #endregion

        #region 总结
        //1.LitJson提供的序列化反序列化方法 JsonMapper.ToJson和ToObject<>
        //2.LitJson无需加特性
        //3.LitJson不支持私有变量
        //4.LitJson支持字典序列化反序列化
        //5.LitJson可以直接将数据反序列化为数据集合
        //6.LitJson反序列化时 自定义类型需要无参构造
        //7.Json文档编码格式必须是UTF-8
        #endregion

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
