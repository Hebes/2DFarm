using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Mobile, EUIMode.Normal, EUILucenyType.Pentrate);
            ConfigEvent.ItemOnDrag.AddEventListener<Vector3>(ItemDrag);
            ConfigEvent.ItemOnBeginDrag.AddEventListener<PointerEventData, SlotUI>(ItemOnBeginDrag);
            ConfigEvent.ItemOnEndDrag.AddEventListener<PointerEventData, SlotUI>(ItemOnEndDrag);
            ConfigEvent.ItemOnPointerClick.AddEventListener<PointerEventData, SlotUI>(ItemOnPointerClick);
            GameObject DragItem = panelGameObject.GetChild("DragItemImage");
            DragItemImage = DragItem.GetComponent<Image>();
        }

        private void ItemOnPointerClick(PointerEventData eventData, SlotUI slotUI)
        {
            if (slotUI.itemDatails == null) return;
            slotUI.isSelected = !slotUI.isSelected;
            slotUI.slotHightLight.gameObject.SetActive(slotUI.isSelected);
            ConfigEvent.UpdateSlotHightLight.EventTrigger(slotUI.key, slotUI.slotIndex);
            //InventoryAllManager.Instance.UpdateSlotHightLight(slotUI.key, slotUI.slotIndex);
            //判断是否是在商店中点击(商店不执行代码)
            //if (eSlotType == ESlotType.Bag)
            //    ConfigEvent.ItemSelect.EventTrigger(slotUI.itemDatails, slotUI.isSelected);
        }

        private void ItemOnEndDrag(PointerEventData eventData, SlotUI slotUI)
        {
            DragItemImage.enabled = false;
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                //物品交换
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null) return;
                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();//如果是存在SlotUI组件的话
                int targetIndex = targetSlot.slotIndex;

                //物品交换
                ACDebug.Log($"当前的{slotUI.key}{slotUI.slotIndex},目标的是{targetSlot.key}{targetSlot.slotIndex}");
                InventoryAllManager.Instance.ChangeItem(slotUI.key, targetSlot.key, slotUI.slotIndex, targetSlot.slotIndex);
                ACDebug.Log($"鼠标指针的射线检测到的物体: {eventData.pointerCurrentRaycast.gameObject}");
                slotUI.slotImage.color = new Color(slotUI.slotImage.color.r, slotUI.slotImage.color.g, slotUI.slotImage.color.b, 1);
            }
            else //测试仍在地上
            {
                if (slotUI.itemDatails.canDropped == false)
                {
                    ACDebug.Log($"{slotUI.itemDatails.name}是不能被扔掉的");
                    return;
                }
                //屏幕坐标转成世界坐标 鼠标对应的世界坐标
                var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                ConfigEvent.ItemCreatOnWorld.EventTrigger(slotUI, pos);//id 数量,坐标//创建世界物体
                InventoryAllManager.Instance.RemoveItemDicArray(slotUI.key, slotUI.itemDatails.itemID, slotUI.itemAmount);//删除原先的
                slotUI.UpdateEmptySlot();
            }
            //清空所有高亮
            ConfigEvent.UpdateSlotHightLight.EventTrigger(string.Empty, -1);//清空所有高亮
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
                ConfigEvent.UpdateSlotHightLight.EventTrigger(slotUI.key, slotUI.slotIndex);
            }
        }

        //物品拖拽
        private void ItemDrag(Vector3 obj)
        {
            DragItemImage.transform.position = obj;
        }
    }
}
