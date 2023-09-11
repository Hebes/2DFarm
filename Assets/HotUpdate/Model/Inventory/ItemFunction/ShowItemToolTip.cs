using UnityEngine;
using UnityEngine.EventSystems;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    物品弹窗描述
    RequireComponent(typeof(AAA)) 要求AAA必须已经在别处资源中实例化或者AddComponent过，否则Unity无法识别AAA为脚本而忽略处理。

-----------------------*/

namespace ACFrameworkCore
{
    [RequireComponent(typeof(SlotUI))]
    public class ShowItemToolTip: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private SlotUI slotUI
        {
            get
            {
                return GetComponent<SlotUI>();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slotUI.itemDatails != null)
                ConfigEvent.ItemToolTipShow.EventTrigger(slotUI.itemDatails, slotUI.ItemKey, transform.position);
            else
                ConfigEvent.ItemToolTipClose.EventTrigger();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            ConfigEvent.ItemToolTipClose.EventTrigger();
        }
    }
}
