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

    private static async UniTask InitData()
    {
        HashSet<ICore> _initHs = new HashSet<ICore>()
        {
            new InventoryManager(),
            new DataManager(),
        };
        foreach (var init in _initHs)
        {
            init.ICroeInit();
            await UniTask.Yield();
        }
    }
    private static async UniTaskVoid EnterGame()
    {
        ACDebug.Log("开始游戏!");
        await ConfigScenes.FieldScenes.LoadSceneAsyncUnitask(LoadSceneMode.Single);
        await ConfigScenes.PersistentSceneScenes.LoadSceneAsyncUnitask(UnityEngine.SceneManagement.LoadSceneMode.Additive);

        //打开窗口面板
        //CUIManager.Instance.ShwoUIPanel<StartPanel>(ConfigUIPanel.StartPanel);
        ActionBarPanel actionBarPanel = ConfigUIPanel.ActionBarPanel.ShwoUIPanel<ActionBarPanel>();
        ItemToolTipPanel itemToolTipPanel =  ConfigUIPanel.ItemToolTipPanel.ShwoUIPanel<ItemToolTipPanel>();
        itemToolTipPanel.gameObject.SetActive(false);
        PlayerBagPanel playerBagPanel =  ConfigUIPanel.PlayerBagPanel.ShwoUIPanel<PlayerBagPanel>();
        playerBagPanel.gameObject.SetActive(false);
        //创建物体
        GameObject gameObject = ConfigPrefab.ItemBasePrefab.YooaddetLoadAsyncAsT<GameObject>();
        //GameObject gameObject =  ResourceExtension.LoadAsyncAsT<GameObject>(ConfigPrefab.ItemBasePrefab);
        GameObject go = GameObject.Instantiate(gameObject);
        Debug.Log($"物体名称是  {go.name}");
    }
}
