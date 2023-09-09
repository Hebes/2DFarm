using ACFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	设置兼暂停面板

-----------------------*/

namespace ACFarm
{
    public class UISettingPanel:UIBase
    {

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.HideOther, EUILucenyType.ImPenetrable);

            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();

            GameObject T_MusicSlider = UIComponent.Get<GameObject>("T_MusicSlider");
            GameObject T_SettingBtn = UIComponent.Get<GameObject>("T_SettingBtn");
            GameObject T_RestBtn = UIComponent.Get<GameObject>("T_RestBtn");
            GameObject T_SettingTab = UIComponent.Get<GameObject>("T_SettingTab");
            GameObject T_RestTab = UIComponent.Get<GameObject>("T_RestTab");


            ButtonOnClickAddListener(T_SettingBtn.name, p => { TogglePausePanel(); });


            //T_MusicSlider.GetComponent<Slider>().onValueChanged.AddListener(AudioManagerSystem.Instance.SetMasterVolume);
        }


        private void TogglePausePanel()
        {
            bool isOpen = panelGameObject.activeInHierarchy;

            if (isOpen)
            {
                CloseUIForm();
                //pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                System.GC.Collect();
                //OpenUIForm<UISettingPanel>()
                Time.timeScale = 0;
            }
        }

        
        public void ReturnMenuCanvas()
        {
            //Time.timeScale = 1;
            //StartCoroutine(BackToMenu());
        }

        private IEnumerator BackToMenu()
        {
            //pausePanel.SetActive(false);

            //EventHandler.CallEndGameEvent();
           yield return new WaitForSeconds(1f);
        }
    }
}
