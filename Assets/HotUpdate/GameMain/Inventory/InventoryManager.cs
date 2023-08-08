using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    物品管理类

-----------------------*/

namespace ACFrameworkCore
{
    public class InventoryManager : ICore
    {

        public static InventoryManager Instance;
        public List<ItemDetails> itemDetailsList;
        public List<InventoryItem> PlayerBagItemList;//玩家背包数量

        public async void ICroeInit()
        {
            Instance = this;
            Debug.Log("开始获取数据执行");
            if (MonoManager.Instance == null)
                Debug.Log("空了");
            await Start11();
        }

        IEnumerator Start11()
        {
            ResourcePackage package = YooAssets.GetPackage("PC");
            RawFileOperationHandle handle = package.LoadRawFileAsync(ConfigBytes.BytesItemDetails);
            yield return handle;
            byte[] fileData = handle.GetRawFileData();
            string fileText = handle.GetRawFileText();
            string filePath = handle.GetRawFilePath();

            itemDetailsList = BinaryAnalysis.LoopGetData<ItemDetails>(fileData);
            foreach (ItemDetails info in itemDetailsList)
            {
                Debug.Log(info.name);
            }
        }

        public ItemDetails GetItem(int ID)
        {
            return itemDetailsList.Find(i => i.itemID == ID);
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
            //EventHandler.CallUpdateInventoryUI(EInventoryLocation.Player, PlayerBag.itemList);//刷新UI
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

        public void SwapItem(int slotIndex,int targetIndex)
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
