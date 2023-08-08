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
    public static async UniTaskVoid Init()
    {

        Debug.Log("初始化场景");
        //await InitRsv();
        //EnterGame();
        //await SwitchInitGameProcess(EInitGameProcess.FSMEnterGame);
        var ss = await getid();
        Debug.Log(ss);
    }
    private static async UniTask<int> getid()
    {
        await UniTask.Delay(1000);
        return 0;
    }

    /// <summary>
    /// 切换初始化场景
    /// </summary>
    private static async UniTask SwitchInitGameProcess(EInitGameProcess initGameProcess)
    {
        switch (initGameProcess)
        {
            case EInitGameProcess.FSMInitFramework:
                await InitRsv();
                await SwitchInitGameProcess(EInitGameProcess.FSMInitData);
                break;
            case EInitGameProcess.FSMInitData:
                await SwitchInitGameProcess(EInitGameProcess.FSMEnterGame);
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
