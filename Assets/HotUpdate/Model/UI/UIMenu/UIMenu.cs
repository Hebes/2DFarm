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
    public class UIMenu : UIBase
    {
        private GameObject T_TitlePanel;
        private GameObject T_InfoPanel;
        private GameObject T_StartPanel;

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);

            UIComponent UIComponent = panelGameObject.GetComponent<UIComponent>();
            GameObject T_TitleBtn = UIComponent.Get<GameObject>("T_TitleBtn");
            GameObject T_ExitBtn = UIComponent.Get<GameObject>("T_ExitBtn");
            GameObject T_InfoBtn = UIComponent.Get<GameObject>("T_InfoBtn");
            GameObject T_StartBtn = UIComponent.Get<GameObject>("T_StartBtn");
            T_TitlePanel = UIComponent.Get<GameObject>("T_TitlePanel");
            T_InfoPanel = UIComponent.Get<GameObject>("T_InfoPanel");
            T_StartPanel = UIComponent.Get<GameObject>("T_StartPanel");

            ButtonOnClickAddListener(T_TitleBtn.name, p => { SwitchPanel(T_TitlePanel.name); });
            ButtonOnClickAddListener(T_StartBtn.name, p => { SwitchPanel(T_StartPanel.name); });
            ButtonOnClickAddListener(T_InfoBtn.name, p => { SwitchPanel(T_InfoPanel.name); });
            ButtonOnClickAddListener(T_ExitBtn.name, ExitGame);
            SwitchPanel(T_TitlePanel.name);
        }

       

        /// <summary>
        /// 切换面板
        /// </summary>
        /// <param name="index"></param>
        public void SwitchPanel(string btnName)
        {
            T_TitlePanel.SetActive(false);
            T_InfoPanel.SetActive(false);
            T_StartPanel.SetActive(false);
            switch (btnName)
            {
                case "T_StartPanel":
                    T_StartPanel.SetActive(true);
                    break;
                case "T_InfoPanel":
                    T_InfoPanel.SetActive(true);
                    break;
                case "T_TitlePanel":
                    T_TitlePanel.SetActive(true);
                    break;
            }
        }

        private void ExitGame(GameObject go)
        {
            Application.Quit();
            Debug.Log("退出游戏");
        }
    }
}
