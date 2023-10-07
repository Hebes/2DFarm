using Core;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	设置兼暂停面板

-----------------------*/

namespace Farm2D
{
    public class UISettingPanel : UIBase
    {

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.HideOther, EUILucenyType.Pentrate);

            UIComponent UIComponent = panelGameObject.GetComponent<UIComponent>();

            GameObject T_MusicSlider = UIComponent.Get<GameObject>("T_MusicSlider");
            GameObject T_SettingBtn = UIComponent.Get<GameObject>("T_SettingBtn");
            GameObject T_RestBtn = UIComponent.Get<GameObject>("T_RestBtn");
            GameObject T_SettingTab = UIComponent.Get<GameObject>("T_SettingTab");
            GameObject T_RestTab = UIComponent.Get<GameObject>("T_RestTab");


            ButtonOnClickAddListener(T_SettingBtn.name, p => { TogglePausePanel(); });
            ButtonOnClickAddListener(T_RestBtn.name, p => { ReturnMenuCanvas().Forget(); });
            //T_MusicSlider.GetComponent<Slider>().onValueChanged.AddListener(AudioManagerSystem.Instance.SetMasterVolume);
        }


        private void TogglePausePanel()
        {
            bool isOpen = panelGameObject.activeInHierarchy;

            if (isOpen)
            {
                CloseUIForm();
                CoreMono.Instance.Pause(1);
            }
            else
            {
                CoreMono.Instance.Pause(0);
                System.GC.Collect();
                OpenUIForm<UISettingPanel>(ConfigUIPanel.UISettingPanel);
            }
        }

        /// <summary>
        /// 返回到主菜单
        /// </summary>
        /// <returns></returns>
        public async UniTask ReturnMenuCanvas()
        {
            CoreMono.Instance.Pause(1);
            CloseUIForm();
            ConfigEvent.EndGameEvent.EventTrigger();
            await UniTask.Delay(TimeSpan.FromSeconds(1f), false);
        }
    }
}
