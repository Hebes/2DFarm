using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    物品提示窗口

-----------------------*/

namespace Farm2D
{
    public class UIItemToolTipPanel : UIBase
    {
        private Text nameText;           //名称
        private Text typeText;           //类型
        private Text descriptionText;    //描述
        private Text valueText;                     //价格
        private GameObject bottomPart;              //底部
        private Transform itemToolTip;              //底部
        private GameObject resourcesPanel;          //资源面板

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Mobile, EUIMode.Normal, EUILucenyType.Pentrate);

            UIComponent UIComponent = panelGameObject.GetComponent<UIComponent>();

            GameObject T_ItemToolTip = UIComponent.Get<GameObject>("T_ItemToolTip");
            GameObject T_ResourcesPanel = UIComponent.Get<GameObject>("T_ResourcesPanel");
            GameObject T_Value = UIComponent.Get<GameObject>("T_Value");
            GameObject T_Bottom = UIComponent.Get<GameObject>("T_Bottom");
            GameObject T_Description = UIComponent.Get<GameObject>("T_Description");
            GameObject T_Type = UIComponent.Get<GameObject>("T_Type");
            GameObject T_Name = UIComponent.Get<GameObject>("T_Name");

            nameText = T_Name.GetText();
            itemToolTip = T_ItemToolTip.transform;
            typeText = T_Type.GetText();
            descriptionText = T_Description.GetText();
            bottomPart = T_Bottom.gameObject;
            valueText = T_Value.GetText();
            resourcesPanel = T_ResourcesPanel;

            ConfigEvent.ItemToolTipShow.AddEventListener<ItemDetailsData, string, Vector3>(SetupTooltip);
            ConfigEvent.ItemToolTipClose.AddEventListener(CloseUIForm);
        }



        /// <summary>
        /// 设置提示工具信息
        /// </summary>
        /// <param name="itemDatails"></param>
        /// <param name="configInventoryKey"></param>
        /// <param name="vector3"></param>
        private void SetupTooltip(ItemDetailsData itemDatails, string configInventoryKey, Vector3 vector3)
        {

            OpenUIForm<UIItemToolTipPanel>(ConfigUIPanel.UIItemToolTip);
            resourcesPanel.SetActive(false);
            itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);//设置锚点
            itemToolTip.position = vector3 + Vector3.up * 30;//设置距离

            nameText.text = itemDatails.name;
            typeText.text = GetItemType((EItemType)itemDatails.itemType);
            descriptionText.text = itemDatails.itemDescription;
            switch ((EItemType)itemDatails.itemType)
            {
                case EItemType.Seed:
                case EItemType.Commdity:
                    bottomPart.SetActive(true);
                    valueText.text = SetSellPrice(itemDatails, configInventoryKey).ToString();
                    break;
                case EItemType.Furniture:
                    bottomPart.SetActive(true);
                    valueText.text = SetSellPrice(itemDatails, configInventoryKey).ToString();
                    ShowResourcesPanel(itemDatails.itemID);
                    break;
                case EItemType.HoeTool:
                case EItemType.ChopTool:
                case EItemType.BreakTool:
                case EItemType.ReapTool:
                case EItemType.WaterTool:
                case EItemType.CollectTool:
                case EItemType.ReapableSceney:
                    bottomPart.SetActive(false);
                    break;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(panelGameObject.transform as RectTransform);//强制刷新,防止descriptionText描述延迟
        }

        /// <summary>
        /// 显示资源面板
        /// </summary>
        private void ShowResourcesPanel(int id)
        {
            //获取数据
            BluePrintDetails bluePrintDetails = ModelBuild.Instance.GetDataOne(id);
            resourcesPanel.SetActive(bluePrintDetails != null);
            for (int i = 0; i < resourcesPanel.transform.childCount; i++)
            {
                bool isInData = bluePrintDetails.resourceItem.Length > i;               //在数据中
                GameObject childGo = resourcesPanel.transform.GetChild(i).gameObject;   //子物体
                childGo.SetActive(isInData);                                            //设置子物体是否关闭和开启
                if (isInData)
                {
                    resourcesPanel.transform.GetChild(i).gameObject.SetActive(true);
                    //设置resourcesPanel子物体的数据
                    InventoryItem item = bluePrintDetails.resourceItem[i];
                    ItemDetailsData itemDetailsData = DataExpansion.GetDataOne<ItemDetailsData>(item.itemID);
                    childGo.GetImage().sprite = LoadResExtension.LoadOrSub<Sprite>(itemDetailsData.itemIconPackage, itemDetailsData.itemIcon);
                    childGo.GetChildComponent<TextMeshProUGUI>("ResourcesItemCount").text= item.itemAmount.ToString();
                }
            }
        }


        /// <summary>
        /// 设置出售价格
        /// </summary>
        /// <param name="itemDatails"></param>
        /// <param name="configInventoryKey"></param>
        /// <returns></returns>
        private int SetSellPrice(ItemDetailsData itemDatails, string configInventoryKey)
        {
            int price = itemDatails.itemPrice;
            switch (configInventoryKey)
            {
                case ConfigEvent.ActionBar:
                case ConfigEvent.PalayerBag:
                    return (int)(price * itemDatails.sellPercentage);
                default:
                    return price;
            }
        }

        /// <summary>
        /// 把物品类型转换成字符串
        /// </summary>
        /// <param name="EItemType"></param>
        /// <returns></returns>
        private string GetItemType(EItemType EItemType)
        {
            return EItemType switch
            {
                EItemType.Seed => "种子",
                EItemType.Commdity => "商品",
                EItemType.Furniture => "家具",
                EItemType.HoeTool => "锄头",
                EItemType.ChopTool => "砍树工具",
                EItemType.BreakTool => "砸石头工具",
                EItemType.ReapTool => "割草工具",
                EItemType.WaterTool => "浇水工具",
                EItemType.CollectTool => "收割工具",
                EItemType.ReapableSceney => "杂草",
                _ => "无"
            };
        }
    }
}
