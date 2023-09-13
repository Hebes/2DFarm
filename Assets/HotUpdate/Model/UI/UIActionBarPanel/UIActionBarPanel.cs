using ACFrameworkCore;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    快捷工具栏

-----------------------*/

namespace ACFarm
{
    public class UIActionBarPanel : UIBase
    {
        private List<SlotUI> ActionBarSlotUIList;   //快捷键槽
        private bool bagOpened = false;             //背包是否被打开了
        private string key;                         //管理的key

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);
            key = ConfigEvent.ActionBar;

            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            GameObject T_BagButton = UIComponent.Get<GameObject>("T_BagButton");//背包按钮
            GameObject T_ActionBar = UIComponent.Get<GameObject>("T_ActionBar");//槽父物体

            ActionBarSlotUIList = new List<SlotUI>();
            for (int i = 0; i < T_ActionBar.transform.childCount; i++)
            {
                if (i == 0) continue;//第0个是背包按钮
                SlotUI slotUI = T_ActionBar.GetChildComponent<SlotUI>(i);
                slotUI.slotIndex = i - 1;
                slotUI.ItemKey = key;//所属于的管理的Key
                ActionBarSlotUIList.Add(slotUI);
            }

            bagOpened = panelGameObject.activeSelf;//UI面板当前的显示状态
            ButtonOnClickAddListener(T_BagButton.name, T_BagButtonListener);

            key.AddEventListener<List<InventoryItem>>(RefreshItem);

            ItemManagerSystem.Instance.AddSlotUIList(key, ActionBarSlotUIList);//添加物品栏数据
            ItemManagerSystem.Instance.CreatItemData(key, 10);//添加背包数据

            //测试数据
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][0] = new InventoryItem() { itemID = 1002, itemAmount = 2 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][1] = new InventoryItem() { itemID = 1015, itemAmount = 300 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][2] = new InventoryItem() { itemID = 1006, itemAmount = 1 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][3] = new InventoryItem() { itemID = 1007, itemAmount = 10 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][4] = new InventoryItem() { itemID = 1008, itemAmount = 10 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][5] = new InventoryItem() { itemID = 1009, itemAmount = 10 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][6] = new InventoryItem() { itemID = 1010, itemAmount = 10 };
            //InventoryAllSystem.Instance.ItemDicArray[ConfigInventory.ActionBar][7] = new InventoryItem() { itemID = 1005, itemAmount = 1 };

            //ItemManagerSystem.Instance.ItemDic[ConfigEvent.ActionBar][0] = new InventoryItem() { itemID = 1002, itemAmount = 1 };
            //ItemManagerSystem.Instance.ItemDic[ConfigEvent.ActionBar][1] = new InventoryItem() { itemID = 1016, itemAmount = 10 };
            //ItemManagerSystem.Instance.ItemDic[ConfigEvent.ActionBar][2] = new InventoryItem() { itemID = 1001, itemAmount = 1 };
            //ItemManagerSystem.Instance.ItemDic[ConfigEvent.ActionBar][3] = new InventoryItem() { itemID = 1018, itemAmount = 10 };
            //ItemManagerSystem.Instance.ItemDic[ConfigEvent.ActionBar][4] = new InventoryItem() { itemID = 1003, itemAmount = 1 };
            //ItemManagerSystem.Instance.ItemDic[ConfigEvent.ActionBar][5] = new InventoryItem() { itemID = 1032, itemAmount = 1 };
            //ItemManagerSystem.Instance.ItemDic[ConfigEvent.ActionBar][6] = new InventoryItem() { itemID = 1033, itemAmount = 1 };
        }
        public override void UIOnEnable()
        {
            base.UIOnEnable();
            List<InventoryItem> playerBagItems = ItemManagerSystem.Instance.GetItemList(key);
            RefreshItem(playerBagItems);
        }

        public override void UIUpdate()
        {
            base.UIUpdate();
            if (Input.GetKeyDown(KeyCode.B))
                T_BagButtonListener(null);
        }


        /// <summary>
        /// 背包按钮监听
        /// </summary>
        /// <param name="go"></param>
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
                ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮
                ConfigEvent.PlayerAnimationsEvent.EventTrigger(1001, false);//通知物品被选中的状态,可以随便填写个数字书要是取消动画
                ConfigEvent.ItemSelectedEvent.EventTrigger(string.Empty, 0, false);//切换鼠标样式
            }
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
                    ActionBarSlotUIList[i].UpdateSlot(item.itemID, obj[i].itemAmount);
                }
                else
                {
                    ActionBarSlotUIList[i].UpdateEmptySlot();
                }
            }
        }
    }
}
