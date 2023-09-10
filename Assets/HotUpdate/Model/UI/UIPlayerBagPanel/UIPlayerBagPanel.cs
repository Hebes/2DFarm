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
using ACFrameworkCore;

namespace ACFarm
{
    public class UIPlayerBagPanel : UIBase
    {
        //public GameObject T_SlotHolder;
        public GameObject T_MoneyText;
        private List<SlotUI> playerBagSlotList;//背包的格子

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);

            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            GameObject T_SlotHolder = UIComponent.Get<GameObject>("T_SlotHolder");
            T_MoneyText = UIComponent.Get<GameObject>("T_MoneyText");

            InventoryAllSystem.Instance.ItemDicArray.Add(ConfigEvent.PalayerBag, new InventoryItem[16]);
            playerBagSlotList = new List<SlotUI>();
            for (int i = 0; i < T_SlotHolder.transform.childCount; i++)
            {
                SlotUI slotUI = T_SlotHolder.GetChildComponent<SlotUI>(i);
                slotUI.slotIndex = i;
                slotUI.configInventoryKey = ConfigEvent.PalayerBag;
                playerBagSlotList.Add(slotUI);
            }
            InventoryAllSystem.Instance.AddSlotUIList(ConfigEvent.PalayerBag, playerBagSlotList);
            ConfigEvent.PalayerBag.AddEventListener<InventoryItem[]>(RefreshItem);//这里触发的是从InventoryAllSystem的AddItemDicArray
            ConfigEvent.MoneyShow.AddEventListener<int>(ShowMoney);

            //添加测试数据
            InventoryAllSystem.Instance.ItemDicArray[ConfigEvent.PalayerBag][0] = new InventoryItem() { itemID = 1015, itemAmount = 80 };
            InventoryAllSystem.Instance.ItemDicArray[ConfigEvent.PalayerBag][1] = new InventoryItem() { itemID = 1014, itemAmount = 80 };
        }
        public override void UIOnEnable()
        {
            base.UIOnEnable();
            InventoryItem[] playerBagItems = InventoryAllSystem.Instance.GetItemListArray(ConfigEvent.PalayerBag);
            ConfigEvent.PalayerBag.EventTrigger(playerBagItems);

            //刷新金币显示
            ConfigEvent.MoneyShow.EventTrigger(CommonManagerSystem.Instance.playerMoney);
        }

        private void ShowMoney(int money)
        {
            T_MoneyText.GetTextMeshPro().text = money.ToString();
        }


        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshItem(InventoryItem[] obj)
        {
            for (int i = 0; i < obj?.Length; i++)
            {
                if (obj[i].itemAmount > 0)//有物品
                {
                    ItemDetailsData item = InventoryAllSystem.Instance.GetItem(obj[i].itemID);
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
