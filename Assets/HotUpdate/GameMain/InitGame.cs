using ACFrameworkCore;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏流程
/// </summary>
public enum EInitGameProcess
{
    FSMInitFramework,//初始化框架
    FSMInitData,//初始化数据
    FSMEnterGame,//进入游戏
}
public class InitGame
{
    public static void Init()
    {
        Debug.Log("初始化场景");
        SwitchInitGameProcess(EInitGameProcess.FSMInitFramework).Forget();
    }

    /// <summary>
    /// 切换初始化场景
    /// </summary>
    private static async UniTaskVoid SwitchInitGameProcess(EInitGameProcess initGameProcess)
    {
        switch (initGameProcess)
        {
            case EInitGameProcess.FSMInitFramework:
                await InitRsv();
                SwitchInitGameProcess(EInitGameProcess.FSMInitData).Forget();
                break;
            case EInitGameProcess.FSMInitData:
                await InitData();
                SwitchInitGameProcess(EInitGameProcess.FSMEnterGame).Forget();
                break;
            case EInitGameProcess.FSMEnterGame:
                Debug.Log("进入游戏");
                EnterGame().Forget();
                break;
        }
    }

    private static async UniTask InitRsv()
    {
        List<ICore> _initHs = new List<ICore>()
        {
            new DebugManager(),
            new UIManager(),
            new ResourceManager(),
            new MonoManager(),
        };
        foreach (var init in _initHs)
        {
            init.ICroeInit();
            await UniTask.Yield();
        }
    }

    private static async UniTask InitData()
    {
        List<ICore> _initHs = new List<ICore>()
        {
            new DataManager(),
            new InventoryAllManager(),
        };
        foreach (var init in _initHs)
        {
            init.ICroeInit();
            await UniTask.Yield();
        }
    }
    private static async UniTaskVoid EnterGame()
    {
        await ConfigScenes.FieldScenes.LoadSceneAsyncUnitask(LoadSceneMode.Single);
        await ConfigScenes.PersistentSceneScenes.LoadSceneAsyncUnitask(LoadSceneMode.Additive);

        //打开窗口面板
        ConfigUIPanel.UIActionBarPanel.ShwoUIPanel<ActionBarPanel>();
        ConfigUIPanel.UIItemToolTipPanel.ShwoUIPanel<ItemToolTipPanel>();
        ConfigUIPanel.UIItemToolTipPanel.CloseUIPanel();
        ConfigUIPanel.UIPlayerBagPanel.ShwoUIPanel<PlayerBagPanel>();
        ConfigUIPanel.UIPlayerBagPanel.CloseUIPanel();
    }
}
