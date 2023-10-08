using Core;
using Cysharp.Threading.Tasks;
using Farm2D;
using System.Collections.Generic;
using UnityEngine;
using Debug = Core.Debug;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    物品管理系统

-----------------------*/

namespace Farm2D
{
    public class ModelItem : IModelInit, ISaveable
    {
        public static ModelItem Instance;
        public Dictionary<string, List<InventoryItem>> ItemDic; //物品字典列表   两本字典的KEY请不要重复
        private Dictionary<string, List<SlotUI>> slotUIDic;       //所有的高亮的格子
        private string SaveKey = "物品总管理系统";

        public async UniTask ModelInit()
        {
            Instance = this;
            ItemDic = new Dictionary<string, List<InventoryItem>>();
            slotUIDic = new Dictionary<string, List<SlotUI>>();

            ConfigEvent.UIDisplayHighlighting.EventAdd<string, int>(UpdateSlotHightLight);//监听高亮事件
            ConfigEvent.HarvestAtPlayerPosition.EventAdd<string, int>(OnHarvestAtPlayerPosition);
            ConfigEvent.BuildFurniture.EventAdd<string, int, Vector3>(OnBuildFurnitureEvent);
            ConfigEvent.StartNewGameEvent.EventAdd<int>(OnStartNewGameEvent);
            //注册保存事件
            ISaveable saveable = this;
            saveable.RegisterSaveable();

            await UniTask.Yield();
        }


        //监听事件
        //收获
        private void OnHarvestAtPlayerPosition(string key, int ID)
        {
            AddItem(key, ID, 1);
        }
        /// <summary>
        /// 建造
        /// </summary>
        /// <param name="BuildKey">建造图纸所在的ksy</param>
        /// <param name="ID"></param>
        /// <param name="mousePos"></param>
        /// <param name="keys">需要从哪里移除</param>
        private void OnBuildFurnitureEvent(string BuildKey, int ID, Vector3 mousePos)
        {
            //删除图纸
            RemoveItem(BuildKey, ID, 1);
            //获取建造蓝图数据
            BluePrintDetails bluePrint = ModelBuild.Instance.GetDataOne(ID);
            foreach (InventoryItem item in bluePrint.resourceItem)
            {
                // TODO 删除，后续判断所有资源是否合格
                RemoveItem(ConfigEvent.ActionBar, item.itemID, item.itemAmount);//删除资源
            }
        }
        private void OnStartNewGameEvent(int obj)
        {
            //ItemDic.Clear();
        }


        //ItemDicArray字典操作
        public bool AddItem(string key, int itemID, int itemAmount)
        {
            ItemDic.TryGetValue(key, out List<InventoryItem> inventoryItemArray);
            if (inventoryItemArray == null)
            {
                Debug.Error($"添加{key}失败,字典中没有包含{key}的索引,请调用CreatItemDicArrayRecord()方法创建");
                return false ;
            }

            int index1 = GetItemIndex(key, itemID);   //是否存在这个物品-1 没有 其他表示有
            int index2 = CheckCapacity(key);               //是否有空位

            if (index1 == -1)//没有物品
            {
                if (index2 == -1) return false ;//-1没有空位
                inventoryItemArray[index2] = new InventoryItem() { itemID = itemID, itemAmount = itemAmount };
            }
            else
            {
                InventoryItem ttt = inventoryItemArray[index1];
                ttt.itemAmount += itemAmount;
                inventoryItemArray[index1] = ttt;
            }
            //更新物品UI 呼叫事件中心,执行委托的代码
            RefreshItem(key);//这里的在比如背包页面那边开启的时候监听
            return true ;
        }
        public void CreatItemData(string key, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                InventoryItem inventoryItem = new InventoryItem();
                if (ItemDic.ContainsKey(key))
                    ItemDic[key].Add(inventoryItem);
                else
                    ItemDic.Add(key, new List<InventoryItem>() { inventoryItem });
            }
            RefreshItem(key);
        }
        public void RefreshItem(string key)
        {
            //更新物品UI 呼叫事件中心,执行委托的代码
            key.EventTrigger(ItemDic[key]);//这里的在比如背包页面那边开启的时候监听
        }
        public InventoryItem GetItem(string key, int ID)
        {
            ItemDic.TryGetValue(key, out List<InventoryItem> itemList);
            for (int i = 0; i < itemList?.Count; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == ID)
                    return inventoryItem;
            }
            return default;
        }
        public bool RemoveItem(string key, int itemID, int itemAmount)
        {
            if (ItemDic.ContainsKey(key) == false) return false ;
            ItemDic.TryGetValue(key, out List<InventoryItem> inventoryItemArray);
            int index1 = GetItemIndex(key, itemID);
            if (index1 == -1)
            {
                Debug.Error($"没有这个物品");
                return false ;
            }
            else
            {
                InventoryItem inventoryItem = inventoryItemArray[index1];
                if (itemAmount > inventoryItem.itemAmount)
                {
                    Debug.Log($"请检查需要移除的物品数量{itemID}");
                    return false;
                }
                inventoryItem.itemAmount -= itemAmount;
                inventoryItemArray[index1] = inventoryItem;
                if (inventoryItemArray[index1].itemAmount == 0)
                    inventoryItemArray[index1] = new InventoryItem();
            }
            RefreshItem(key);
            return true;
        }
        public void ChangeItem(string oldKey, string newKey, int oldIndex, int newIndex)
        {
            if (ItemDic.ContainsKey(oldKey)==false)
            {
                Debug.Error($"老的{oldKey}是空的,请先创建");
                return;
            }
            if (ItemDic.ContainsKey(newKey)==false)
            {
                Debug.Error($"老的{newKey}是空的,请先创建");
                return;
            }

            ItemDic.TryGetValue(oldKey, out List<InventoryItem> oldInventoryItemArray);
            ItemDic.TryGetValue(newKey, out List<InventoryItem> newInventoryItemArray);
           
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
            RefreshItem(oldKey);
            RefreshItem(newKey);
        }
        public List<InventoryItem> GetItemList(string key)
        {
            ItemDic.TryGetValue(key, out List<InventoryItem> inventoryItemArray);
            return inventoryItemArray;
        }
        public bool ChackKey(string key)
        {
            return ItemDic.ContainsKey(key);
        }
        //是否存在这个物品
        private int GetItemIndex(string key, int ID)
        {
            ItemDic.TryGetValue(key, out List<InventoryItem> itemList);
            for (int i = 0; i < itemList?.Count; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == ID)
                    return i;
            }
            return -1;
        }
        //检查空位
        private int CheckCapacity(string key)
        {
            ItemDic.TryGetValue(key, out List<InventoryItem> itemList);
            for (int i = 0; i < itemList?.Count; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == 0)
                    return i;
            }
            return -1;
        }
        public ItemDetailsData GetItemInfo(int id)
        {
            return id.GetDataOne<ItemDetailsData>();
        }


        //获取格子的信息
        public SlotUI GetSlotUIInfo(string key, int ID)
        {
            slotUIDic.TryGetValue(key, out List<SlotUI> slotUIs);
            return slotUIs.Find((slotUI) => { return slotUI.itemDatails.itemID == ID; });
        }
        //高亮格子添加  TODO 需要编写代码
        public void AddSlotUIList(string key, List<SlotUI> slotUIs)
        {
            if (slotUIDic.ContainsKey(key))
                Debug.Log("已经添加过了,请勿重复添加");
            else
                slotUIDic.Add(key, slotUIs);
        }
        public void RemoveSlotUIDic(string key)
        {
            foreach (SlotUI item in slotUIDic[key])
                GameObject.Destroy(item.gameObject);
            slotUIDic.Remove(key);
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
                            slot.slotHightLight.SetActive(true);
                        }
                        else
                        {
                            slot.isSelected = false;
                            slot.slotHightLight.SetActive(false);
                        }
                    });
                }
                else
                {
                    slotUI.Value.ForEach(slot => { slot.isSelected = false; slot.slotHightLight.SetActive(false); });
                }
            }
        }
        /// <summary>
        /// 商店交易物品
        /// </summary>
        /// <param name="itemDetails">物品信息</param>
        /// <param name="amount">交易数量</param>
        /// <param name="isSellTrade">是否卖东西</param>
        public void TradeItem(string oldKey, string newKey, int id, int amount, bool isSellTrade)
        {
            //获取数据信息
            ItemDetailsData itemDetails = id.GetDataOne<ItemDetailsData>();
            int cost = itemDetails.itemPrice * amount;//一共价格
            //获得旧物品位置
            int index = GetItemIndex(oldKey, itemDetails.itemID);

            if (isSellTrade)    //卖
            {
                if (ItemDic.ContainsKey(oldKey) == false) return;
                ItemDic.TryGetValue(oldKey, out List<InventoryItem> inventoryItems);
                if (inventoryItems[index].itemAmount >= amount)
                {
                    RemoveItem(oldKey, itemDetails.itemID, amount);
                    //卖出总价
                    cost = (int)(cost * itemDetails.sellPercentage);
                    ModelCommon.Instance.playerMoney += cost;
                }
            }
            else if (ModelCommon.Instance.playerMoney - cost >= 0)   //买
            {
                int indexTemp = CheckCapacity(newKey);
                if (indexTemp != -1)
                    AddItem(newKey, itemDetails.itemID, amount);
                ModelCommon.Instance.playerMoney -= cost;
            }
            //刷新钱
            ConfigEvent.MoneyShow.EventTrigger(ModelCommon.Instance.playerMoney);
        }



        //保存数据
        public string GUID => SaveKey;
        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.playerMoney = ModelCommon.Instance.playerMoney;
            saveData.ItemDic = ItemDic;
            return saveData;
        }
        public void RestoreData(GameSaveData saveData)
        {
            ModelCommon.Instance.playerMoney = saveData.playerMoney;
            ItemDic = saveData.ItemDic;
            foreach (string key in ItemDic.Keys)
                RefreshItem(key);
        }

       
    }
}
