using ACFrameworkCore;
using System.Collections.Generic;


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
    public class ShopManagerSystem : ICore
    {
        public static ShopManagerSystem Instance;

        public void ICroeInit()
        {
            Instance = this;

            //初始化商人数据
            //初始化数据(商店和箱子)
            List<ShopDetailsData> shopDetailsDatasList = this.GetDataListThis<ShopDetailsData>();
            foreach (ShopDetailsData shopDetailsData in shopDetailsDatasList)
            {
                InventoryItem inventoryItem = new InventoryItem();
                inventoryItem.itemID = shopDetailsData.itemID;
                inventoryItem.itemAmount = shopDetailsData.itemAmount;
                if (ItemManagerSystem.Instance.ChackKey(shopDetailsData.shopkeeperName))
                    ItemManagerSystem.Instance.ItemDic[shopDetailsData.shopkeeperName].Add(inventoryItem);
                else
                    ItemManagerSystem.Instance.ItemDic.Add(shopDetailsData.shopkeeperName, new List<InventoryItem>() { inventoryItem });
            }
        }
    }
}
