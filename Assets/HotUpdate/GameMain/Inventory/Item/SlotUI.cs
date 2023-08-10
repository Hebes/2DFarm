using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    槽UI

-----------------------*/

namespace ACFrameworkCore
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        private Image slotImage;            //图片
        private TextMeshProUGUI amountText; //数量
        private Button button;              //按钮

        public Image slotHightLight;        //高亮
        public ESlotType eSlotType;         //类型
        public bool isSelected;             //是否启用高亮
        public ItemDetails itemDatails;     //物品信息
        public int itemAmount;              //物品数量
        public int slotIndex;               //物品序列号


        private void Start()
        {
            isSelected = false;
            if (itemDatails == null)
                UpdateEmptySlot();
        }

        private InventoryUI inventoryUI
        {
            get
            {
                return GetComponentInParent<InventoryUI>();
            }
        }

        /// <summary>
        /// 更新Slot显示
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Amount"></param>
        public void UpdateSlot(ItemDetails item, int Amount)
        {
            itemDatails = item;
            slotImage.sprite = ResourceExtension.LoadAsyncAsT<Sprite>(item.itemIcon);
            itemAmount = Amount;
            amountText.text = Amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;//该组是否可交互（组下的元素是否处于启用状态）。
        }

        /// <summary>
        /// 清空Slot更新
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
                //清空所有高亮
                inventoryUI.UpdateSlotHightLight(-1);
                ConfigEvent.ItemSelect.EventTrigger(itemDatails, isSelected);
            }
            itemDatails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;//该组是否可交互（组下的元素是否处于启用状态）。
        }


        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;//拖拽物品的移动
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true;//启用拖拽的物体
                inventoryUI.dragItem.sprite = slotImage.sprite;//设置拖拽物体的图片
                slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, .5f);
                inventoryUI.dragItem.SetNativeSize();

                isSelected = true;
                inventoryUI.UpdateSlotHightLight(slotIndex);
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDatails == null) return;
            isSelected = !isSelected;
            slotHightLight.gameObject.SetActive(isSelected);
            inventoryUI.UpdateSlotHightLight(slotIndex);
            //判断是否是在商店中点击(商店不执行代码)
            if (eSlotType == ESlotType.Bag)
                ConfigEvent.ItemSelect.EventTrigger(itemDatails, isSelected);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                //物品交换
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null) return;
                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();//如果是存在SlotUI组件的话
                int targetIndex = targetSlot.slotIndex;

                //在player自身背包范围内的交换
                if (eSlotType == ESlotType.Bag && targetSlot.eSlotType == ESlotType.Bag)//类型都相同的话
                    InventoryAllManager.Instance.SwapItem(slotIndex, targetIndex);
                Debug.Log(eventData.pointerCurrentRaycast.gameObject);//打印鼠标指针的射线检测到的物体
                slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 1);
            }
            //else //测试仍在地上  //TUDO  31集
            //{
            //    if (itemDatails.canDropped)
            //    {
            //        //屏幕坐标转成世界坐标 鼠标对应的世界坐标
            //        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            //        EventHandler.CallInstantiateItemScen(itemDatails.itemID, pos);
            //        UpdateEmptySlot();
            //    }
            //}

            //清空所有高亮
            inventoryUI.UpdateSlotHightLight(-1);
        }
    }
}
