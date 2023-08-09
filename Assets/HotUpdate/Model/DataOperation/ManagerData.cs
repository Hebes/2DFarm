/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据管理类

-----------------------*/

using System.Runtime.Remoting.Metadata;

namespace ACFrameworkCore
{
    /// <summary>
    /// 数据操作类型
    /// </summary>
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
    public class ManagerData : Singleton<ManagerData>
    {

        #region  Type type参数
        //不用object对象传入 而使用 Type传入
        //主要目的是节约一行代码（在外部）
        //假设现在你要 读取一个Player类型的数据 如果是object 你就必须在外部new一个对象传入
        //现在有Type的 你只用传入 一个Type typeof(Player) 然后我在内部动态创建一个对象给你返回出来
        //达到了 让你在外部 少写一行代码的作用
        #endregion

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
