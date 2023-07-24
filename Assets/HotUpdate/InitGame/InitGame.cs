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
        //DLog.Log("初始化框架核心开始");

        //GameObject gameObject = new GameObject("test");
       

        string value = await InitRsv();
        DLog.Log(value);

        //ResComponent.Insatance.LoadAssetAsync<GameObject>("Cube", go =>
        //{
        //    GameObject.Instantiate(go);
        //});
        DLog.Log("开始创建物体");

        //var package = YooAssets.GetPackage("PC");
        //AssetOperationHandle handle1 = package.LoadAssetAsync<GameObject>("Cube");
        //handle1.Completed += Handle_Completed;

        //AssetOperationHandle handle2 = package.LoadAssetAsync<GameObject>("Cube");
        //handle2.Completed += Handle_Completed1;


        //AssetOperationHandle handle3 = package.LoadAssetAsync<GameObject>("Cube");
        //handle3.Completed += Handle_Completed2;

        //DLog.Log("创建物体结束");
        //GameObject go = ResComponent.Insatance.LoadAsset<GameObject>("Cube");
        //GameObject.Instantiate(go);
        //DLog.Log(go.name);
        //ResComponent.Insatance.LoadAssetAsync<GameObject>("Cube",null);
    }

    private static void Handle_Completed(AssetOperationHandle obj)
    {
        GameObject go = obj.InstantiateSync();
        Debug.Log($"Prefab name is {go.name}");
    } 
    
    private static void Handle_Completed1(AssetOperationHandle obj)
    {
        GameObject go = obj.InstantiateSync();
        go.transform.position = new Vector3(1, 1, 1);

        Debug.Log($"Prefab name is {go.name}");
    }
    
    private static void Handle_Completed2(AssetOperationHandle obj)
    {
        GameObject go = obj.InstantiateSync();
        go.transform.position = new Vector3(2, 2, 2);

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
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
        }
        return "核心框架模块已经全都初始化完毕1!";
    }
}
