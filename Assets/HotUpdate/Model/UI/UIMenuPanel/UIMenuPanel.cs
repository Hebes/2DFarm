using ACFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	菜单面板

-----------------------*/

namespace ACFarm
{
    public class UIMenuPanel : UIBase
    {
        public GameObject[] panels;

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Mobile, EUIMode.Normal, EUILucenyType.Pentrate);

            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            GameObject T_StartPanel = UIComponent.Get<GameObject>("T_StartPanel");
            GameObject T_SavePanel = UIComponent.Get<GameObject>("T_InfoPanel");
            GameObject T_TitlePanel = UIComponent.Get<GameObject>("T_TitlePanel");

            panels = new GameObject[3];
            panels[0] = T_StartPanel;
            panels[1] = T_SavePanel;
            panels[2] = T_TitlePanel;

            ButtonOnClickAddListener("StartBtn", p => { SwitchPanel(0); });
            ButtonOnClickAddListener("InfoBtn", p => { SwitchPanel(1); });
            ButtonOnClickAddListener("ExitBtn", p => { SwitchPanel(2); });
        }

        public void SwitchPanel(int index)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (i == index)
                {
                    panels[i].transform.SetAsLastSibling();
                }
            }
        }

        public void ExitGame()
        {
            Application.Quit();
            Debug.Log("EXIT GAME");
        }
    }
}
