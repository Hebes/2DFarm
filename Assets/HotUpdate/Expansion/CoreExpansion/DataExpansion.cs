using System.Collections.Generic;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据获取帮助类

-----------------------*/

namespace ACFrameworkCore
{
    public static class DataExpansion
    {
        public static T GetDataOne<T>(this int id) where T : class, IData
        {
            return DataManager.Instance.GetDataOne<T>(id);
        }
        public static List<IData> GetDataList<T>(this object obj) where T : class, IData
        {
            return DataManager.Instance.GetDataList<T>();
        }
    }
}
