/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    工具栏

-----------------------*/

using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ACFrameworkCore
{
    public class ActionBarPanel : UIBase
    {
        public GameObject T_BagButton { get; set; }
        public GameObject T_ActionBar { get; set; }

        private List<SlotUI> ActionBarSlotUIList;

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);

            ActionBarSlotUIList = new List<SlotUI>();

            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            T_BagButton = UIComponent.Get<GameObject>("T_BagButton");
            T_ActionBar = UIComponent.Get<GameObject>("T_ActionBar");

            RigisterButtonObjectEvent(T_BagButton.name,
             p =>
             {
                 ACDebug.Log("开启界面"+ ConfigUIPanel.UIPlayerBagPanel);
                 OpenUIForm<PlayerBagPanel>(ConfigUIPanel.UIPlayerBagPanel);
             });

            for (int i = 0; i < T_ActionBar.transform.childCount; i++)
            {
                if (i == 0) continue;
                SlotUI slotUI = T_ActionBar.transform.GetChild(i).GetComponent<SlotUI>();
                slotUI.UpdateEmptySlot();
                ActionBarSlotUIList.Add(slotUI);
            }

            //InventoryItem[] inventoryItems = InventoryAllManager.Instance.ItemDicArray[ConfigInventory.ActionBar];
            GameObject gameObject = ResourceExtension.LoadAssetSync<GameObject>(ConfigPrefab.ItemBasePrefab);
            GameObject.Instantiate(gameObject);
        }
    }
}
