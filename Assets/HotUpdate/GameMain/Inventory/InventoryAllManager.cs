using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        public Dictionary<string, List<InventoryItem>> ItemDicList; //物品字典列表 两本字典的KEY请不要重复
        public Dictionary<string, InventoryItem[]> ItemDicArray; //物品字典列表   两本字典的KEY请不要重复
        public Dictionary<string, List<SlotUI>> slotUIDic;       //所有的高亮的格子

        public Image dragItem;//拖拽的物体

        public void ICroeInit()
        {
            Instance = this;
            ItemDicList = new Dictionary<string, List<InventoryItem>>();
            ItemDicArray = new Dictionary<string, InventoryItem[]>();
            slotUIDic = new Dictionary<string, List<SlotUI>>();
            //TODO 这里可以编写从保存的数据中加载的数据用于给ItemDicArray和ItemDicList赋值,保证后面UI界面信息可以有数据初始化
            ItemDicArray.Add(ConfigInventory.ActionBar, new InventoryItem[10]);
            ItemDicArray.Add(ConfigInventory.PalayerBag, new InventoryItem[16]);
            //ConfigEvent.BeforeSceneUnloadEvent.AddEventListener<string,int>(UpdateSlotHightLight);//切换场景的时候触发下

            //事件监听
            ConfigEvent.UpdateSlotHightLight.AddEventListener<string, int>(UpdateSlotHightLight);//监听高亮事件
        }

        //ItemDicList字典操作
        public bool AddItemDicList(string key, Item item, bool toDestory = false)//捡东西可以设置ture
        {
            ItemDicList.TryGetValue(key, out List<InventoryItem> itemList);
            if (itemList == null)
            {
                ACDebug.Error($"添加{key}失败,字典中没有包含{key}的索引,请调用CreatItemDicListRecord()方法创建");
                return false;
            }
            int index1 = GetItemIndexItemDicList(key, item.itemID);//是否存在这个物品
            int index2 = CheckCapacityList(key);//检查空位

            if (index1 == -1)//没有物品
            {
                InventoryItem inventoryItem = new InventoryItem() { itemID = item.itemID, itemAmount = item.itemAmount };
                if (index2 == -1)//-1没有空位
                    itemList[index2] = inventoryItem;
                else
                    itemList.Add(inventoryItem);
            }
            else
            {
                InventoryItem inventoryItem1 = itemList[index1];
                inventoryItem1.itemAmount += item.itemAmount;
                itemList[index1] = inventoryItem1;
            }
            if (toDestory)
                GameObject.Destroy(item.gameObject);
            //更新物品UI 呼叫事件中心,执行委托的代码
            ConfigEvent.UpdateInvenoryUI.EventTrigger(key, ItemDicList[key]);
            return true;
        }
        public void CreatItemDicListRecord(string key)
        {
            ItemDicList.Add(key, new List<InventoryItem>());
        }
        private int GetItemIndexItemDicList(string key, int ID)
        {
            ItemDicList.TryGetValue(key, out List<InventoryItem> itemList);
            for (int i = 0; i < itemList?.Count; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == ID)
                    return i;
            }
            return -1;
        }//是否存在这个物品
        private int CheckCapacityList(string key)
        {
            ItemDicList.TryGetValue(key, out List<InventoryItem> itemList);
            for (int i = 0; i < itemList.Count; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == 0)
                    return i;
            }
            return -1;
        }//检查空位

        //ItemDicArray字典操作
        public bool AddItemDicArray(string key, Item item, bool toDestory = false)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] inventoryItemArray);
            if (inventoryItemArray == null)
            {
                ACDebug.Error($"添加{key}失败,字典中没有包含{key}的索引,请调用CreatItemDicArrayRecord()方法创建");
                return false;
            }

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
            key.EventTrigger(ItemDicArray[key]);//这里的在比如背包页面那边开启的时候监听
            return true;
        }
        public void CreatItemDicArrayRecord(string key, int count)
        {
            ItemDicArray.Add(key, new InventoryItem[count]);
        }//创建
        public bool RemoveItemDicArray(string key,int itemID ,int itemAmount)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] inventoryItemArray);
            if (inventoryItemArray == null) return false;
            int index1 = GetItemIndexArray(key, itemID);
            if (index1 == -1)//没有物品
                return false;
            else
            {
                inventoryItemArray[index1].itemAmount -= itemAmount;
                if (inventoryItemArray[index1].itemAmount==0)
                    inventoryItemArray[index1] = new InventoryItem();
            }

            key.EventTrigger(inventoryItemArray);
            return true;
        }//删除
        public bool ChangeItemDicArray(string oldKey, string newKey, int oldIndex, int newIndex)
        {
            ItemDicArray.TryGetValue(oldKey, out InventoryItem[] oldInventoryItemArray);
            if (oldInventoryItemArray == null)
            {
                ACDebug.Error($"老的{oldKey}是空的,请先创建");
                return false;
            }
            ItemDicArray.TryGetValue(newKey, out InventoryItem[] newInventoryItemArray);
            if (newInventoryItemArray == null)
            {
                ACDebug.Error($"新的{oldKey}是空的,请先创建");
                return false;
            }
            InventoryItem currentItem = oldInventoryItemArray[oldIndex];
            InventoryItem targetItem = newInventoryItemArray[newIndex];
            //数据交换
            if (targetItem.itemID != 0)//交换的目标有物品的情况下
            {
                oldInventoryItemArray[oldIndex] = targetItem;
                newInventoryItemArray[newIndex] = currentItem;
            }
            else
            {
                oldInventoryItemArray[oldIndex] = targetItem;
                newInventoryItemArray[newIndex] = currentItem;// new InventoryItem();
            }

            //if (oldInventoryItemArray[oldIndex].itemID == newInventoryItemArray[newIndex].itemID)//说明拖拽放下的是同一个物体
            //{
            //    oldInventoryItemArray[oldIndex].itemAmount += newInventoryItemArray[newIndex].itemAmount;
            //    newInventoryItemArray[newIndex].itemID = 0;
            //    newInventoryItemArray[newIndex].itemAmount = 0;
            //}
            oldKey.EventTrigger(ItemDicArray[oldKey]);//这里的在比如背包页面那边开启的时候监听
            newKey.EventTrigger(ItemDicArray[newKey]);//这里的在比如背包页面那边开启的时候监听
            return true;
        }
        public InventoryItem[] GetItemListArray(string key)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] inventoryItemArray);
            return inventoryItemArray;
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

        //ItemDicList和ItemDicArray交换 TODO 需要编写代码 
        public bool ChangeItem(string oldKey, string newKey, int oldIndex, int newIndex)
        {
            //ItemDicArray交换
            bool ChangeItemDicArrayIsOk = ChangeItemDicArray(oldKey, newKey, oldIndex, newIndex);
            if (ChangeItemDicArrayIsOk==false) { ACDebug.Log("temDicArray交换失败,进入ItemDicList交换."); }
            //TODO 后面继续写ItemDicList交换和上面差不多
            return ChangeItemDicArrayIsOk;
        }
        //public void UpdateItemInfos()
        //{

        //}//更新所有的物品信息

        //高亮格子添加  TODO 需要编写代码
        public void AddSlotUIList(string key, List<SlotUI> slotUIs)
        {
            if (slotUIDic.ContainsKey(key))
                ACDebug.Log("已经添加过了,请勿重复添加");
            else
                slotUIDic.Add(key, slotUIs);
        }
        public void UpdateSlotHightLight(string key = "", int index = -1)//-1全都不显示
        {
            foreach (KeyValuePair<string, List<SlotUI>> slotUI in slotUIDic)
            {
                if (slotUI.Key == key)
                {
                    slotUI.Value.ForEach(slot =>
                    {
                        if (slot.isSelected && slot.slotIndex == index)
                        {
                            slot.slotHightLight.gameObject.SetActive(true);
                        }
                        else
                        {
                            slot.isSelected = false;
                            slot.slotHightLight.gameObject.SetActive(false);
                        }
                    });
                }
                else
                {
                    slotUI.Value.ForEach(slot => { slot.isSelected = false; slot.slotHightLight.gameObject.SetActive(false); });
                }
            }
            ////清空所有高亮
            //foreach (List<SlotUI> slotUI in slotUIDic.Values)
            //{
            //    foreach (SlotUI slot in slotUI)
            //    {
            //        slot.isSelected = false;
            //        slot.slotHightLight.gameObject.SetActive(false);
            //    }
            //}
            //if (index == -1) return;
            ////显示高亮
            //slotUIDic.TryGetValue(key, out List<SlotUI> slotUIList);
            //foreach (SlotUI slot in slotUIList)
            //{
            //    if (slot.isSelected && slot.slotIndex == index)
            //        slot.slotHightLight.gameObject.SetActive(true);
            //}
        }//显示高亮

        //获取物品信息
        public ItemDetails GetItem(int id)
        {
            return DataManager.Instance.GetDataOne<ItemDetails>(id);
        }
    }
}
