using Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    玩家背包面板

-----------------------*/

namespace Farm2D
{
    public class UIPlayerBagPanel : UIBase
    {
        public TextMeshProUGUI moneyText;
        private List<SlotUI> playerBagSlotList;//背包的格子

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);
            playerBagSlotList = new List<SlotUI>();

            UIComponent UIComponent = panelGameObject.GetComponent<UIComponent>();

            GameObject T_Solt_Bag = UIComponent.Get<GameObject>("T_Solt_Bag");
            GameObject T_SlotHolder = UIComponent.Get<GameObject>("T_SlotHolder");
            GameObject T_MoneyText = UIComponent.Get<GameObject>("T_MoneyText");

            moneyText = T_MoneyText.GetTextMeshPro();
            T_Solt_Bag.SetActive(false);

            ModelItem.Instance.CreatItemData(ConfigEvent.PalayerBag, 16);
            for (int i = 0; i < 16; i++)
            {
                GameObject T_Solt_BagItem = GameObject.Instantiate(T_Solt_Bag, T_SlotHolder.transform);
                T_Solt_BagItem.SetActive(true);
                SlotUI slotUI = T_Solt_BagItem.GetComponent<SlotUI>();
                slotUI.slotIndex = i;
                slotUI.ItemKey = ConfigEvent.PalayerBag;
                playerBagSlotList.Add(slotUI);
            }
            ModelItem.Instance.AddSlotUIList(ConfigEvent.PalayerBag, playerBagSlotList);

            ConfigEvent.PalayerBag.EventAdd<List<InventoryItem>>(RefreshItem);//这里触发的是从InventoryAllSystem的AddItemDicArray
            ConfigEvent.MoneyShow.EventAdd<int>(ShowMoney);

            //添加测试数据
            //ItemManagerSystem.Instance.ItemDic[ConfigEvent.PalayerBag][0] = new InventoryItem() { itemID = 1015, itemAmount = 80 };
            //ItemManagerSystem.Instance.ItemDic[ConfigEvent.PalayerBag][1] = new InventoryItem() { itemID = 1014, itemAmount = 80 };
        }


        public override void UIOnEnable()
        {
            base.UIOnEnable();
            List<InventoryItem> playerBagItems = ModelItem.Instance.GetItemList(ConfigEvent.PalayerBag);
            RefreshItem(playerBagItems);
            ShowMoney(ModelCommon.Instance.playerMoney);
        }


        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshItem(List<InventoryItem> obj)
        {
            for (int i = 0; i < obj?.Count; i++)
            {
                if (obj[i].itemAmount > 0)//有物品
                {
                    ItemDetailsData item = obj[i].itemID.GetDataOne<ItemDetailsData>();
                    playerBagSlotList[i].UpdateSlot(item.itemID, obj[i].itemAmount);
                }
                else
                {
                    playerBagSlotList[i].UpdateEmptySlot();
                }
            }
        }

        /// <summary>
        /// 刷新金币显示
        /// </summary>
        /// <param name="money"></param>
        private void ShowMoney(int money)
        {
            moneyText.text = money.ToString();
        }
    }
}
