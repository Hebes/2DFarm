using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ACFrameworkCore
{
    public class InitGame : MonoBehaviour
    {
        private async void Awake()
        {
            await InitRsv();
        }


        private async Task<string> InitRsv()
        {
            HashSet<ICoreComponent> _initHs = new HashSet<ICoreComponent>()
            {
                new DebugComponent(),
                new AduioComponent(),

            };

            foreach (var init in _initHs)
            {
                init.OnCroeComponentInit();
                await Task.Delay(TimeSpan.FromSeconds(1.5f));
            }
            return "InitRsvOver";
        }

    }
}
