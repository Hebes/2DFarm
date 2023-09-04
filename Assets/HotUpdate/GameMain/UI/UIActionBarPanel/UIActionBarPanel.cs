/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    工具栏

-----------------------*/

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace ACFrameworkCore
{
    public class UIActionBarPanel : UIBase
    {
        public GameObject T_BagButton;              //背包按钮
        public GameObject T_ActionBar;              //槽父物体
        private List<SlotUI> ActionBarSlotUIList;   //快捷键槽
        private bool bagOpened = false;             //背包是否被打开了

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);

            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            T_BagButton = UIComponent.Get<GameObject>("T_BagButton");
            T_ActionBar = UIComponent.Get<GameObject>("T_ActionBar");

            ActionBarSlotUIList = new List<SlotUI>();
            for (int i = 0; i < T_ActionBar.transform.childCount; i++)
            {
                if (i == 0) continue;//第0个是背包按钮
                SlotUI slotUI = T_ActionBar.GetChildComponent<SlotUI>(i);
                slotUI.slotIndex = i - 1;
                slotUI.configInventoryKey = ConfigInventory.ActionBar;//所属于的管理的Key
                ActionBarSlotUIList.Add(slotUI);
            }

            bagOpened = panelGameObject.activeSelf;//UI面板当前的显示状态
            ButtonOnClickAddListener(T_BagButton.name, T_BagButtonListener);

            InventoryAllSystem.Instance.AddSlotUIList(ConfigInventory.ActionBar, ActionBarSlotUIList);
            InventoryAllSystem.Instance.ItemDicArray.Add(ConfigInventory.ActionBar, new InventoryItem[10]);
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][0] = new InventoryItem() { itemID = 1002, itemAmount = 2 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][1] = new InventoryItem() { itemID = 1015, itemAmount = 300 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][2] = new InventoryItem() { itemID = 1006, itemAmount = 1 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][3] = new InventoryItem() { itemID = 1007, itemAmount = 10 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][4] = new InventoryItem() { itemID = 1008, itemAmount = 10 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][5] = new InventoryItem() { itemID = 1009, itemAmount = 10 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][6] = new InventoryItem() { itemID = 1010, itemAmount = 10 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][7] = new InventoryItem() { itemID = 1005, itemAmount = 1 };

            InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][0] = new InventoryItem() { itemID = 1002, itemAmount = 1 };
            InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][1] = new InventoryItem() { itemID = 1016, itemAmount = 10 };
            InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][2] = new InventoryItem() { itemID = 1001, itemAmount = 1 };
            InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][3] = new InventoryItem() { itemID = 1018, itemAmount = 10 };
            InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][4] = new InventoryItem() { itemID = 1003, itemAmount = 1 };
            InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][5] = new InventoryItem() { itemID = 1032, itemAmount = 1 };
            InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][6] = new InventoryItem() { itemID = 1033, itemAmount = 1 };
        }
        public override void UIOnEnable()
        {
            base.UIOnEnable();
            ConfigInventory.ActionBar.AddEventListener<InventoryItem[]>(RefreshItem);
            InventoryItem[] playerBagItems = InventoryAllSystem.Instance.GetItemListArray(ConfigInventory.ActionBar);
            ConfigInventory.ActionBar.EventTrigger(playerBagItems);
        }
        public override void UIOnDisable()
        {
            base.UIOnDisable();
            ConfigInventory.ActionBar.RemoveEventListener<InventoryItem[]>(RefreshItem);
        }
        public override void UIUpdate()
        {
            base.UIUpdate();
            if (Input.GetKeyDown(KeyCode.B))
                T_BagButtonListener(null);
        }


        /// <summary> 背包按钮监听 </summary>
        private void T_BagButtonListener(GameObject go)
        {
            if (bagOpened)
            {
                bagOpened = false;
                OpenUIForm<UIPlayerBagPanel>(ConfigUIPanel.UIPlayerBag);
            }
            else
            {
                bagOpened = true;
                CloseOtherUIForm(ConfigUIPanel.UIPlayerBag);
            }
        }
        /// <summary> 刷新界面 </summary>
        private void RefreshItem(InventoryItem[] obj)
        {
            for (int i = 0; i < obj?.Length; i++)
            {
                if (obj[i].itemAmount > 0)//有物品
                {
                    ItemDetailsData item = InventoryAllSystem.Instance.GetItem(obj[i].itemID);
                    ActionBarSlotUIList[i].UpdateSlot(item, obj[i].itemAmount).Forget();
                }
                else
                {
                    ActionBarSlotUIList[i].UpdateEmptySlot();
                }
            }
        }
    }
}
