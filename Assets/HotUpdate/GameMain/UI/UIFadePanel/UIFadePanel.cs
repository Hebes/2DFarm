using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ACFrameworkCore
{
    public class UIFadePanel : UIBase
    {
        private CanvasGroup fadeCanvasGroup;
        public bool isFade;

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fade, EUIMode.Normal, EUILucenyType.Pentrate);
            fadeCanvasGroup = panelGameObject.GetComponent<CanvasGroup>();
        }
        public override void UIOnEnable()
        {
            base.UIOnEnable();  
            ConfigEvent.UIFade.AddEventListenerUniTask<float>(Fade);
        }
        public override void UIOnDisable()
        {
            base.UIOnDisable(); 
            ConfigEvent.UIFade.RemoveEventListenerUniTask<float>(Fade);
        }



        /// <summary>loading画面淡入淡出场景</summary>
        /// <param name="targetAlpha">1是黑 0是透明</param>
        /// <returns></returns>
        public async UniTask Fade(float targetAlpha)
        {
            isFade = true;
            fadeCanvasGroup.blocksRaycasts = true;
            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / ConfigSettings.fadeDuretion;
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))//Approximately 判断是否大概相似
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                await UniTask.Yield();
                //if (fadeCanvasGroup.alpha < 0.02)//强制退出渐变画面
                //{
                //    fadeCanvasGroup.alpha = 0;
                //    break;
                //}
            }
            fadeCanvasGroup.blocksRaycasts = false;
            isFade = false;
        }
    }
}
