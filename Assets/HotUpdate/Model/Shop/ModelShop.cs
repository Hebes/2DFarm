using Core;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	商店管理系统

-----------------------*/

namespace Farm2D
{
    public class ModelShop : IModelInit
    {
        public static ModelShop Instance;

        public async UniTask ModelInit()
        {
            Instance = this;

            //初始化商人数据
            //初始化数据(商店和箱子)
            List<ShopDetailsData> shopDetailsDatasList = this.GetDataList<ShopDetailsData>();
            foreach (ShopDetailsData shopDetailsData in shopDetailsDatasList)
            {
                InventoryItem inventoryItem = new InventoryItem();
                inventoryItem.itemID = shopDetailsData.itemID;
                inventoryItem.itemAmount = shopDetailsData.itemAmount;
                if (ModelItem.Instance.ChackKey(shopDetailsData.shopkeeperName))
                    ModelItem.Instance.ItemDic[shopDetailsData.shopkeeperName].Add(inventoryItem);
                else
                    ModelItem.Instance.ItemDic.Add(shopDetailsData.shopkeeperName, new List<InventoryItem>() { inventoryItem });
            }

            await UniTask.Yield();
        }
    }
}
