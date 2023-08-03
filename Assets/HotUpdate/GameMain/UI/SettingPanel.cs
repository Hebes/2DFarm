using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACFrameworkCore
{
    public class SettingPanel : UIBase
    {
        //public SettingPanel(EUIType type, EUIMode mod, EUILucenyType lucenyType, bool isClearStack = false) : base(type, mod, lucenyType, isClearStack)
        //{
        //}
        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Normal, EUIMode.HideOther, EUILucenyType.Translucence);
            RigisterButtonObjectEvent("Break",
               p =>
               {
                   CloseUIForm();
               });
        }
    }
}
