using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	商店数据拓展类

-----------------------*/

namespace ACFarm
{
    public static class ShopManagerSystemExpansion
    {
        public static ACFrameworkCore.InventoryItem[] GetShopData(this string key)
        {
            return ShopManagerSyste.Instance.GetShopData(key);
        }
    }
}
