namespace ACFrameworkCore
{
    public class ConfigEvent
    {
        //物品提示信息页面
        public const string ItemToolTipShow = "显示物品提示信息物品被选中的状态和信息";
        public const string ItemToolTipClose = "关闭物品提示信息物品被选中的状态和信息";

        //玩家相关
        public const string PlayerHoldUpAnimations = "玩家的举起动画";
        public const string PlayerMouseClicked = "鼠标点击事件";

        //时间系统相关
        public const string GameDate = "显示日期和春夏秋冬";
        public const string GameMinute = "显示时间";

        //场景相关
        public const string SwichConfinerShape = "切换场景边界";
        public const string SceneTransition = "场景传送";
        public const string PlayerMoveToPosition = "人物加载场景时候的坐标";
        public const string SceneBeforeUnload = "卸载场景之前需要做的事件";
        public const string SceneAfterLoaded = "加载场景之后需要做的事件";

        //UI界面相关
        public const string UIDisplayHighlighting = "UI界面显示选中物体高亮";
        public const string UIItemCreatOnWorld = "在世界地图生成物品";

        public const string UIItemDropItem = "扔东西";
        public const string UIItemOnDrag = "物品拖拽中";
        public const string UIItemOnEndDrag = "物品拖拽结束";
        public const string UIItemOnBeginDrag = "物品拖拽开始";
        public const string UIItemOnPointerClick = "物品点击";

        //场景过度相关
        public const string UIFade = "场景过度";

        //鼠标相关
        public const string CursorItemSelect = "鼠标选择样式";
        public const string ExecuteActionAfterAnimation = "执行操作后的动画播放";
    }
}
