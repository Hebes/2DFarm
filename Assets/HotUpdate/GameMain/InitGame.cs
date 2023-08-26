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
    FSMInitData,
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
            case EInitGameProcess.FSMInitData: await FSMInitData(); break;
            case EInitGameProcess.FSMInitUI: FSMInitUI(); break;
            case EInitGameProcess.FSMEnterGame: await FSMEnterGame(); break;
        }
    }

    private static async UniTask FSMInitBaseCore()
    {
        List<ICore> _initHs = new List<ICore>()
        {
            new DebugManager(),
            new MonoManager(),
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
            new AduioManager(),
            new EventManager(),
            new DataManager(),
            new PoolManager(),
            new ResourceManager(),
            new ManagerScene(),
            new UIManager(),
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
        List<ICore> _initHs = new List<ICore>()
        {
            new RecordedDataLoadSystem(),
        };
        foreach (var init in _initHs)
        {
            init.ICroeInit();
            await UniTask.Yield();
        }
        SwitchInitGameProcess(EInitGameProcess.FSMInitData).Forget();
    }
    private static async UniTask FSMInitData()
    {
        List<ICore> _initHs = new List<ICore>()
        {
            new InventoryAllSystem(),
            new InventoryWorldItemSystem(),
            new TimeSystem(),
            new SceneTransitionSystem(),
            new MouseSystem(),
            new EffectsSystem(),
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
        ConfigUIPanel.UIActionBarPrefab.ShwoUIPanel<UIActionBarPanel>();                           //显示快捷栏面板
        ConfigUIPanel.UIItemToolTipPrefab.ShwoUIPanel<UIItemToolTipPanel>();                     //显示物体信息描述面板
        ConfigUIPanel.UIPlayerBagPrefab.ShwoUIPanel<PlayerBagPanel>();                           //显示玩家背包面板
        ConfigUIPanel.UIDragPanelPrefab.ShwoUIPanel<UIDragPanel>();                              //显示拖拽面板
        ConfigUIPanel.UIGameTimePrefab.ShwoUIPanel<UIGameTimePanel>();                           //显示时间面板
        ConfigUIPanel.UIFadePrefab.ShwoUIPanel<UIFadePanel>();                                   //显示时间面板

        ConfigEvent.UIFade.EventTriggerUniTask(0f).Forget();                                    //显隐界面
        ConfigUIPanel.UIItemToolTipPrefab.CloseUIPanel();                                        //关闭物体信息描述面板
        ConfigUIPanel.UIPlayerBagPrefab.CloseUIPanel();                                          //关闭玩家背包面板

        SwitchInitGameProcess(EInitGameProcess.FSMEnterGame).Forget();
    }
    private static async UniTask FSMEnterGame()
    {
        await UniTask.DelayFrame(40);
        ACDebug.Log("开始游戏");
        await UniTask.Yield();
        //测试创建拾取的物体
        GameObject gameObject = await ResourceExtension.LoadAsyncUniTask<GameObject>(ConfigPrefab.ItemBasePrefab);

        GameObject go1 = GameObject.Instantiate(gameObject);
        Item item = go1.GetComponent<Item>();
        item.Init(1007,3).Forget();

        GameObject go2 = GameObject.Instantiate(gameObject);
        Item item2 = go2.GetComponent<Item>();
        item2.Init(1008, 6).Forget();

        GameObject go3 = GameObject.Instantiate(gameObject);
        Item item3 = go3.GetComponent<Item>();
        item3.Init(1015, 119).Forget();

        GameObject go4 = GameObject.Instantiate(gameObject);
        Item item4 = go4.GetComponent<Item>();
        item4.Init(1001, 1).Forget();

        GameObject go5 = GameObject.Instantiate(gameObject);
        Item item5 = go5.GetComponent<Item>();
        item5.Init(1004, 1).Forget();
    }
}
