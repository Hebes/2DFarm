using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACFrameworkCore;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	商店管理系统

-----------------------*/

namespace ACFarm
{

    public class ShopManagerSyste:ICore
    {
        public static ShopManagerSyste Instance;
        private Dictionary<string, InventoryItem[]> ShopDic;

        public void ICroeInit()
        {
            Instance = this;
            ShopDic=new Dictionary<string, InventoryItem[]>();

            //初始化商人数据
            //初始化数据(商店和箱子)
            List<ShopDetailsData> shopDetailsDatasList = this.GetDataListThis<ShopDetailsData>();
            foreach (ShopDetailsData shopDetailsData in shopDetailsDatasList)
            {
                InventoryItem inventoryItem = new InventoryItem();
                inventoryItem.itemID = shopDetailsData.itemID;
                inventoryItem.itemAmount = shopDetailsData.itemAmount;
                if (ShopDic.ContainsKey(shopDetailsData.shopkeeperName))
                {
                    List<InventoryItem> inventoryItems = ShopDic[shopDetailsData.shopkeeperName].ToList();
                    inventoryItems.Add(inventoryItem);
                    ShopDic[shopDetailsData.shopkeeperName] = inventoryItems.ToArray();
                }
                else
                {
                    ShopDic.Add(shopDetailsData.shopkeeperName, new InventoryItem[1]);
                    ShopDic[shopDetailsData.shopkeeperName][0] = inventoryItem;
                }
            }
        }

        public InventoryItem[] GetShopData(string key)
        {
            if (ShopDic.TryGetValue(key, out InventoryItem[] InventoryItems))
                return InventoryItems;
            ACDebug.Log($"获取的数据是空的{key}");
            return null;
        }
    }
}
