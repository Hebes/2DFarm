using ACFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InitGame
{
    public static async void Init()
    {
        Debug.Log("初始化框架核心开始");
        string value = await InitRsv();
        Debug.Log(value);
    }

    private static async Task<string> InitRsv()
    {
        HashSet<ICoreComponent> _initHs = new HashSet<ICoreComponent>()
            {
                new DebugComponent(),
                new AduioComponent(),
                new UIComponent(),
            };

        foreach (var init in _initHs)
        {
            init.OnCroeComponentInit();
            await Task.Delay(TimeSpan.FromSeconds(1.5f));
        }
        return "核心框架模块已经全都初始化完毕!";
    }
}
