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
    public class ActionBarPanel : UIBase
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
            InventoryAllSystem.Instance.AddSlotUIList(ConfigInventory.ActionBar, ActionBarSlotUIList);

            bagOpened = panelGameObject.activeSelf;//UI面板当前的显示状态

            ButtonOnClickAddListener(T_BagButton.name, T_BagButtonListener);

            InventoryAllSystem.Instance.ItemDicArray.Add(ConfigInventory.ActionBar, new InventoryItem[10]);
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

        private void T_BagButtonListener(GameObject go)
        {
            if (bagOpened)
            {
                bagOpened = false;
                OpenUIForm<PlayerBagPanel>(ConfigUIPanel.UIPlayerBagPanel);
            }
            else
            {
                bagOpened = true;
                CloseOtherUIForm(ConfigUIPanel.UIPlayerBagPanel);
            }
        }//背包按钮监听
        private void RefreshItem(InventoryItem[] obj)
        {
            for (int i = 0; i < obj?.Length; i++)
            {
                if (obj[i].itemAmount > 0)//有物品
                {
                    ItemDetails item = InventoryAllSystem.Instance.GetItem(obj[i].itemID);
                    ActionBarSlotUIList[i].UpdateSlot(item, obj[i].itemAmount).Forget();
                }
                else
                {
                    ActionBarSlotUIList[i].UpdateEmptySlot();
                }
            }
        } //刷新界面
    }
}
