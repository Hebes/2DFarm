/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    玩家背包面板

-----------------------*/

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace ACFrameworkCore
{
    public class PlayerBagPanel : UIBase
    {
        public GameObject T_SlotHolder;
        public GameObject T_MoneyText;
        private List<SlotUI> playerBagSlotList;//背包的格子

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);

            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            T_SlotHolder = UIComponent.Get<GameObject>("T_SlotHolder");
            T_MoneyText = UIComponent.Get<GameObject>("T_MoneyText");

            InventoryAllSystem.Instance.ItemDicArray.Add(ConfigInventory.PalayerBag, new InventoryItem[16]);
            playerBagSlotList = new List<SlotUI>();
            for (int i = 0; i < T_SlotHolder.transform.childCount; i++)
            {
                SlotUI slotUI = T_SlotHolder.GetChildComponent<SlotUI>(i);
                slotUI.slotIndex = i;
                slotUI.configInventoryKey = ConfigInventory.PalayerBag;
                playerBagSlotList.Add(slotUI);
            }
            InventoryAllSystem.Instance.AddSlotUIList(ConfigInventory.PalayerBag, playerBagSlotList);
        }
        public override void UIOnEnable()
        {
            base.UIOnEnable();
            ConfigInventory.PalayerBag.AddEventListener<InventoryItem[]>(RefreshItem);//这里触发的是从InventoryAllSystem的AddItemDicArray
            InventoryItem[] playerBagItems = InventoryAllSystem.Instance.GetItemListArray(ConfigInventory.PalayerBag);
            RefreshItem(playerBagItems);
        }
        public override void UIOnDisable()
        {
            base.UIOnDisable();
            ConfigInventory.PalayerBag.RemoveEventListener<InventoryItem[]>(RefreshItem);
        }

        //刷新界面
        private void RefreshItem(InventoryItem[] obj)
        {
            for (int i = 0; i < obj?.Length; i++)
            {
                if (obj[i].itemAmount > 0)//有物品
                {
                    ItemDetails item = InventoryAllSystem.Instance.GetItem(obj[i].itemID);
                    playerBagSlotList[i].UpdateSlot(item, obj[i].itemAmount).Forget();
                }
                else
                {
                    playerBagSlotList[i].UpdateEmptySlot();
                }
            }
        }
    }
}
