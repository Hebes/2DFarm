using UnityEngine;
using UnityEngine.EventSystems;

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
        private InventoryUI inventoryUI
        {
            get
            {
                return GetComponentInParent<InventoryUI>();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slotUI.itemDatails != null)
            {
                inventoryUI.itemToolTip.gameObject.SetActive(true);
                inventoryUI.itemToolTip.SetupTooltip(slotUI.itemDatails, slotUI.eSlotType);

                inventoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);//设置锚点
                inventoryUI.itemToolTip.transform.position = transform.position + Vector3.up * 60;//设置距离
            }
            else
            {
                inventoryUI.itemToolTip.gameObject.SetActive(false);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemToolTip.gameObject.SetActive(false);
        }
    }
}
