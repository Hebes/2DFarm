using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    物品总管理类

-----------------------*/

namespace ACFrameworkCore
{
    public class InventoryAllManager : ICore
    {
        public static InventoryAllManager Instance;
        public Dictionary<string, List<InventoryItem>> ItemDicList; //物品字典列表
        public Dictionary<string, InventoryItem[]> ItemDicArray; //物品字典列表


        public void ICroeInit()
        {
            Instance = this;
            ItemDicList = new Dictionary<string, List<InventoryItem>>();
            ItemDicArray = new Dictionary<string, InventoryItem[]>();

            ItemDicArray.Add(ConfigInventory.ActionBar, new InventoryItem[8]);
            ItemDicArray.Add(ConfigInventory.PalayerBag, new InventoryItem[16]);
        }

        public bool AddItem(string key, Item item, bool toDestory = false)//捡东西可以设置ture
        {
            ItemDicList.TryGetValue(key, out List<InventoryItem> itemList);
            if (itemList == null) return false;
            InventoryItem inventoryItem = itemList.Find(itemTemp => { return itemTemp.itemID == item.itemID; });
            if (inventoryItem.itemID != 0)//有东西
            {
                InventoryItem itemTemp = new InventoryItem { itemID = item.itemID, itemAmount = item.itemAmount };
                ItemDicList.Add(key, new List<InventoryItem>() { itemTemp });
            }
            else//没有
            {
                inventoryItem.itemAmount += item.itemAmount;
            }
            if (toDestory)
                GameObject.Destroy(item.gameObject);
            //更新物品UI 呼叫事件中心,执行委托的代码
            //ConfigEvent.UpdateInvenoryUI.EventTrigger(key, ItemDicList[key]);
            return true;
        }

        //是否存在这个物品
        private int GetItemIndexBag(string key, int ID)
        {
            //ItemDicList.TryGetValue(key, out List<InventoryItem> itemList);
            //for (int i = 0; i < itemList?.Count; i++)
            //{
            //    InventoryItem inventoryItem = PlayerBagItemList[i];
            //    if (inventoryItem.itemID == ID)
            //        return i;
            //}
            return -1;
        }
        //检查空位
        private bool CheckBagCapacity(string key)
        {
            //ItemDicList.TryGetValue(key, out List<InventoryItem> itemList);
            //for (int i = 0; i < PlayerBagItemList.Count; i++)
            //{
            //    InventoryItem inventoryItem = PlayerBagItemList[i];
            //    if (inventoryItem.itemID == 0)
            //        return true;
            //}
            return false;
        }

        //ItemDicArray字典操作
        public bool AddItemItemDicArray(string key, Item item, bool toDestory = false)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] inventoryItemArray);
            if (inventoryItemArray == null) return false;
            int index1 = GetItemIndexArray(key, item.itemID);
            int index2 = CheckCapacityArray(key);
            if (index1 == -1)//没有物品
            {
                if (index2 == -1) return false;//-1没有空位
                inventoryItemArray[index2] = new InventoryItem() { itemID = item.itemID, itemAmount = item.itemAmount };
            }
            else
            {
                inventoryItemArray[index1].itemAmount += item.itemAmount;
            }
            if (toDestory)
                GameObject.Destroy(item.gameObject);
            //更新物品UI 呼叫事件中心,执行委托的代码
            key.EventTrigger(ItemDicArray[key]);
            return true;
        }
        private int GetItemIndexArray(string key, int ID)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] itemList);
            for (int i = 0; i < itemList?.Length; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == ID)
                    return i;
            }
            return -1;
        }//是否存在这个物品
        private int CheckCapacityArray(string key)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] itemList);
            for (int i = 0; i < itemList?.Length; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == 0)
                    return i;
            }
            return -1;
        }//检查空位

        //获取物品信息
        public ItemDetails GetItem(int id)
        {
            return DataManager.Instance.GetDataOne<ItemDetails>(id);
        }
    }
}
