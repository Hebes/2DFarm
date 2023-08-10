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
        public GameObject T_SlotHolder { get; set; }
        public GameObject T_MoneyText { get; set; }

        private List<SlotUI> playerBagSlotList;//背包的格子

        public override void UIAwake()
        {
            base.UIAwake();
            playerBagSlotList = new List<SlotUI>();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);

            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();


            T_SlotHolder = UIComponent.Get<GameObject>("T_SlotHolder");
            T_MoneyText = UIComponent.Get<GameObject>("T_MoneyText");

            for (int i = 0; i < T_SlotHolder.transform.childCount; i++)
            {
                SlotUI slotUI = T_SlotHolder.transform.GetChild(i).GetComponent<SlotUI>();
                slotUI.UpdateEmptySlot();
                playerBagSlotList.Add(slotUI);
            }
        }
        public override void UIOnEnable()
        {
            base.UIOnEnable();
            ConfigInventory.PalayerBag.AddEventListener<InventoryItem[]>(RefreshItem);

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
    }
}
