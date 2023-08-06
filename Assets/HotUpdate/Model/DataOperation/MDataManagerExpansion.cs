/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据管理帮助类

-----------------------*/

namespace ACFrameworkCore
{
    public static class MDataManagerExpansion
    {
        public static K Load<K>(this string fileName, EDataType dataType) where K : UnityEngine.Object
        {
            return MDataManager.Instance.Load<K>(fileName, dataType);
        }
    }
}
