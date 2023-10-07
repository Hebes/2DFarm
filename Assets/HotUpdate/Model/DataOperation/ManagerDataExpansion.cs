/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据管理帮助类

-----------------------*/

namespace Farm2D
{
    public static class ManagerDataExpansion
    {
        public static void Save(object obj, string fileName, EDataType dataType)
        {
            DataSystem.Instance.Save(obj, fileName, dataType);
        }
        public static K Load<K>(this string fileName, EDataType dataType) where K : class
        {
            return DataSystem.Instance.Load<K>(fileName, dataType);
        }
    }
}
