using UnityEngine;
using DG.Tweening;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    走进物体,颜色变淡

-----------------------*/

namespace Farm2D
{
    [RequireComponent(typeof(SpriteRenderer))]//必须挂载SpriteRenderer组件
    public class ItemFader : MonoBehaviour
    {
        private SpriteRenderer SpriteRenderer;

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// 逐渐恢复颜色
        /// </summary> 
        public void fadeIn()
        {
            Color TargetColor = new Color(1, 1, 1, 1);
            SpriteRenderer.DOColor(TargetColor, ConfigSettings.itemFadeDuretion);
        }

        ///// <summary>
        ///// 逐渐半透明
        ///// </summary>
        public void fadeOut()
        {
            Color TargetColor = new Color(1, 1, 1, ConfigSettings.targetAlpha);
            SpriteRenderer.DOColor(TargetColor, ConfigSettings.itemFadeDuretion);
        }
    }
}
