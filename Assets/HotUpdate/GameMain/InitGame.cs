using ACFrameworkCore;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

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
        //await InitRsv();
        //EnterGame();
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

                SwitchInitGameProcess(EInitGameProcess.FSMEnterGame).Forget();
                break;
            case EInitGameProcess.FSMEnterGame:
                Debug.Log("进入游戏");
                EnterGame();
                break;
        }
    }

    private static async UniTask InitRsv()
    {
        HashSet<ICore> _initHs = new HashSet<ICore>()
        {
            new DebugManager(),
            new UIManager(),
            new ResourceManager(),
            new MonoManager(),
            new DataManager(),
        };
        foreach (var init in _initHs)
        {
            init.ICroeInit();
            await UniTask.Yield();
        }
    }
    private static void EnterGame()
    {
        ACDebug.Log("开始游戏!");
        //CUIManager.Instance.ShwoUIPanel<StartPanel>(ConfigUIPanel.StartPanel);
        //SceneExpansion.LoadSceneAsync(ConfigScenes.FieldScenes);
        //SceneExpansion.LoadSceneAsync(ConfigScenes.PersistentSceneScenes, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
}
