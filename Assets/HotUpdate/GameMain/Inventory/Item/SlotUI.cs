using Cysharp.Threading.Tasks;
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

        public Image slotImage;            //图片
        private TextMeshProUGUI amountText; //数量
        private Button button;              //按钮
        public Image slotHightLight;        //高亮

        public ESlotType eSlotType;         //类型
        public bool isSelected;             //是否启用高亮
        public ItemDetails itemDatails;     //物品信息
        public int itemAmount;              //物品数量
        public int slotIndex;               //格子序列号
        public string key;                  //属于哪个物品管理类的,也就是InventoryAllManager的ItemDicList或者ItemDicArray

        private InventoryAllManager IAM;

        private void Awake()
        {
            slotImage = gameObject.GetChildComponent<Image>("Image");
            amountText = gameObject.GetChildComponent<TextMeshProUGUI>("Amount");
            button = gameObject.GetComponent<Button>();
            slotHightLight = gameObject.GetChildComponent<Image>("HighLight");

            IAM = InventoryAllManager.Instance;
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
        public async UniTask UpdateSlot(ItemDetails item, int Amount)
        {
            itemDatails = item;
            slotImage.sprite = await ResourceExtension.LoadAsyncUniTask<Sprite>(item.itemIcon);
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
                isSelected = false;
                slotHightLight.gameObject.SetActive(false);
                //TODO 这里需要编写代码,参考下面
                //ConfigEvent.ItemSelect.EventTrigger(itemDatails, isSelected);
            }
            itemDatails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;//该组是否可交互（组下的元素是否处于启用状态）。
        }


        public void OnDrag(PointerEventData eventData)
        {
            ConfigEvent.ItemOnDrag.EventTrigger(Input.mousePosition);
        }//拖拽中
        public void OnEndDrag(PointerEventData eventData)
        {
            ConfigEvent.ItemOnEndDrag.EventTrigger(eventData,this);
        }//拖拽结束
        public void OnPointerClick(PointerEventData eventData)
        {
            ConfigEvent.ItemOnPointerClick.EventTrigger(eventData,this);
        }//点击
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            ConfigEvent.ItemOnBeginDrag.EventTrigger(eventData,this);
        }//拖拽结束
    }
}
