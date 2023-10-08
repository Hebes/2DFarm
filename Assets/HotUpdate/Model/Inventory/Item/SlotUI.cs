using Core;
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

namespace Farm2D
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        [HideInInspector] public Image slotImage;             //图片
        private TextMeshProUGUI amountText; //数量
        private Button button;              //按钮
        [HideInInspector] public Image slotHightLight;        //高亮

        public bool isSelected;             //是否启用高亮
        public ItemDetailsData itemDatails;     //物品信息
        public int itemAmount;              //物品数量
        public int slotIndex;               //格子序列号

        public string ItemKey;   //属于哪个物品管理类的,也就是InventoryAllManager的ItemDicList或者ItemDicArray的Key

        private void Awake()
        {
            slotImage = gameObject.GetChildComponent<Image>("Image");
            amountText = gameObject.GetChildComponent<TextMeshProUGUI>("Amount");
            button = gameObject.GetComponent<Button>();
            slotHightLight = gameObject.GetChildComponent<Image>("HighLight");
        }

        private void Start()
        {
            isSelected = false;
            if (itemDatails == null)
                UpdateEmptySlot();
        }

        /// <summary>
        /// 更新Slot显示
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Amount"></param>
        public void UpdateSlot(int itemID, int Amount)
        {
            ItemDetailsData item = itemID.GetDataOne<ItemDetailsData>();
            itemDatails = item;
            slotImage.sprite = LoadResExtension.LoadOrSub<Sprite>(item.itemIconPackage, item.itemIcon);
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
                //清空所有高亮
                isSelected = false;
                ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮
                ConfigEvent.ItemSelectedEvent.EventTrigger(ItemKey, itemDatails.itemID, isSelected);
                ConfigEvent.PlayerAnimationsEvent.EventTrigger(itemDatails.itemID, isSelected);
            }
            itemDatails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;//该组是否可交互（组下的元素是否处于启用状态）。
        }


        public void OnDrag(PointerEventData eventData)
        {
            ConfigEvent.UIItemOnDrag.EventTrigger(Input.mousePosition);
        }//拖拽中
        public void OnEndDrag(PointerEventData eventData)
        {
            ConfigEvent.UIItemOnEndDrag.EventTrigger(eventData, this);
        }//拖拽结束
        public void OnPointerClick(PointerEventData eventData)
        {
            ConfigEvent.UIItemOnPointerClick.EventTrigger(eventData, this);
        }//点击
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            ConfigEvent.UIItemOnBeginDrag.EventTrigger(eventData, this);
        }//拖拽结束
    }
}
