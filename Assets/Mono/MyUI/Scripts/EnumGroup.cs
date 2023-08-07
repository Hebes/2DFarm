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
        /// �ƶ�
        /// </summary>
        Move,
        /// <summary>
        /// ����
        /// </summary>
        Scale,
        /// <summary>
        /// ��ת
        /// </summary>
        Rotate,
        /// <summary>
        /// �ߴ�
        /// </summary>
        Size,
        /// <summary>
        /// ����
        /// </summary>
        Fade,
    }
    /// <summary>
    /// ������
    /// </summary>
    public enum EnFadeType
    {
        Single,
        Group,
    }
}