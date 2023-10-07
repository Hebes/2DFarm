namespace Core
{
    /// <summary> UI窗体（位置）类型 </summary>
    public enum EUIType
    {
        /// <summary> 普通窗体 </summary>
        Normal,
        /// <summary> 固定窗体 </summary>
        Fixed,
        /// <summary> 弹出窗体 </summary>
        PopUp,
        /// <summary> 独立的窗口可移动的 </summary> TODO 等待敲代码
        Mobile,
        /// <summary> 渐变过度窗体 </summary>
        Fade,
    }

    /// <summary> UI窗体的显示类型 </summary>
    public enum EUIMode
    {
        /// <summary> 普通 模式允许多个窗体同时显示 </summary>
        Normal,
        /// <summary> 反向切换 一般要求玩家必须先关闭弹出的顶层窗体，再依次关闭下一级窗体</summary>
        ReverseChange,
        /// <summary> 隐藏其他 一般应用于全局性的窗体 </summary>
        HideOther,
    }

    /// <summary> UI窗体透明度类型 </summary>
    public enum EUILucenyType
    {
        /// <summary> 完全透明，不能穿透 </summary>
        Lucency,
        /// <summary> 半透明，不能穿透 </summary>
        Translucence,
        /// <summary> 低透明度，不能穿透 </summary>
        ImPenetrable,
        /// <summary> 可以穿透 </summary>
        Pentrate
    }

    public class UIConfig
    {
        /* 遮罩管理器中，透明度常量 */
        public const float SYS_UIMASK_LUCENCY_COLOR_RGB = 255 / 255F;
        public const float SYS_UIMASK_LUCENCY_COLOR_RGB_A = 0F / 255F;

        public const float SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB = 220 / 255F;
        public const float SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB_A = 50F / 255F;

        public const float SYS_UIMASK_IMPENETRABLE_COLOR_RGB = 50 / 255F;
        public const float SYS_UIMASK_IMPENETRABLE_COLOR_RGB_A = 200F / 255F;
    }
}
