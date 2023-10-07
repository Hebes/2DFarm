using Core;
using UnityEngine;
using Debug = Core.Debug;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	菜单面板

-----------------------*/

namespace Farm2D
{
    public class UIMenuPanel : UIBase
    {
        public GameObject[] panels;

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Mobile, EUIMode.Normal, EUILucenyType.Pentrate);

            UIComponent UIComponent = panelGameObject.GetComponent<UIComponent>();
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
            Debug.Log("退出游戏");
        }
    }
}
