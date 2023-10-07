using Core;
using Farm2D;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = Core.Debug;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    物体拖拽界面

-----------------------*/

namespace Farm2D
{
    public class UIDragPanel : UIBase
    {
        public Image DragItemImage;
        public string key;                     //属于哪个物品管理类的,也就是InventoryAllManager的ItemDicList或者ItemDicArray的Key
        public int itemAmount;                  //物品数量

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Mobile, EUIMode.Normal, EUILucenyType.Pentrate);

            GameObject DragItem = panelGameObject.GetChild("DragItemImage");
            DragItemImage = DragItem.GetComponent<Image>();

            ConfigEvent.UIItemOnDrag.AddEventListener<Vector3>(ItemDrag);
            ConfigEvent.UIItemOnBeginDrag.AddEventListener<PointerEventData, SlotUI>(ItemOnBeginDrag);
            ConfigEvent.UIItemOnEndDrag.AddEventListener<PointerEventData, SlotUI>(ItemOnEndDrag);
            ConfigEvent.UIItemOnPointerClick.AddEventListener<PointerEventData, SlotUI>(ItemOnPointerClick);
        }


        /// <summary>
        /// 物品拖拽
        /// </summary>
        /// <param name="obj"></param>
        private void ItemDrag(Vector3 obj)
        {
            DragItemImage.transform.position = obj;
        }

        /// <summary>
        /// 物品拖拽开始
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="slotUI"></param>
        private void ItemOnBeginDrag(PointerEventData eventData, SlotUI slotUI)
        {
            if (slotUI.itemAmount != 0)
            {
                DragItemImage.enabled = true;//启用拖拽的物体
                DragItemImage.sprite = slotUI.slotImage.sprite;//设置拖拽物体的图片
                DragItemImage.color = new Color(DragItemImage.color.r, DragItemImage.color.g, DragItemImage.color.b, .5f);
                DragItemImage.SetNativeSize();
                slotUI.isSelected = true;
                ConfigEvent.UIDisplayHighlighting.EventTrigger(slotUI.ItemKey, slotUI.slotIndex);
            }
        }

        /// <summary>
        /// 拖拽结束
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="slotUI"></param>
        private void ItemOnEndDrag(PointerEventData eventData, SlotUI slotUI)
        {
            key = slotUI.ItemKey;
            DragItemImage.enabled = false;
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                //物品交换
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null) return;
                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();//如果是存在SlotUI组件的话
                if (key == ConfigEvent.PalayerBag && targetSlot.ItemKey == ConfigEvent.PalayerBag)//两个都是背包的话就是交换
                {
                    Debug.Log($"背包数据交换");
                    //物品交换
                    ModelItem.Instance.ChangeItem(slotUI.ItemKey, targetSlot.ItemKey, slotUI.slotIndex, targetSlot.slotIndex);
                    slotUI.slotImage.color = new Color(slotUI.slotImage.color.r, slotUI.slotImage.color.g, slotUI.slotImage.color.b, 1);
                }
                else if (key == ConfigEvent.Mira && targetSlot.ItemKey == ConfigEvent.PalayerBag)//买
                {
                    Debug.Log($"买东西");
                    ConfigEvent.ShowTradeUI.EventTrigger(key, targetSlot.ItemKey, slotUI.itemDatails.itemID, false);
                }
                else if (key == ConfigEvent.PalayerBag && targetSlot.ItemKey == ConfigEvent.Mira)//卖
                {
                    Debug.Log($"卖东西");
                    ConfigEvent.ShowTradeUI.EventTrigger(key, targetSlot.ItemKey, slotUI.itemDatails.itemID, true);
                }
                else if (key == ConfigEvent.Mira && targetSlot.ItemKey == ConfigEvent.ActionBar)//买
                {
                    Debug.Log($"买东西");
                    ConfigEvent.ShowTradeUI.EventTrigger(key, targetSlot.ItemKey, slotUI.itemDatails.itemID, false);
                }
                else if (key == ConfigEvent.ActionBar && targetSlot.ItemKey == ConfigEvent.Mira)//卖
                {
                    Debug.Log($"卖东西");
                    ConfigEvent.ShowTradeUI.EventTrigger(key, targetSlot.ItemKey, slotUI.itemDatails.itemID, true);
                }
                else if (key != ConfigEvent.Shop && targetSlot.ItemKey != ConfigEvent.Shop && key != targetSlot.ItemKey)
                {
                    Debug.Log($"跨背包数据交换物品");
                    ModelItem.Instance.ChangeItem(slotUI.ItemKey, targetSlot.ItemKey, slotUI.slotIndex, targetSlot.slotIndex);
                }
            }
            //清空所有高亮
            ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮
        }

        /// <summary>
        /// 点击物品
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="slotUI"></param>
        private void ItemOnPointerClick(PointerEventData eventData, SlotUI slotUI)
        {
            if (slotUI.itemDatails == null) return;
            slotUI.isSelected = !slotUI.isSelected;
            ConfigEvent.UIDisplayHighlighting.EventTrigger(slotUI.ItemKey, slotUI.slotIndex);

            itemAmount = slotUI.itemAmount;
            key = slotUI.ItemKey;

            switch (slotUI.ItemKey)
            {
                case ConfigEvent.PalayerBag:
                case ConfigEvent.ActionBar:
                    ConfigEvent.PlayerAnimationsEvent.EventTrigger(slotUI.itemDatails.itemID, slotUI.isSelected);//通知物品被选中的状态
                    ConfigEvent.ItemSelectedEvent.EventTrigger(slotUI.ItemKey, slotUI.itemDatails.itemID, slotUI.isSelected);//切换鼠标样式
                    break;
            }
        }
    }
}
