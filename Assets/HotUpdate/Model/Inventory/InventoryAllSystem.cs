using ACFarm;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    物品总管理系统

-----------------------*/

namespace ACFrameworkCore
{
    public class InventoryAllSystem : ICore, ISaveable
    {
        //注意:SlotUI指的都是例如ItemDicArray或者ItemDicList的key
        public static InventoryAllSystem Instance;
        public Dictionary<string, InventoryItem[]> ItemDicArray; //物品字典列表   两本字典的KEY请不要重复
        private Dictionary<string, List<SlotUI>> slotUIDic;       //所有的高亮的格子
        private string SaveKey = "物品总管理系统";



        public void ICroeInit()
        {
            Instance = this;
            ItemDicArray = new Dictionary<string, InventoryItem[]>();
            slotUIDic = new Dictionary<string, List<SlotUI>>();

            ConfigEvent.UIDisplayHighlighting.AddEventListener<string, int>(UpdateSlotHightLight);//监听高亮事件
            ConfigEvent.HarvestAtPlayerPosition.AddEventListener<string, int>(OnHarvestAtPlayerPosition);
            ConfigEvent.BuildFurniture.AddEventListener<int, Vector3>(OnBuildFurnitureEvent);


            

            //注册保存事件
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }

        //监听事件
        private void OnHarvestAtPlayerPosition(string key, int ID)
        {
            bool AddOK = AddItemDicArray(key, ID, 1);

            if (!AddOK)
                ACDebug.Log($"添加失败");
        }
        private void OnBuildFurnitureEvent(int ID, Vector3 mousePos)
        {
            //删除图纸
            RemoveItemDicArray(ConfigEvent.ActionBar, ID, 1);
            //获取建造蓝图数据
            BluePrintDetails bluePrint = BuildManagerSystem.Instance.GetDataOne(ID);
            foreach (var item in bluePrint.resourceItem)
            {
                //删除资源
                RemoveItemDicArray(ConfigEvent.PalayerBag, item.itemID, item.itemAmount);
            }
        }



        //ItemDicArray字典操作
        public bool AddItemDicArray(string key, int itemID, int itemAmount)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] inventoryItemArray);
            if (inventoryItemArray == null)
            {
                ACDebug.Error($"添加{key}失败,字典中没有包含{key}的索引,请调用CreatItemDicArrayRecord()方法创建");
                return false;
            }

            int index1 = GetItemIndexArray(key, itemID);   //是否存在这个物品-1 没有 其他表示有
            int index2 = CheckCapacityArray(key);               //是否有空位
            if (index1 == -1)//没有物品
            {
                if (index2 == -1) return false;//-1没有空位
                inventoryItemArray[index2] = new InventoryItem() { itemID = itemID, itemAmount = itemAmount };
            }
            else
            {
                inventoryItemArray[index1].itemAmount += itemAmount;
            }
            //更新物品UI 呼叫事件中心,执行委托的代码
            key.EventTrigger(ItemDicArray[key]);//这里的在比如背包页面那边开启的时候监听
            return true;
        }
        public void CreatItemDicArrayRecord(string key, int count)
        {
            ItemDicArray.Add(key, new InventoryItem[count]);
        }
        public void RefreshItemDic(string key)
        {
            //更新物品UI 呼叫事件中心,执行委托的代码
            key.EventTrigger(ItemDicArray[key]);//这里的在比如背包页面那边开启的时候监听
        }
        public bool RemoveItemDicArray(string key, int itemID, int itemAmount)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] inventoryItemArray);
            if (inventoryItemArray == null) return false;
            int index1 = GetItemIndexArray(key, itemID);
            if (index1 == -1)//没有物品
                return false;
            else
            {
                inventoryItemArray[index1].itemAmount -= itemAmount;
                if (inventoryItemArray[index1].itemAmount == 0)
                    inventoryItemArray[index1] = new InventoryItem();
            }

            key.EventTrigger(inventoryItemArray);
            return true;
        }
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



        public InventoryItem GetData(string key, int ID)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] itemList);
            for (int i = 0; i < itemList?.Length; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == ID)
                    return inventoryItem;
            }
            return default;
        }

        //获取格子的信息
        public SlotUI GetSlotUIInfo(string key, int ID)
        {
            slotUIDic.TryGetValue(key, out List<SlotUI> slotUIs);
            return slotUIs.Find((slotUI) => { return slotUI.itemDatails.itemID == ID; });
        }
        //ItemDicList和ItemDicArray交换 TODO 需要编写代码 
        public bool ChangeItem(string oldKey, string newKey, int oldIndex, int newIndex)
        {
            //ItemDicArray交换
            bool ChangeItemDicArrayIsOk = ChangeItemDicArray(oldKey, newKey, oldIndex, newIndex);
            if (ChangeItemDicArrayIsOk == false) { ACDebug.Log("temDicArray交换失败,进入ItemDicList交换."); }
            //TODO 后面继续写ItemDicList交换和上面差不多
            return ChangeItemDicArrayIsOk;
        }
        //高亮格子添加  TODO 需要编写代码
        public void AddSlotUIList(string key, List<SlotUI> slotUIs)
        {
            if (slotUIDic.ContainsKey(key))
                ACDebug.Log("已经添加过了,请勿重复添加");
            else
                slotUIDic.Add(key, slotUIs);
        }
        //显示高亮（-1全都不显示）
        private void UpdateSlotHightLight(string key = "", int index = -1)
        {
            //关闭所有的
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
        }
        //获取物品信息
        public ItemDetailsData GetItem(int id)
        {
            return DataManager.Instance.GetDataOne<ItemDetailsData>(id);
        }
        /// <summary>
        /// 交易物品
        /// </summary>
        /// <param name="itemDetails">物品信息</param>
        /// <param name="amount">交易数量</param>
        /// <param name="isSellTrade">是否卖东西</param>
        public void TradeItem(string oldKey, string newKey, ItemDetailsData itemDetails, int amount, bool isSellTrade)
        {
            int cost = itemDetails.itemPrice * amount;//一共价格
            //获得物品位置
            int index = GetItemIndexArray(oldKey, itemDetails.itemID);

            if (isSellTrade)    //卖
            {
                ItemDicArray.TryGetValue(oldKey, out InventoryItem[] inventoryItems);
                if (inventoryItems[index].itemAmount >= amount)
                {
                    RemoveItemDicArray(oldKey, itemDetails.itemID, amount);
                    //卖出总价
                    cost = (int)(cost * itemDetails.sellPercentage);
                    CommonManagerSystem.Instance.playerMoney += cost;
                }
            }
            else if (CommonManagerSystem.Instance.playerMoney - cost >= 0)   //买
            {
                int indexTemp = CheckCapacityArray(newKey);
                if (indexTemp != -1)
                    AddItemDicArray(newKey, itemDetails.itemID, amount);
                CommonManagerSystem.Instance.playerMoney -= cost;
            }
            //刷新UI,添加的时候已经刷新了

            //刷新钱
            ConfigEvent.MoneyShow.EventTrigger(CommonManagerSystem.Instance.playerMoney);
        }



        //保存数据
        public string GUID => SaveKey;
        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.playerMoney = CommonManagerSystem.Instance.playerMoney;
            saveData.ItemDicArray = ItemDicArray;
            return saveData;
        }
        public void RestoreData(GameSaveData saveData)
        {
            CommonManagerSystem.Instance.playerMoney = saveData.playerMoney;
            if (ItemDicArray == null)
                ItemDicArray = new Dictionary<string, InventoryItem[]>();
            ItemDicArray = saveData.ItemDicArray;
            foreach (string key in ItemDicArray.Keys)
                RefreshItemDic(key);
        }
    }
}
