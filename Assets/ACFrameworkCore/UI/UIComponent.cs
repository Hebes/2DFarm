using Assets.ACFrameworkCore.UI;
using System.Collections.Generic;
using UnityEngine;

namespace ACFrameworkCore
{
    public class UIComponent : ICoreComponent
    {
        public Dictionary<string, UI> panelDic { get; set; }

        private Transform bot;
        private Transform mid;
        private Transform top;
        private Transform system;

        public void OnCroeComponentInit()
        {
            panelDic = new Dictionary<string, UI>();
        }


        public void OnInitPoint()
        {

        }
    }
}
