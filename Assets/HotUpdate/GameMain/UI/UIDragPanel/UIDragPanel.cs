using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    物体拖拽界面

-----------------------*/

namespace ACFrameworkCore
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



        //事件监听
        private void ItemDrag(Vector3 obj)
        {
            DragItemImage.transform.position = obj;
        }
        private void ItemOnBeginDrag(PointerEventData eventData, SlotUI slotUI)
        {
            if (slotUI.itemAmount != 0)
            {
                DragItemImage.enabled = true;//启用拖拽的物体
                DragItemImage.sprite = slotUI.slotImage.sprite;//设置拖拽物体的图片
                DragItemImage.color = new Color(DragItemImage.color.r, DragItemImage.color.g, DragItemImage.color.b, .5f);
                DragItemImage.SetNativeSize();
                slotUI.isSelected = true;
                ConfigEvent.UIDisplayHighlighting.EventTrigger(slotUI.configInventoryKey, slotUI.slotIndex);
            }
        }
        private void ItemOnEndDrag(PointerEventData eventData, SlotUI slotUI)
        {
            key = slotUI.configInventoryKey;
            DragItemImage.enabled = false;
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                //物品交换
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null) return;
                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();//如果是存在SlotUI组件的话
                if (key == ConfigInventory.PalayerBag && targetSlot.configInventoryKey == ConfigInventory.PalayerBag)//两个都是背包的话就是交换
                {
                    ACDebug.Log($"背包数据交换");
                    //物品交换
                    InventoryAllSystem.Instance.ChangeItem(slotUI.configInventoryKey, targetSlot.configInventoryKey, slotUI.slotIndex, targetSlot.slotIndex);
                    slotUI.slotImage.color = new Color(slotUI.slotImage.color.r, slotUI.slotImage.color.g, slotUI.slotImage.color.b, 1);
                }
                else if (key == ConfigInventory.Mira && targetSlot.configInventoryKey == ConfigInventory.PalayerBag)//买
                {
                    ACDebug.Log($"买东西");
                    ConfigEvent.ShowTradeUI.EventTrigger(key, targetSlot.configInventoryKey, slotUI.itemDatails, false);
                }
                else if (key == ConfigInventory.PalayerBag && targetSlot.configInventoryKey == ConfigInventory.Mira)//卖
                {
                    ACDebug.Log($"卖东西");
                    ConfigEvent.ShowTradeUI.EventTrigger(key, targetSlot.configInventoryKey, slotUI.itemDatails, true);
                }
                else if (key == ConfigInventory.Shop && targetSlot.configInventoryKey == ConfigInventory.PalayerBag)//买
                {
                    ACDebug.Log($"买东西");
                    ConfigEvent.ShowTradeUI.EventTrigger(key,slotUI.itemDatails, false);
                }
                else if (key == ConfigInventory.PalayerBag && targetSlot.configInventoryKey == ConfigInventory.Shop)//卖
                {
                    ACDebug.Log($"卖东西");
                    ConfigEvent.ShowTradeUI.EventTrigger(key,slotUI.itemDatails, true);
                }
                else if (key != ConfigInventory.Shop && targetSlot.configInventoryKey != ConfigInventory.Shop && key != targetSlot.configInventoryKey)
                {
                    ACDebug.Log($"跨背包数据交换物品");
                    //跨背包数据交换物品
                    //InventoryAllSystem.Instance.SwapItem(Location, slotIndex, targetSlot.Location, targetSlot.slotIndex);
                    InventoryAllSystem.Instance.ChangeItem(slotUI.configInventoryKey, targetSlot.configInventoryKey, slotUI.slotIndex, targetSlot.slotIndex);
                }
            }
            //清空所有高亮
            ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮
        }
        private void ItemOnPointerClick(PointerEventData eventData, SlotUI slotUI)
        {
            if (slotUI.itemDatails == null) return;
            slotUI.isSelected = !slotUI.isSelected;
            ConfigEvent.UIDisplayHighlighting.EventTrigger(slotUI.configInventoryKey, slotUI.slotIndex);

            itemAmount = slotUI.itemAmount;
            key = slotUI.configInventoryKey;

            switch (slotUI.configInventoryKey)
            {
                case ConfigInventory.PalayerBag:
                case ConfigInventory.ActionBar:
                    ConfigEvent.PlayerHoldUpAnimations.EventTrigger(slotUI.itemDatails, slotUI.isSelected);//通知物品被选中的状态
                    ConfigEvent.ItemSelectedEvent.EventTrigger(slotUI.itemDatails, slotUI.isSelected);//切换鼠标样式
                    break;
            }
        }
    }
}
