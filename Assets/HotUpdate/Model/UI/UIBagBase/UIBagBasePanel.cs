using ACFarm;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UI;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	商店和箱子界面

-----------------------*/

namespace ACFrameworkCore
{
    public class UIBagBasePanel : UIBase
    {
        private GameObject shopSlotPrefab;                                      //商品的预制体
        private List<SlotUI> baseBagSlots;                                      //正在出售的列表
        private Transform slotHolder;                                           //子物体生成后挂在的父物体
        public GameObject boxSlotPrefab;
        private string InventoryKey;                                            //存储物品Key
        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Normal, EUIMode.Normal, EUILucenyType.Lucency);
            //获取组件
            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            GameObject T_ESCImage = UIComponent.Get<GameObject>("T_ESCImage");
            GameObject T_Solt_ShopPreafab = UIComponent.Get<GameObject>("T_Solt_ShopPreafab");
            GameObject T_SlotHolder = UIComponent.Get<GameObject>("T_SlotHolder");
            shopSlotPrefab = T_Solt_ShopPreafab;
            slotHolder = T_SlotHolder.transform;

            //事件
            //ButtonOnClickAddListener(T_ESCImage.name, p => { CloseUIForm(); });
            ConfigEvent.BaseBagOpen.AddEventListener<string, string>(OnBaseBagOpenEvent);
            ConfigEvent.BaseBagClose.AddEventListener<string, string>(OnBaseBagCloseEvent);
        }

        public override void UIOnEnable()
        {
            base.UIOnEnable();

        }
        public override void UIOnDisable()
        {
            base.UIOnDisable();

        }

        /// <summary>
        /// 打开通用包裹UI事件
        /// </summary>
        /// <param name="Name">NPC或者玩家的名称，里面的数据</param>
        /// <param name="slotType"></param>
        private void OnBaseBagOpenEvent(string Name, string slotType)
        {
            Name.AddEventListener<InventoryItem[]>(OnUpdateInventoryUI);
            GameObject prefab = null;
            switch (slotType)
            {
                default:
                case ConfigEvent.Shop: prefab = shopSlotPrefab; break;
                case ConfigEvent.Box: prefab = boxSlotPrefab; break;
            }

            //生成背包UI
            OpenUIForm<UIBagBasePanel>(ConfigUIPanel.UIBagBase);

            baseBagSlots = new List<SlotUI>();
            //从仓管系统获取数据，请先提前吧仓管里面的数据初始化完毕!
            InventoryItem[] shopDetailsDatasList = ShopManagerSystemExpansion.GetShopData(Name);
            if (shopDetailsDatasList != null)
            {
                for (int i = 0; i < shopDetailsDatasList.ToList().Count; i++)
                {

                    SlotUI slot = GameObject.Instantiate(prefab, slotHolder).GetComponent<SlotUI>();
                    slot.gameObject.SetActive(true);
                    slot.slotIndex = i;
                    slot.configInventoryKey = Name;
                    baseBagSlots.Add(slot);
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate(slotHolder.GetComponent<RectTransform>());

                if (slotType.Equals(ConfigEvent.Shop))
                {
                    OpenUIForm<UIPlayerBagPanel>(ConfigUIPanel.UIPlayerBag);
                    UIPlayerBagPanel uIPlayerBagPanel = GetUIForm<UIPlayerBagPanel>(ConfigUIPanel.UIPlayerBag);
                    uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().offsetMin = new Vector2(130f, 0.0f);
                    uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().offsetMax = new Vector2(130f, 0.0f);
                }
                //刷新UI数据
                OnUpdateInventoryUI(shopDetailsDatasList);
            }
        }

        /// <summary>
        /// 更新指定位置的Slot事件函数
        /// </summary>
        /// <param name="location">库存位置</param>
        /// <param name="list">数据列表</param>
        private void OnUpdateInventoryUI(InventoryItem[] InventoryItemList)
        {
            //更新打卡物体信息
            for (int i = 0; i < baseBagSlots.Count; i++)
            {
                if (InventoryItemList[i].itemAmount > 0)
                {
                    ItemDetailsData item = InventoryAllSystem.Instance.GetItem(InventoryItemList[i].itemID);
                    baseBagSlots[i].UpdateSlot(item, InventoryItemList[i].itemAmount);
                }
                else
                {
                    baseBagSlots[i].UpdateEmptySlot();
                }
            }
        }

        /// <summary>
        /// 关闭通用包裹UI事件
        /// </summary>
        /// <param name="slotType"></param>
        /// <param name="bagData"></param>
        private void OnBaseBagCloseEvent(string Name, string slotType)
        {
            Name.RemoveEventListener<InventoryItem[]>(OnUpdateInventoryUI);
            CloseUIForm();
            CloseOtherUIForm(ConfigUIPanel.UIItemToolTip);
            ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮

            foreach (var slot in baseBagSlots)
                GameObject.Destroy(slot.gameObject);
            baseBagSlots.Clear();

            if (slotType == ConfigEvent.Mira)
            {
                CloseOtherUIForm(ConfigUIPanel.UIPlayerBag);
                UIPlayerBagPanel uIPlayerBagPanel = GetUIForm<UIPlayerBagPanel>(ConfigUIPanel.UIPlayerBag);
                uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
                uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
            }
        }
    }
}
