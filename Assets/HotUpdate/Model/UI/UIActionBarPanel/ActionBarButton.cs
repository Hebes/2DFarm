using Core;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	快捷栏的快捷键

-----------------------*/

namespace Farm2D
{
    [RequireComponent(typeof(SlotUI))]
    public class ActionBarButton : MonoBehaviour
    {
        public KeyCode key;
        private SlotUI slotUI;
        private bool canUse = true;

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
            ConfigEvent.UpdateGameStateEvent.AddEventListener<EGameState>(OnUpdateGameStateEvent);
        }

        private void OnUpdateGameStateEvent(EGameState gameState)
        {
            canUse = gameState == EGameState.Gameplay;
        }

        /// <summary>
        /// 快捷键
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(key) && canUse)
            {
                if (slotUI.itemDatails == null) return;
                slotUI.isSelected = !slotUI.isSelected;
                if (slotUI.isSelected)
                    ConfigEvent.UIDisplayHighlighting.EventTrigger(ConfigEvent.ActionBar, slotUI.slotIndex);//显示高亮
                else
                    ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮
                ConfigEvent.ItemSelectedEvent.EventTrigger(slotUI.ItemKey, slotUI.itemDatails.itemID, slotUI.isSelected);//更换鼠标图片
                ConfigEvent.PlayerAnimationsEvent.EventTrigger(slotUI.itemDatails.itemID, slotUI.isSelected);//切换玩家动画
            }
        }
    }
}
