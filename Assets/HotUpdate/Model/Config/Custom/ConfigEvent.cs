namespace Farm2D
{
    public class ConfigEvent
    {
        //物品提示信息页面
        public const string ItemToolTipShow = "显示物品提示信息物品被选中的状态和信息";
        public const string ItemToolTipClose = "关闭物品提示信息物品被选中的状态和信息";

        //玩家相关
        public const string PlayerAnimationsEvent = "玩家动画";
        public const string PlayerMouseClicked = "鼠标点击事件";
        public const string HarvestAtPlayerPosition = "在玩家位置收获";

        //时间系统相关
        public const string GameDate = "显示日期和春夏秋冬";
        public const string GameMinute = "显示时间";

        //场景相关
        public const string SwichConfinerShape = "切换场景边界";
        public const string SceneTransition = "场景传送";
        public const string PlayerMoveToPosition = "人物加载场景时候的坐标";
        public const string BeforeSceneUnloadEvent = "卸载场景之前需要做的事件";
        public const string AfterSceneLoadedEvent = "加载场景之后需要做的事件";
        public const string GenerateCrop = "生成农作物";

        //UI界面相关
        public const string UIDisplayHighlighting = "UI界面显示选中物体高亮";
        public const string ShowDialogueEvent = "显示对话";

        //物品相关
        public const string UIItemDropItem = "扔东西";
        public const string UIItemOnDrag = "物品拖拽中";
        public const string UIItemOnEndDrag = "物品拖拽结束";
        public const string UIItemOnBeginDrag = "物品拖拽开始";
        public const string UIItemOnPointerClick = "物品点击";
        public const string InstantiateItemInScene = "生成物品在场景";

        //场景过度相关
        public const string UIFade = "场景过度";

        //鼠标相关
        public const string ItemSelectedEvent = "鼠标选择样式";
        public const string ExecuteActionAfterAnimation = "执行操作后的动画播放";

        //地图数据刷新相关
        public const string GameDay = "游戏一天需要做的(保存代码等)";
        public const string PlantSeed = "种植种子";
        public const string RefreshCurrentMap = "刷新档期那地图";

        //特效相关
        public const string ParticleEffect = "粒子效果";

        //音乐相关
        public const string PlaySoundEvent = "播放音乐";
        public const string InitSoundEffect = "初始音效";

        //建造相关
        public const string BuildFurniture = "建造家具";

        //游戏相关
        public const string UpdateGameStateEvent = "更新游戏状态事件";
        public const string EndGameEvent = "结束游戏";
        public const string StartNewGameEvent = "开始新的游戏";

        //对话相关
        public const string OnFinishEvent = "对话完成事件触发的监听函数";

        //UI商店
        public const string BaseBagOpen = "商店面板打开";
        public const string BaseBagClose = "商店面板关闭";
        public const string ShowTradeUI = "显示贸易UI界面";

        //其他
        public const string MoneyShow = "显示金币";

        //灯光相关
        public const string LightShiftChangeEvent = "光移变化";

        //物品库存相关
        public const string PalayerBag = "玩家背包";//玩家背包
        public const string ActionBar = "快捷键";//快捷键
        public const string Shop = "商店";//快捷键
        public const string Box = "箱子";//快捷键
        public const string Mira = "Mira";//快捷键
    }
}
