using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Farm2D
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
            ConfigEvent.UIFade.EventAddAsync<float>(Fade);
        }

        /// <summary>loading画面淡入淡出场景</summary>
        /// <param name="targetAlpha">1是黑 0是透明</param>
        /// <returns></returns>
        public async UniTask Fade(float targetAlpha)
        {
            if (targetAlpha == 1)
            {
                fadeCanvasGroup.alpha = 1;
                return;
            }
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

        /// <summary>
        /// 黑幕淡入
        /// </summary>
        /// <param name="self"></param>
        /// <param name="curtain"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        //public static IEnumerator Fadein(this UIMateLoadingComponent self, GameObject curtain, float speed)
        //{
        //    curtain.SetActive(true);
        //    Image image;
        //    image = curtain.GetComponent<Image>();
        //    while (image.color.a >= 0.1f)
        //    {
        //        image.color = Color.Lerp(image.color, Color.clear, speed * Time.deltaTime);
        //        yield return null;
        //    }
        //    curtain.SetActive(false);
        //}

        /// <summary>
        /// 黑幕淡出
        /// </summary>
        /// <param name="self"></param>
        /// <param name="curtain"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        //public static IEnumerator Fadeout(this UIMateLoadingComponent self, float speed)
        //{
        //    self.Curtain.SetActive(true);
        //    Image image;
        //    image = self.Curtain.GetComponent<Image>();
        //    while (image.color.a <= 0.999f)
        //    {
        //        image.color = Color.Lerp(image.color, Color.black, speed * Time.deltaTime);
        //        yield return null;
        //    }
        //}
    }
}
