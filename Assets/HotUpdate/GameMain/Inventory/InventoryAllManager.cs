using NUnit.Framework;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public class itemt
    {
        public int itemID;
        public int count;
    }

    public class InventoryAllManager : ICore
    {
        public static InventoryAllManager Instance;
        Dictionary<string, IList> inventoryDic;//所有物品的管理字典 key:比如背包 Value:背包里面的数据

        public List<IData> itemDetailsList;         //物品数据
        public List<InventoryItem> PlayerBagItemList;//玩家背包数量

        public void ICroeInit()
        {
            Instance = this;
            itemDetailsList = this.GetDataList<ItemDetails>();
            inventoryDic = new Dictionary<string, IList>();
            //inventoryDic.Add("t", new List<itemt>()
            //{
            //    new itemt() {itemID=1,count=1},
            //    new itemt() {itemID=2,count=1},
            //    new itemt() {itemID=3,count=1},
            //});
            //itemt[] ints = new itemt[50];
            //ints[0] = new itemt() { itemID = 1, count = 1 };
            //ints[1] = new itemt() { itemID = 2, count = 2 };
            //inventoryDic.Add("e", ints);
            //AddItemt("t", new itemt() { itemID = 4, count = 1 });
            //AddItemt("t", new itemt() { itemID = 3, count = 1 });

            //AddItemt("e", new itemt() { itemID = 3, count = 1 });
            //AddItemt("e", new itemt() { itemID = 1, count = 12 });
        }

        public bool AddItemt(string key, itemt t)
        {
            bool isAddSucceed = true;
            inventoryDic.TryGetValue(key, out IList objects);
            Type type = objects?.GetType();
            if (type == null) return false;
            //查找数据
            itemt item1 = null;
            for (int i = 0; i < objects?.Count; i++)
            {
                itemt item = objects[i] as itemt;
                if (item != null && item.itemID == t.itemID)
                {
                    item1 = item;
                    break;
                }
            }
            //数据操作
            if (type == typeof(itemt[]))
            {
                if (item1 != null)
                {
                    (objects[objects.IndexOf(item1)] as itemt).count += t.count;//存在
                }
                else
                {
                    int itemNullNumber = GetNullItem(key);
                    if (itemNullNumber == -1)
                        return false;
                    else
                        objects[itemNullNumber] = t;
                }
            }
            else if (type == typeof(List<itemt>))
            {
                if (item1 != null)
                    (objects[objects.IndexOf(item1)] as itemt).count += t.count;//存在
                else
                    objects.Add(t);//不存在的话
            }

            if (isAddSucceed)
                ConfigEvent.UpdateInvenoryUI.EventTrigger(key, inventoryDic[key]); //更新物品UI 呼叫事件中心,执行委托的代码
            return isAddSucceed;
        }
        public bool RemoveItemt(string key, itemt t)
        {
            bool isRemoveSucceed = true;
            inventoryDic.TryGetValue(key, out IList objects);
            Type type = objects?.GetType();
            if (type == null) return false;
            //查找数据
            itemt item1 = null;
            for (int i = 0; i < objects?.Count; i++)
            {
                itemt item = objects[i] as itemt;
                if (item != null && item.itemID == t.itemID)
                {
                    item1 = item;
                    break;
                }
            }
            //数据操作
            if (type == typeof(itemt[]))
            {
                if (item1 != null)
                {
                    if ((objects[objects.IndexOf(item1)] as itemt).count == t.count)
                    {
                        objects[objects.IndexOf(item1)] = null;
                    }
                    else if ((objects[objects.IndexOf(item1)] as itemt).count > t.count)
                    {
                        (objects[objects.IndexOf(item1)] as itemt).count -= t.count;
                    }
                    else if ((objects[objects.IndexOf(item1)] as itemt).count < t.count)
                    {
                        return false;
                    }
                }
            }
            else if (type == typeof(List<itemt>))
            {
                if (item1 != null)
                {
                    if ((objects[objects.IndexOf(item1)] as itemt).count == t.count)
                    {
                        objects.Remove(item1);
                    }
                    else if ((objects[objects.IndexOf(item1)] as itemt).count > t.count)
                    {
                        (objects[objects.IndexOf(item1)] as itemt).count -= t.count;
                    }
                    else if ((objects[objects.IndexOf(item1)] as itemt).count < t.count)
                    {
                        return false;
                    }
                }
            }

            if (isRemoveSucceed)
                ConfigEvent.UpdateInvenoryUI.EventTrigger(key, inventoryDic[key]); //更新物品UI 呼叫事件中心,执行委托的代码
            return isRemoveSucceed;
        }
        public itemt GetItemt(string key, itemt t)
        {
            itemt item1 = null;
            inventoryDic.TryGetValue(key, out IList objects);
            Type type = objects?.GetType();
            if (type == null) return item1;
            //查找数据
            for (int i = 0; i < objects?.Count; i++)
            {
                itemt item = objects[i] as itemt;
                if (item != null && item.itemID == t.itemID)
                    return item;
            }
            return null;
        }
        public bool ChangeItemt(string oldKey, string newKey, itemt oldt)
        {
            bool isChangeSucceed = true;

            itemt itemtTemp = GetItemt(oldKey, oldt);
            itemt itemt1 = new itemt();
            itemt1.itemID = itemtTemp.itemID;
            itemt1.count = itemtTemp.count;

            isChangeSucceed = RemoveItemt(oldKey, oldt);
            if (isChangeSucceed==false)
                ACDebug.Log("删除失败");
            isChangeSucceed = AddItemt(newKey, itemt1);
            if (isChangeSucceed == false)
                ACDebug.Log("添加失败");
            if (isChangeSucceed)
            {
                ConfigEvent.UpdateInvenoryUI.EventTrigger(oldKey, inventoryDic[oldKey]); //更新物品UI 呼叫事件中心,执行委托的代码
                ConfigEvent.UpdateInvenoryUI.EventTrigger(newKey, inventoryDic[newKey]); //更新物品UI 呼叫事件中心,执行委托的代码
            }
            return isChangeSucceed;
        }

        //查找空格子
        private int GetNullItem(string key)
        {
            inventoryDic.TryGetValue(key, out IList objects);
            Type type = objects?.GetType();
            if (type == null) return -1;
            if (type == typeof(itemt[]))
            {
                for (int i = 0; i < objects?.Count; i++)
                {
                    itemt item = objects[i] as itemt;
                    if (item == null || item.itemID == 0)
                        return i;
                }
            }
            return -1;
        }



        public ItemDetails GetItem(int ID)
        {
            return itemDetailsList.Find(i => i.GetId() == ID) as ItemDetails;
        }
        public void AddItem(Item item, bool toDestory)
        {
            //背包是否有该物品
            int index = GetItemIndexBag(item.itemID);
            AddItemAtItem(item.itemID, index, 1);
            if (toDestory)
                GameObject.Destroy(item.gameObject);

            //更新物品UI 呼叫事件中心,执行委托的代码
            ConfigEvent.UpdateInvenoryUI.EventTrigger(EInventoryLocation.Player, PlayerBagItemList);
        }
        /// <summary>
        /// 移除指定数量的背包物品
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <param name="removeAmoun">数量</param>
        private void RemoveItem(int ID, int removeAmoun)
        {
            int index = GetItemIndexBag(ID);
            if (PlayerBagItemList[index].itemAmount > removeAmoun)
            {
                int amount = PlayerBagItemList[index].itemAmount - removeAmoun;
                PlayerBagItemList[index] = new InventoryItem
                {
                    itemAmount = amount,
                    itemID = ID
                };
            }
            else if (PlayerBagItemList[index].itemAmount == removeAmoun)
            {
                PlayerBagItemList[index] = new InventoryItem();//清空 数量相减等于0 约等于没有物品了
            }
            //更新物品UI 呼叫事件中心,执行委托的代码
            ConfigEvent.UpdateInvenoryUI.EventTrigger(EInventoryLocation.Player, PlayerBagItemList);//刷新UI
        }
        /// <summary>
        /// 检查背包空位
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < PlayerBagItemList.Count; i++)
            {
                InventoryItem inventoryItem = PlayerBagItemList[i];
                if (inventoryItem.itemID == 0)
                    return true;
            }
            return false;
        }
        /// <summary>在指定背包序号位置添加物品</summary>
        /// <param name="ID">物品ID</param>
        /// <param name="index">物品序号</param>
        /// <param name="aumount">物品数量</param>
        private void AddItemAtItem(int ID, int index, int aumount)
        {
            if (index == -1 && CheckBagCapacity())//背包里面没有这个物品,同时背包有空位
            {
                InventoryItem item = new InventoryItem { itemID = ID, itemAmount = aumount };
                for (int i = 0; i < PlayerBagItemList.Count; i++)
                {
                    if (PlayerBagItemList[i].itemID == 0)//找到一个空位
                    {
                        PlayerBagItemList[i] = item;
                        break;
                    }
                }
            }
            else//背包里面有这个物品
            {
                int currentAmount = PlayerBagItemList[index].itemAmount + aumount;
                InventoryItem item = new InventoryItem { itemID = ID, itemAmount = currentAmount };
                PlayerBagItemList[index] = item;
            }
        }
        //交换功能
        public void SwapItem(int slotIndex, int targetIndex)
        {

        }
        #region 其他功能
        /// <summary>
        /// 通过物品ID找到背包已有物品位置
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private int GetItemIndexBag(int ID)
        {
            for (int i = 0; i < PlayerBagItemList.Count; i++)
            {
                InventoryItem inventoryItem = PlayerBagItemList[i];
                if (inventoryItem.itemID == ID)
                    return i;
            }
            return -1;
        }
        #endregion
    }
}
