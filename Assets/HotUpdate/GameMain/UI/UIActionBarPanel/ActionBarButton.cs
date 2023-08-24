using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	快捷栏的快捷键

-----------------------*/

namespace ACFrameworkCore
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
        }
        private void OnEnable()
        {
            ConfigEvent.UpdateGameStateEvent.AddEventListener<EGameState>(OnUpdateGameStateEvent);
        }
        private void OnDisable()
        {
            ConfigEvent.UpdateGameStateEvent.RemoveEventListener<EGameState>(OnUpdateGameStateEvent);
        }

        private void OnUpdateGameStateEvent(EGameState gameState)
        {
            canUse = gameState == EGameState.Gameplay;
        }

        private void Update()
        {
            if (Input.GetKeyDown(key) && canUse)
            {
                if (slotUI.itemDatails != null)
                {
                    slotUI.isSelected = !slotUI.isSelected;
                    if (slotUI.isSelected)
                        ConfigEvent.UIDisplayHighlighting.EventTrigger(ConfigInventory.ActionBar, slotUI.slotIndex);//显示高亮
                    else
                        ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮
                    ConfigEvent.ItemSelectedEvent.EventTrigger(slotUI.itemDatails, slotUI.isSelected);
                }
            }
        }
    }
}
