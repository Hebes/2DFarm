using Core;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据管理系统

-----------------------*/

namespace Farm2D
{
    /// <summary> 数据操作类型 </summary>
    public enum EDataType
    {
        /// <summary> 二进制 </summary>
        Binary,
        /// <summary> Json </summary>
        Json,
        /// <summary> 编辑器自带 </summary>
        PlayerPrefs,
        /// <summary> XML </summary>
        XML,
    }
    public class DataSystem : SingletonBase<DataSystem>
    {
        public void Save(object obj, string fileName, EDataType dataType)
        {
            switch (dataType)
            {
                case EDataType.Binary: Save<BinaryOperation>(obj, fileName); break;
                case EDataType.Json: Save<JsonOperation>(obj, fileName); break;
                case EDataType.PlayerPrefs: Save<PlayerPrefsOperation>(obj, fileName); break;
                case EDataType.XML: Save<XMLOperation>(obj, fileName); break;
            }
        }

        public K Load<K>(string fileName, EDataType dataType) where K : class
        {
            switch (dataType)
            {
                case EDataType.Binary:
                    return Load<BinaryOperation, K>(fileName);
                case EDataType.Json:
                    return Load<JsonOperation, K>(fileName);
                case EDataType.PlayerPrefs:
                    return Load<PlayerPrefsOperation, K>(fileName);
                case EDataType.XML:
                    return Load<XMLOperation, K>(fileName);
                default: return null;
            }
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        private void Save<T>(object obj, string fileName) where T : IDataHandle, new()
        {
            T t = new T();
            t.Save(obj, fileName);
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private K Load<T, K>(string fileName) where T : IDataHandle, new() where K : class
        {
            T t = new T();
            return t.Load<K>(fileName);
        }
    }
}
