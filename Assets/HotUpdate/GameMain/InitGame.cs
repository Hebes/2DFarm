using ACFrameworkCore;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

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
            case EInitGameProcess.FSMEnterGame: FSMEnterGame(); break;
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
        ConfigUIPanel.UIActionBarPanel.ShwoUIPanel<ActionBarPanel>();                           //显示快捷栏面板
        ConfigUIPanel.UIItemToolTipPanel.ShwoUIPanel<UIItemToolTipPanel>();                     //显示物体信息描述面板
        ConfigUIPanel.UIPlayerBagPanel.ShwoUIPanel<PlayerBagPanel>();                           //显示玩家背包面板
        ConfigUIPanel.UIDragPanelPanel.ShwoUIPanel<UIDragPanel>();                              //显示拖拽面板
        ConfigUIPanel.UIGameTimePanel.ShwoUIPanel<UIGameTimePanel>();                           //显示时间面板

        ConfigUIPanel.UIItemToolTipPanel.CloseUIPanel();                                        //关闭物体信息描述面板
        ConfigUIPanel.UIPlayerBagPanel.CloseUIPanel();                                          //关闭玩家背包面板

        SwitchInitGameProcess(EInitGameProcess.FSMEnterGame).Forget();
    }
    private static void FSMEnterGame()
    {
        ACDebug.Log("开始游戏");
    }
}
