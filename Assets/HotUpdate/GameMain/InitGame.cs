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
    FSMInitUI,//初始化UI
    FSMInitSaveData,//加载保存的数据
    FSMEnterGame,//进入游戏
}
public class InitGame
{
    public static void Init()
    {
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
                SwitchInitGameProcess(EInitGameProcess.FSMInitSaveData).Forget();
                break;
            case EInitGameProcess.FSMInitSaveData:
                FSMInitSaveData().Forget();
                SwitchInitGameProcess(EInitGameProcess.FSMInitUI).Forget();
                 break;
            case EInitGameProcess.FSMInitUI:
                FSMInitUI();
                SwitchInitGameProcess(EInitGameProcess.FSMEnterGame).Forget();
                break;
            case EInitGameProcess.FSMEnterGame:
                Debug.Log("进入游戏");
                EnterGame().Forget();
                break;
        }
    }

    //初始化框架
    private static async UniTask InitRsv()
    {
        List<ICore> _initHs = new List<ICore>()
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

    //初始化需要的数据
    private static async UniTask InitData()
    {
        List<ICore> _initHs = new List<ICore>()
        {
            new InventoryAllManager(),
            new InventoryWorldItemManager(),
            new TimeSystem(),
            new SceneTransitionSystem(),
        };
        foreach (var init in _initHs)
        {
            init.ICroeInit();
            await UniTask.Yield();
        }
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
    }

    private static async UniTaskVoid FSMInitSaveData()
    {
        await UniTask.Yield();
    }
    private static async UniTaskVoid EnterGame()
    {
        ////测试创建拾取的物体
        //GameObject gameObject = await ResourceExtension.LoadAsyncUniTask<GameObject>(ConfigPrefab.ItemBasePrefab);

        //GameObject go1 = GameObject.Instantiate(gameObject);
        //Item item = go1.GetComponent<Item>();
        //item.itemID = 1007;
        //item.itemAmount = 3;

        //GameObject go2 = GameObject.Instantiate(gameObject);
        //Item item2 = go2.GetComponent<Item>();
        //item2.itemID = 1008;
        //item2.itemAmount = 6;

        //GameObject go3 = GameObject.Instantiate(gameObject);
        //Item item3 = go3.GetComponent<Item>();
        //item3.itemID = 1015;
        //item3.itemAmount = 119;
    }
}
