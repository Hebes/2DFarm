/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据管理类

-----------------------*/

namespace ACFrameworkCore
{
    public class DataManager
    {
        private static DataManager instance;

        #region  Type type参数
        //不用object对象传入 而使用 Type传入
        //主要目的是节约一行代码（在外部）
        //假设现在你要 读取一个Player类型的数据 如果是object 你就必须在外部new一个对象传入
        //现在有Type的 你只用传入 一个Type typeof(Player) 然后我在内部动态创建一个对象给你返回出来
        //达到了 让你在外部 少写一行代码的作用
        #endregion

        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataManager();
                return instance;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        public void Save<T>(object obj, string fileName) where T : IData, new()
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
        public K Load<T, K>(string fileName) where T : IData, new() where K : UnityEngine.Object
        {
            T t = new T();
            return t.Load<K>(fileName);
        }
    }
}
