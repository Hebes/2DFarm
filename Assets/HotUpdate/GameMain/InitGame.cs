using ACFrameworkCore;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏流程
/// </summary>
public enum EInitGameProcess
{
    /// <summary> 初始化框架基础核心 </summary>
    FSMInitBaseCore,
    /// <summary> 初始化框架管理核心 </summary>
    FSMInitManagerCore,
    /// <summary> 初始化数据 </summary>
    FSMInitModel,
    /// <summary> 初始化UI </summary>
    FSMInitUI,
    /// <summary> 加载保存的数据 </summary>
    FSMInitSaveDataLoad,
    /// <summary> 进入游戏 </summary>
    FSMEnterGame,
}
public class InitGame
{

    public static void Init()
    {
        SwitchInitGameProcess(EInitGameProcess.FSMInitBaseCore).Forget();
    }
    private static async UniTaskVoid SwitchInitGameProcess(EInitGameProcess initGameProcess)
    {
        switch (initGameProcess)
        {
            case EInitGameProcess.FSMInitBaseCore: await FSMInitBaseCore(); break;
            case EInitGameProcess.FSMInitManagerCore: await FSMInitManagerCore(); break;
            case EInitGameProcess.FSMInitSaveDataLoad: FSMInitSaveDataLoad().Forget(); break;
            case EInitGameProcess.FSMInitModel: await FSMInitModel(); break;
            case EInitGameProcess.FSMInitUI: FSMInitUI(); break;
            case EInitGameProcess.FSMEnterGame: await FSMEnterGame(); break;
        }
    }

    private static async UniTask FSMInitBaseCore()
    {
        List<ICore> _initHs = new List<ICore>()
        {
            new DebugManager(), //日志管理
            new MonoManager(),  //mono管理
        };
        foreach (var init in _initHs)
        {
            init.ICroeInit();
            await UniTask.Yield();
        }
        SwitchInitGameProcess(EInitGameProcess.FSMInitManagerCore).Forget();
    }
    private static async UniTask FSMInitManagerCore()
    {
        List<ICore> _initHs = new List<ICore>()
        {
            new AduioManager(),     //音频管理
            new EventManager(),     //事件管理
            new DataManager(),      //数据管理
            new PoolManager(),      //对象池管理
            new ResourceManager(),  //加载管理
            new SceneManager(),     //场景管理
            new UIManager(),        //UI管理
        };
        foreach (var init in _initHs)
        {
            init.ICroeInit();
            await UniTask.Yield();
        }
        SwitchInitGameProcess(EInitGameProcess.FSMInitSaveDataLoad).Forget();
    }
    private static async UniTask FSMInitSaveDataLoad()
    {
        await UniTask.Yield();
        //List<ICore> _initHs = new List<ICore>()
        //{
        //};
        //foreach (var init in _initHs)
        //{
        //    init.ICroeInit();
        //    await UniTask.Yield();
        //}
        SwitchInitGameProcess(EInitGameProcess.FSMInitModel).Forget();
    }
    private static async UniTask FSMInitModel()
    {
        List<ICore> _initHs = new List<ICore>()
        {
            new InventoryAllSystem(),           //背包系统
            new InventoryWorldItemSystem(),     //物品在世界系统
            new TimeSystem(),                   //时间系统
            new CursorManagerSystem(),                  //鼠标系统
            new EffectsSystem(),                //特效系统
            new CommonManagerSystem(),          //常用物体管理系统
            new DialogueManagerSystem(),        //对话系统
            new SceneTransitionSystem(),        //场景过渡系统
            new AnimatorManagerSystem(),        //场景过渡系统
        };
        foreach (var init in _initHs)
        {
            init.ICroeInit();
            await UniTask.Yield();
        }
        SwitchInitGameProcess(EInitGameProcess.FSMInitUI).Forget();
    }
    private static void FSMInitUI()
    {
        ConfigUIPanel.UIActionBar.ShwoUIPanel<UIActionBarPanel>();       //显示快捷栏面板
        ConfigUIPanel.UIItemToolTip.ShwoUIPanel<UIItemToolTipPanel>();   //显示物体信息描述面板
        ConfigUIPanel.UIPlayerBag.ShwoUIPanel<PlayerBagPanel>();         //显示玩家背包面板
        ConfigUIPanel.UIDragPanel.ShwoUIPanel<UIDragPanel>();            //显示拖拽面板
        ConfigUIPanel.UIGameTime.ShwoUIPanel<UIGameTimePanel>();         //显示时间面板
        ConfigUIPanel.UIFade.ShwoUIPanel<UIFadePanel>();                 //显示时间面板
        ConfigUIPanel.UIDialogue.ShwoUIPanel<UIDialoguePanel>();         //显示对话面板
        ConfigUIPanel.UICursor.ShwoUIPanel<UICursorPanel>();             //显示鼠标面板

        ConfigEvent.UIFade.EventTriggerUniTask(0f).Forget();        //显隐界面
        ConfigUIPanel.UIItemToolTip.CloseUIPanel();                      //关闭物体信息描述面板
        ConfigUIPanel.UIPlayerBag.CloseUIPanel();                        //关闭玩家背包面板
        ConfigUIPanel.UIDialogue.CloseUIPanel();                         //关闭对话面板

        SwitchInitGameProcess(EInitGameProcess.FSMEnterGame).Forget();
    }
    private static async UniTask FSMEnterGame()
    {
        await UniTask.DelayFrame(40);
        ACDebug.Log("开始游戏");
        await UniTask.Yield();
        await SceneTransitionSystem.Instance.CreatScene();
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
