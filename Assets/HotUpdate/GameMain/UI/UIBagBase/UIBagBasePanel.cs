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

        /// <summary>
        /// 打开通用包裹UI事件
        /// </summary>
        /// <param name="Name">NPC或者玩家的名称</param>
        /// <param name="slotType"></param>
        private void OnBaseBagOpenEvent(string Name, string slotType)
        {
            GameObject prefab = null;
            switch (slotType)
            {
                case ConfigInventory.Shop: prefab = shopSlotPrefab; break;
                case ConfigInventory.Box: prefab = boxSlotPrefab; break;
            }

            //生成背包UI
            OpenUIForm<UIBagBasePanel>(ConfigUIPanel.UIBagBase);

            baseBagSlots = new List<SlotUI>();
            InventoryAllSystem.Instance.ItemDicArray.TryGetValue(Name, out InventoryItem[] shopDetailsDatasList);
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

                if (slotType.Equals(ConfigInventory.Shop))
                {
                    OpenUIForm<UIPlayerBagPanel>(ConfigUIPanel.UIPlayerBag);
                    UIPlayerBagPanel uIPlayerBagPanel = GetUIForm<UIPlayerBagPanel>(ConfigUIPanel.UIPlayerBag);
                    //uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().pivot = new Vector2(-1, 0.5f);
                    //uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().offsetMin = new Vector2(130f,0);
                    //uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().offsetMin = new Vector2(130f,0);
                    uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().offsetMin = new Vector2(130f, 0.0f);
                    uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().offsetMax = new Vector2(130f, 0.0f);

                    //bagOpened = true;
                }
                //刷新UI数据
                OnUpdateInventoryUI(slotType, shopDetailsDatasList.ToList());   
            }
        }

        /// <summary>
        /// 更新指定位置的Slot事件函数
        /// </summary>
        /// <param name="location">库存位置</param>
        /// <param name="list">数据列表</param>
        private void OnUpdateInventoryUI(string location, List<InventoryItem> InventoryItemList)
        {
            switch (location)
            {
                case ConfigInventory.Shop://如果是商店的格子
                    for (int i = 0; i < baseBagSlots.Count; i++)
                    {
                        if (InventoryItemList[i].itemAmount > 0)
                        {
                            ItemDetailsData item = InventoryAllSystem.Instance.GetItem(InventoryItemList[i].itemID);
                            baseBagSlots[i].UpdateSlot(item, InventoryItemList[i].itemAmount).Forget();
                        }
                        else
                        {
                            baseBagSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
            }
            //ConfigInventory.PalayerBag.EventTrigger()
            //switch (location)
            //{
            //    case ConfigInventory.PalayerBag:
            //        for (int i = 0; i < playerSlots.Length; i++)
            //        {
            //            if (list[i].itemAmount > 0)
            //            {
            //                var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
            //                playerSlots[i].UpdateSlot(item, list[i].itemAmount);
            //            }
            //            else
            //            {
            //                playerSlots[i].UpdateEmptySlot();
            //            }
            //        }
            //        break;
            //    case InventoryLocation.Box:
            //        for (int i = 0; i < baseBagSlots.Count; i++)
            //        {
            //            if (list[i].itemAmount > 0)
            //            {
            //                var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
            //                baseBagSlots[i].UpdateSlot(item, list[i].itemAmount);
            //            }
            //            else
            //            {
            //                baseBagSlots[i].UpdateEmptySlot();
            //            }
            //        }
            //        break;
            //}

            //playerMoneyText.text = InventoryManager.Instance.playerMoney.ToString();
        }

        /// <summary>
        /// 关闭通用包裹UI事件
        /// </summary>
        /// <param name="slotType"></param>
        /// <param name="bagData"></param>
        private void OnBaseBagCloseEvent(string Name, string slotType)
        {
            CloseUIForm();
            CloseOtherUIForm(ConfigUIPanel.UIItemToolTip);
            ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮

            foreach (var slot in baseBagSlots)
                GameObject.Destroy(slot.gameObject);
            baseBagSlots.Clear();

            if (slotType == ConfigInventory.Mira)
            {
                CloseOtherUIForm(ConfigUIPanel.UIPlayerBag);
                UIPlayerBagPanel uIPlayerBagPanel = GetUIForm<UIPlayerBagPanel>(ConfigUIPanel.UIPlayerBag);
                //uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
                uIPlayerBagPanel.panelGameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
                //bagOpened = false;
            }
        }
    }
}
