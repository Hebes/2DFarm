namespace MyUI
{
    public enum EnMoveDir
    {
        Up,
        Left,
        Down,
        Right,
    }
    public enum EnValueType
    {
        Default,
        Custom,
    }
    public enum EnButtonAnimeType
    {
        None,
        Scale,
        Move,
        Rotate,
        Outline,
    }
    public enum EnPanelAnimeType
    {
        /// <summary>
        /// 移动
        /// </summary>
        Move,
        /// <summary>
        /// 缩放
        /// </summary>
        Scale,
        /// <summary>
        /// 旋转
        /// </summary>
        Rotate,
        /// <summary>
        /// 尺寸
        /// </summary>
        Size,
        /// <summary>
        /// 渐变
        /// </summary>
        Fade,
    }
    /// <summary>
    /// 渐变组
    /// </summary>
    public enum EnFadeType
    {
        Single,
        Group,
    }
}