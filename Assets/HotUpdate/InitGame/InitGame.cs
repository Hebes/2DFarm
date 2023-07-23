using ACFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class InitGame
{
    public static async void Init()
    {
        Debug.Log("初始化框架核心开始");
        string value = await InitRsv();
        Debug.Log(value);

        //var package = YooAssets.GetPackage("PC");
        //AssetOperationHandle handle1 = package.LoadAssetAsync<GameObject>("Cube");
        //handle1.Completed += Handle_Completed;
        //ResComponent.Insatance.LoadAssetAsync<GameObject>("Cube", go =>
        //{
        //    GameObject.Instantiate(go);
        //});
        DLog.Log("开始创建物体");
        if (ResComponent.Insatance == null)
        {
            DLog.Log("ResComponent空了");

        }
        GameObject go = ResComponent.Insatance.LoadAsset<GameObject>("Cube");
        GameObject.Instantiate(go);
        //DLog.Log(go.name);
        //ResComponent.Insatance.LoadAssetAsync<GameObject>("Cube",null);
    }

    private static void Handle_Completed(AssetOperationHandle obj)
    {
        GameObject go = obj.InstantiateSync();
        Debug.Log($"Prefab name is {go.name}");
    }

    private static async Task<string> InitRsv()
    {
        HashSet<ICoreComponent> _initHs = new HashSet<ICoreComponent>()
            {
                new DebugComponent(),
                new AduioComponent(),
                new UIComponent(),
                new ResComponent(),
                new MonoComponent(),
            };

        foreach (var init in _initHs)
        {
            init.OnCroeComponentInit();
            await Task.Delay(TimeSpan.FromSeconds(1.5f));
        }
        return "核心框架模块已经全都初始化完毕!";
    }
}
