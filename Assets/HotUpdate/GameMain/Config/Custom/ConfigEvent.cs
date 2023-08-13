namespace ACFrameworkCore
{
    public class ConfigEvent
    {
        public const string UpdateInvenoryUI = "更新nvenoryUI";
        public const string BeforeSceneUnloadEvent = "场景卸载前事件";
        public const string DropItem = "扔东西";
        public const string ItemSelect = "物品被选中的状态和信息";

        public const string SwichConfinerShape = "切换场景边界";

        public const string UpdateSlotHightLight = "更新物品高亮";
        public const string ItemCreatOnWorld = "在世界地图生成物品";
        public const string ItemOnDrag = "物品拖拽中";
        public const string ItemOnEndDrag = "物品拖拽结束";
        public const string ItemOnBeginDrag = "物品拖拽开始";
        public const string ItemOnPointerClick = "物品点击";


        public const string ItemToolTipShow = "显示物品提示信息";
        public const string ItemToolTipClose = "关闭物品提示信息";

        public const string PlayerHoldUpAnimations = "玩家的举起动画";


        public const string GameDate = "显示日期和春夏秋冬";
        public const string GameMinute = "显示时间";


        public const string SceneTransition = "场景传送";
        public const string BeforeSceneUnload = "卸载场景之后需要做的事件";
        public const string PlayerMoveToPosition = "人物加载场景时候的坐标";
        public const string AfterSceneLoaded = "加载场景之后需要做的事件";
    }
}
