using System;
using UnityEngine;
using UnityEngine.UI;
using ACFrameworkCore;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	购买或者出售面板

-----------------------*/

namespace ACFarm
{
    public class UITradePanel : UIBase
    {

        public Image itemIcon;
        public Text itemName;
        public InputField tradeAmount;
        public Button submitButton;
        public Button cancelButton;
        public Text OKText;

        private ItemDetailsData item;
        private bool isSellTrade;
        private string oldKey;             //从哪个数据库存中过来的
        private string newKey;

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Lucency);

            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            GameObject T_ItemName = UIComponent.Get<GameObject>("T_ItemName");
            GameObject T_Icon = UIComponent.Get<GameObject>("T_Icon");
            GameObject T_Cancel = UIComponent.Get<GameObject>("T_Cancel");
            GameObject T_OK = UIComponent.Get<GameObject>("T_OK");
            GameObject T_OKText = UIComponent.Get<GameObject>("T_OKText");
            GameObject T_Count = UIComponent.Get<GameObject>("T_Count");

            itemIcon = T_Icon.GetImage();
            itemName = T_ItemName.GetText();
            tradeAmount = T_Count.GetInputField();
            submitButton = T_OK.GetButton();
            cancelButton = T_Cancel.GetButton();
            OKText = T_OKText.GetText();

            ButtonOnClickAddListener(T_OK.name, p => TradeItem());
            ButtonOnClickAddListener(T_Cancel.name, p => CancelTrade());

            ConfigEvent.ShowTradeUI.AddEventListener<string, string, int, bool>(SetupTradeUI);
        }

        /// <summary>
        /// 设置TradeUI显示详情
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isSell"></param>
        public void SetupTradeUI(string oldKey, string newKey, int itemID, bool isSell)
        {
            ItemDetailsData item = itemID.GetDataOne<ItemDetailsData>();
            OpenUIForm<UITradePanel>(ConfigUIPanel.UITrade);
            this.item = item;
            this.oldKey = oldKey;
            this.newKey = newKey;
            itemIcon.sprite = ResourceExtension.Load<Sprite>(item.itemIcon);
            itemName.text = item.name;
            isSellTrade = isSell;
            tradeAmount.text = string.Empty;
            OKText.text = isSell ? "出售" : "购买";
        }

        /// <summary>
        /// 购买
        /// </summary>
        private void TradeItem()
        {
            int amount = Convert.ToInt32(tradeAmount.text);
            ItemManagerSystem.Instance.TradeItem(oldKey, newKey, item.itemID, amount, isSellTrade);
            CancelTrade();
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void CancelTrade()
        {
            CloseUIForm();
        }
    }
}
