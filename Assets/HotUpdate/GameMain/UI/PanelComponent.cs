using UnityEngine;

namespace ACFrameworkCore
{
    public class PanelComponent : IUIState
    {
        public GameObject UIGO { get; set; }

        public void UIAwake()
        {
            DLog.Log("打开了界面");
            DLog.Log($"UI界面的名称是{UIGO.name}");
        }

        //public void UIUpdate()
        //{
        //    DLog.Log("每帧更新UIUpdate");
        //}
    }
}
