using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    UI面板的管理

-----------------------*/


namespace ACFrameworkCore
{

    public class InventoryUI : MonoBehaviour
    {
        public ItemToolTip itemToolTip;//信息提示框
        public Image dragItem;//拖拽的物体
        private GameObject bagUI;//玩家背包
        private bool bagOpened;//背包是否被打开了
        private Button bagButton;//背包按钮
        private SlotUI[] playerSlot;//玩家格子

        private void OnEnable()
        {
            ConfigEvent.UpdateInvenoryUI.AddEventListener<EInventoryLocation, List<InventoryItem>>(OnUpdateInventoryUI);
            ConfigEvent.BeforeSceneUnloadEvent.AddEventListener(OnBeforeSceneUnloadEvent);
        }
        private void OnDisable()
        {
            ConfigEvent.UpdateInvenoryUI.RemoveEventListener<EInventoryLocation, List<InventoryItem>>(OnUpdateInventoryUI);
            ConfigEvent.BeforeSceneUnloadEvent.RemoveEventListener(OnBeforeSceneUnloadEvent);
        }
        private void Awake()
        {
            itemToolTip.gameObject.SetActive(false);//默认是关闭的
            bagUI.SetActive(false);//确保游戏开始的时候肯定是关闭状态
            bagButton.onClick.AddListener(OpenBagUI);
        }
        private void Start()
        {
            for (int i = 0; i < playerSlot.Length; i++)
                playerSlot[i].slotIndex = i;//给每个各自序列号
            bagOpened = bagUI.activeInHierarchy;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
                OpenBagUI();
        }

        /// <summary>
        /// 取消物品选择高亮
        /// </summary>
        private void OnBeforeSceneUnloadEvent()
        {
            UpdateSlotHightLight(-1);
        }
        /// <summary>
        /// 打开背包
        /// </summary>
        public void OpenBagUI()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }
        /// <summary>
        /// 更新Slot高亮显示
        /// </summary>
        /// <param name="index"></param>
        public void UpdateSlotHightLight(int index)
        {
            foreach (var slot in playerSlot)
            {
                if (slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHightLight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHightLight.gameObject.SetActive(false);
                }
            }
        }
        /// <summary>
        /// 监听的事件的具体执行方法
        /// </summary>
        /// <param name="location"></param>
        /// <param name="list"></param>
        private void OnUpdateInventoryUI(EInventoryLocation location, List<InventoryItem> list)
        {
            switch (location)
            {
                case EInventoryLocation.Player:
                    for (int i = 0; i < playerSlot.Length; i++)
                    {
                        if (list[i].itemAmount > 0)//有物品
                        {
                            ItemDetails item = InventoryAllManager.Instance.GetItem(list[i].itemID);
                            playerSlot[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            playerSlot[i].UpdateEmptySlot();
                        }
                    }
                    break;
                case EInventoryLocation.Box:
                    break;
                default:
                    break;
            }
        }
    }
}
