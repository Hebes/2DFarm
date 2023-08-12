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


        public override async void UIAwake()
        {
            base.UIAwake();
            //初始化
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);
            ActionBarSlotUIList = new List<SlotUI>();
            ////TODO 这里可以编写从保存的数据中加载的数据用于给ItemDicArray和ItemDicList赋值,保证后面UI界面信息可以有数据初始化
            //InventoryAllManager.Instance.CreatItemDicArrayRecord(ConfigInventory.ActionBar, 10);//初始化背包数据
            //获取变量
            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            T_BagButton = UIComponent.Get<GameObject>("T_BagButton");
            T_ActionBar = UIComponent.Get<GameObject>("T_ActionBar");
            //添加数据
            for (int i = 0; i < T_ActionBar.transform.childCount; i++)
            {
                if (i == 0) continue;//第0个是背包按钮
                SlotUI slotUI = T_ActionBar.GetChildComponent<SlotUI>(i);
                slotUI.slotIndex = i - 1;
                slotUI.key = ConfigInventory.ActionBar;//所属于的管理的Key
                ActionBarSlotUIList.Add(slotUI);
            }
            //按钮监听
            RigisterButtonObjectEvent(T_BagButton.name, T_BagButtonMethod);
            //设置变量
            bagOpened = panelGameObject.activeSelf;//UI面板当前的显示状态
            InventoryAllManager.Instance.AddSlotUIList(ConfigInventory.ActionBar, ActionBarSlotUIList);
            //测试创建拾取的物体
            GameObject gameObject = await ResourceExtension.LoadAsyncUniTask<GameObject>(ConfigPrefab.ItemBasePrefab);

            GameObject go1 = GameObject.Instantiate(gameObject);
            Item item = go1.GetComponent<Item>();
            item.itemID = 1007;
            item.itemAmount = 3;

            GameObject go2 = GameObject.Instantiate(gameObject);
            Item item2 = go2.GetComponent<Item>();
            item2.itemID = 1008;
            item2.itemAmount = 6;

            GameObject go3 = GameObject.Instantiate(gameObject);
            Item item3 = go3.GetComponent<Item>();
            item3.itemID = 1015;
            item3.itemAmount = 119;
        }
        public override void UIOnEnable()
        {
            base.UIOnEnable();
            ConfigInventory.ActionBar.AddEventListener<InventoryItem[]>(RefreshItem);
            InitItemInfo();
        }
        public override void UIUpdate()
        {
            base.UIUpdate();
            if (Input.GetKeyDown(KeyCode.B))
                T_BagButtonMethod(null);
        }
        public override void UIOnDisable()
        {
            base.UIOnDisable();
            ConfigInventory.ActionBar.RemoveEventListener<InventoryItem[]>(RefreshItem);
        }

        private void T_BagButtonMethod(GameObject go)
        {
            if (bagOpened)
            {
                bagOpened = false;
                OpenUIForm<PlayerBagPanel>(ConfigUIPanel.UIPlayerBagPanel);
            }
            else
            {
                bagOpened = true;
                ACDebug.Log("开启界面" + ConfigUIPanel.UIPlayerBagPanel);
                CloseOtherUIForm(ConfigUIPanel.UIPlayerBagPanel);
            }
        }//背包按钮

        private void RefreshItem(InventoryItem[] obj)
        {
            for (int i = 0; i < obj?.Length; i++)
            {
                if (obj[i].itemAmount > 0)//有物品
                {
                    ItemDetails item = InventoryAllManager.Instance.GetItem(obj[i].itemID);
                    ActionBarSlotUIList[i].UpdateSlot(item, obj[i].itemAmount).Forget();
                }
                else
                {
                    ActionBarSlotUIList[i].UpdateEmptySlot();
                }
            }
        } //刷新界面
        public void InitItemInfo()
        {
            //获取物品信息
            InventoryItem[] playerBagItems = InventoryAllManager.Instance.GetItemListArray(ConfigInventory.ActionBar);
            //刷新界面
            RefreshItem(playerBagItems);
        } //初始化物品信息
    }
}
