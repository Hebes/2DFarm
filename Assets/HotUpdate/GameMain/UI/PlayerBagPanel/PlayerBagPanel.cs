/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    玩家背包面板

-----------------------*/

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
            //初始化
            playerBagSlotList = new List<SlotUI>();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);
            //获取变量
            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            T_SlotHolder = UIComponent.Get<GameObject>("T_SlotHolder");
            T_MoneyText = UIComponent.Get<GameObject>("T_MoneyText");
            //添加数据
            for (int i = 0; i < T_SlotHolder.transform.childCount; i++)
            {
                SlotUI slotUI = T_SlotHolder.GetChildComponent<SlotUI>(i);
                slotUI.slotIndex = i;
                slotUI.key = ConfigInventory.PalayerBag;
                playerBagSlotList.Add(slotUI);
            }
            //设置变量
            InventoryAllManager.Instance.AddSlotUIList(ConfigInventory.PalayerBag, playerBagSlotList);
        }
        public override void UIOnEnable()
        {
            base.UIOnEnable();
            ConfigInventory.PalayerBag.AddEventListener<InventoryItem[]>(RefreshItem);
            InitItemInfo();
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
                    ItemDetails item = InventoryAllManager.Instance.GetItem(obj[i].itemID);
                    playerBagSlotList[i].UpdateSlot(item, obj[i].itemAmount);
                }
                else
                {
                    playerBagSlotList[i].UpdateEmptySlot();
                }
            }
        }

        //初始化物品信息
        public void InitItemInfo()
        {
            //获取物品信息
            InventoryItem[] playerBagItems = InventoryAllManager.Instance.GetItemListArray(ConfigInventory.PalayerBag);
            //刷新界面
            RefreshItem(playerBagItems);
        }
    }
}
