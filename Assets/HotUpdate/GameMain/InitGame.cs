using ACFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    public async static void Init()
    {
        await InitRsv();
        await Task.Delay(TimeSpan.FromSeconds(2f));
        //InventoryManager inventoryManager = InventoryManager.Instance;
        EnterGame();

        //SwitchInitGameProcess(EInitGameProcess.FSMEnterGame);
    }

    /// <summary>
    /// 切换初始化场景
    /// </summary>
    private static async void SwitchInitGameProcess(EInitGameProcess initGameProcess)
    {
        switch (initGameProcess)
        {
            case EInitGameProcess.FSMInitFramework:
                await InitRsv();
                SwitchInitGameProcess(EInitGameProcess.FSMInitData);
                break;
            case EInitGameProcess.FSMInitData:
                InventoryManager inventoryManager = InventoryManager.Instance;
                SwitchInitGameProcess(EInitGameProcess.FSMEnterGame);
                break;
            case EInitGameProcess.FSMEnterGame:
                EnterGame();
                break;
        }
    }



    private static async Task<string> InitRsv()
    {
        HashSet<ICore> _initHs = new HashSet<ICore>()
        {
            new DebugManager(),
            new UIManager(),
            new ResourceManager(),
            new MonoManager(),
        };
        foreach (var init in _initHs)
        {
            init.ICroeInit();
            await Task.Delay(TimeSpan.FromSeconds(.001f));
        }

        return "核心框架模块已经全都初始化完毕";
    }
    private static void EnterGame()
    {
        DLog.Log("开始游戏!");
        //CUIManager.Instance.ShwoUIPanel<StartPanel>(ConfigUIPanel.StartPanel);
        SceneManager.Instance.LoadSceneAsyn(ConfigScenes.FieldScenes);
        SceneManager.Instance.LoadSceneCommon(ConfigScenes.PersistentSceneScenes);
    }
}
