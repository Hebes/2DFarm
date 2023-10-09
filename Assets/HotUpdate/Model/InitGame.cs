using Core;
using Cysharp.Threading.Tasks;
using Farm2D;
using System.Collections.Generic;

/// <summary>
/// 游戏流程
/// </summary>
public enum EInitGameProcess
{
    /// <summary> 初始化框架基础核心 </summary>
    FSMInitBaseCore,
    /// <summary> 初始化数据 </summary>
    FSMInitModel,
    /// <summary> 初始化UI </summary>
    FSMInitUI,
    /// <summary> 进入游戏 </summary>
    FSMEnterGame,
}
public class InitGame
{
    public static void Init()
    {
        SwitchInitGameProcess(EInitGameProcess.FSMInitBaseCore);
    }

    private static void SwitchInitGameProcess(EInitGameProcess initGameProcess)
    {
        switch (initGameProcess)
        {
            case EInitGameProcess.FSMInitBaseCore: FSMInitManagerCore(); break;
            case EInitGameProcess.FSMInitModel: FSMInitModel().Forget(); break;
            case EInitGameProcess.FSMInitUI: FSMInitUI(); break;
            case EInitGameProcess.FSMEnterGame: FSMEnterGame().Forget(); break;
        }
    }

    private static void FSMInitManagerCore()
    {
        List<ICore> _initHs = new List<ICore>();
        _initHs.Add(new CoreDebug());            //日志管理
        _initHs.Add(new CoreEvent());            //事件管理
        _initHs.Add(new CoreMono());             //mono管理
        _initHs.Add(new CoreAduio());            //音频管理
        _initHs.Add(new CoreData());             //数据管理
        _initHs.Add(new CorePool());             //对象池管理
        _initHs.Add(new CoreResource());         //加载管理
        _initHs.Add(new CoreScene());           //场景管理
        _initHs.Add(new CoreUI());               //UI管理
        foreach (var init in _initHs)
            init.ICroeInit();
        SwitchInitGameProcess(EInitGameProcess.FSMInitModel);
    }

    private static async UniTask FSMInitModel()
    {
        List<IModelInit> _initHs = new List<IModelInit>();
        _initHs.Add(new ModelSaveLoad());   //数据保存系统
        _initHs.Add(new ModelAudio());      //音效系统
        _initHs.Add(new ModelCrop());       //庄稼系统
        _initHs.Add(new ModelItem());        //背包系统
        _initHs.Add(new ModelItemWorld());  //物品在世界系统
        _initHs.Add(new ModelShop());       //商店系统
        _initHs.Add(new ModelTime());       //时间系统
        _initHs.Add(new ModelMouse());      //鼠标系统
        _initHs.Add(new ModelEffects());     //特效系统
        _initHs.Add(new ModelCommon());     //物体管理系统
        _initHs.Add(new ModelDialogue());   //对话系统
        _initHs.Add(new ModelTimeline());   //动画系统
        _initHs.Add(new ModelSwitchScene());   //场景过渡系统
        _initHs.Add(new ModelBuild());      //建造系统
        _initHs.Add(new ModelLight());       //灯光系统
        foreach (var init in _initHs)
            await init.ModelInit();
        SwitchInitGameProcess(EInitGameProcess.FSMInitUI);
    }
    private static void FSMInitUI()
    {
        ConfigUIPanel.UIActionBar.ShwoUIPanel<UIActionBarPanel>();       //显示快捷栏面板
        ConfigUIPanel.UIItemToolTip.ShwoUIPanel<UIItemToolTipPanel>();   //显示物体信息描述面板
        ConfigUIPanel.UIPlayerBag.ShwoUIPanel<UIPlayerBagPanel>();       //显示玩家背包面板
        ConfigUIPanel.UIDragPanel.ShwoUIPanel<UIDragPanel>();            //显示拖拽面板
        ConfigUIPanel.UIGameTime.ShwoUIPanel<UIGameTimePanel>();         //显示时间面板
        ConfigUIPanel.UIFade.ShwoUIPanel<UIFadePanel>();                 //显示时间面板
        ConfigUIPanel.UIDialogue.ShwoUIPanel<UIDialoguePanel>();         //显示对话面板
        ConfigUIPanel.UIBagBase.ShwoUIPanel<UIBagBasePanel>();           //显示商店箱子面板
        ConfigUIPanel.UITrade.ShwoUIPanel<UITradePanel>();               //显示购买数量面板
        ConfigUIPanel.UICursor.ShwoUIPanel<UICursorPanel>();             //显示鼠标面板


        ConfigEvent.UIFade.EventTriggerAsync(0f).Forget();          //过度界面
        ConfigUIPanel.UIItemToolTip.CloseUIPanel();                      //关闭物体信息描述面板
        ConfigUIPanel.UIPlayerBag.CloseUIPanel();                        //关闭玩家背包面板
        ConfigUIPanel.UIDialogue.CloseUIPanel();                         //关闭对话面板
        ConfigUIPanel.UIBagBase.CloseUIPanel();                          //关闭商店箱子面板
        ConfigUIPanel.UITrade.CloseUIPanel();                            //关闭购买数量面板

        SwitchInitGameProcess(EInitGameProcess.FSMEnterGame);
    }
    private static async UniTask FSMEnterGame()
    {
        await UniTask.DelayFrame(40);
        await UniTask.Yield();
        //await SceneTransitionManagerSystem.Instance.CreatScene();

        //显示图片
        //Sprite sprite = await ConfigSprites.Turnip_growPng.LoadSubAssetsAsyncUniTask($"{ConfigSprites.Turnip_growPng}_0");
        //GameObject test = new GameObject("test");
        //SpriteRenderer spriteRenderer = test.AddComponent<SpriteRenderer>();
        //spriteRenderer.sprite = sprite;

        //测试创建拾取的物体
        //GameObject gameObject = await ResourceExtension.LoadAsyncUniTask<GameObject>(Config.ItemBasePreafab);

        //GameObject go1 = GameObject.Instantiate(gameObject);
        //Item item = go1.GetComponent<Item>();
        //item.Init(1007, 3).Forget();

        //GameObject go2 = GameObject.Instantiate(gameObject);
        //Item item2 = go2.GetComponent<Item>();
        //item2.Init(1008, 6).Forget();

        //GameObject go3 = GameObject.Instantiate(gameObject);
        //Item item3 = go3.GetComponent<Item>();
        //item3.Init(1015, 119).Forget();

        //GameObject go4 = GameObject.Instantiate(gameObject);
        //Item item4 = go4.GetComponent<Item>();
        //item4.Init(1001, 1).Forget();

        //GameObject go5 = GameObject.Instantiate(gameObject);
        //Item item5 = go5.GetComponent<Item>();
        //item5.Init(1004, 1).Forget();
    }
}
