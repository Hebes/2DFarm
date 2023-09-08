using System.Collections.Generic;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    NPC对话完成后触发的函数

-----------------------*/

namespace ACFrameworkCore
{
    public class NPCFunction : MonoBehaviour
    {
        public List<InventoryItem> shopData;
        private bool isOpen;
        public string NPCName;

        private void Awake()
        {
            MonoManager.Instance.OnAddUpdateEvent(OnUpdate);
        }

        private void OnUpdate()
        {
            if (isOpen && Input.GetKeyDown(KeyCode.Escape))
                CloseShop();//关闭背包
        }

        /// <summary>
        /// 打开商店
        /// </summary>
        public void OpenShop()
        {
            isOpen = true;
            ConfigEvent.BaseBagOpen.EventTrigger(NPCName,ConfigEvent.Shop);
            ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Pause);
        }

        /// <summary>
        /// 关闭商店
        /// </summary>
        public void CloseShop()
        {
            isOpen = false;
            ConfigEvent.BaseBagClose.EventTrigger(ConfigEvent.Shop, NPCName);
            ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Gameplay);
        }
    }
}
