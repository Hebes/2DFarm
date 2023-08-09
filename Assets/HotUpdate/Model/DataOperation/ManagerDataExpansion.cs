﻿/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据管理帮助类

-----------------------*/

namespace ACFrameworkCore
{
    public static class ManagerDataExpansion
    {
        public static void Save(object obj, string fileName, EDataType dataType)
        {
            ManagerData.Instance.Save(obj, fileName, dataType);
        }
        public static K Load<K>(this string fileName, EDataType dataType) where K : class
        {
            return ManagerData.Instance.Load<K>(fileName, dataType);
        }
    }
}
