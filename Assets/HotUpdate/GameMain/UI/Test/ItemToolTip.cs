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

namespace ACFrameworkCore
{
    public class ItemToolTip : MonoBehaviour
    {
        private TextMeshProUGUI nameText;//名称
        private TextMeshProUGUI typeText;//类型
        private TextMeshProUGUI descriptionText;//描述
        private Text valueText;//价格
        private GameObject bottomPart;//底部

        /// <summary>设置提示工具信息</summary>
        public void SetupTooltip(ItemDetails itemDatails, ESlotType eSlotType)
        {
            nameText.text = itemDatails.name;
            typeText.text = GetItemType((EItemType)itemDatails.itemType);
            descriptionText.text = itemDatails.itemDescription;
            switch ((EItemType)itemDatails.itemType)
            {
                case EItemType.Seed:
                case EItemType.Commdity:
                case EItemType.Furniture:
                    bottomPart.SetActive(true);
                    valueText.text = SetSellPrice(itemDatails, eSlotType).ToString();
                    break;
                case EItemType.HoeTool:
                case EItemType.ChopTool:
                case EItemType.BreakTool:
                case EItemType.ReapTool:
                case EItemType.WaterTool:
                case EItemType.ClooectTool:
                case EItemType.ReapableSceney:
                    bottomPart.SetActive(false);
                    break;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);//强制刷新,防止descriptionText描述延迟
        }

        /// <summary>
        /// 设置出售价格
        /// </summary>
        /// <param name="itemDatails"></param>
        /// <param name="eSlotType"></param>
        /// <returns></returns>
        private int SetSellPrice(ItemDetails itemDatails, ESlotType eSlotType)
        {
            int price = itemDatails.itemPrice;

            return eSlotType switch
            {
                ESlotType.Bag => (int)(price * itemDatails.sellPercentage),
                ESlotType.Box => price,
                ESlotType.Shop => price,
                _ => price
            };
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
                EItemType.ClooectTool => "收割工具",
                EItemType.ReapableSceney => "杂草",
                _ => "无"
            };
        }
    }
}
