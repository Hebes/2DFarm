using ACFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

//namespace ACFrameworkCore
//{
    public class InitGame 
    {

        public static void Init()
        {
            Debug.Log("热更新代码444");
        }

        private async void Awake()
        {
            await InitRsv();
            //GameObject.DontDestroyOnLoad(this.gameObject);
        }


        private async Task<string> InitRsv()
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
            return "InitRsvOver";
        }

    }
//}
