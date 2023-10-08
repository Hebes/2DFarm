using System.Collections.Generic;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据获取帮助类

-----------------------*/

namespace Core
{
    public static class DataExpansion
    {
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetDataOne<T>(this int id) where T : class, IData
        {
            T t = CoreData.Instance.GetDataOne<T>(id);
            if (t == null)
                Debug.Error($"请先初始化数据{typeof(T).FullName}");
            return t;
        }

        /// <summary>
        /// 获取一组数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> GetDataList<T>(this object obj) where T : class, IData
        {
            List<T> tempList = CoreData.Instance.GetDataList<T>();
            if (tempList == null || tempList.Count == 0)
                Debug.Error($"请先初始化数据{typeof(T).FullName}");
            return tempList;
        }
    }
}
