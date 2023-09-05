using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    场景的灯光控制

-----------------------*/

namespace ACFrameworkCore
{
    public class LightControl: MonoBehaviour
    {
        private Light2D currentLight;                   //当前灯光
        private LightDetails currentLightDetails;       //当前灯光数据
        public ELightType lightType;                    //灯光类型

        private void Awake()
        {
            currentLight = GetComponent<Light2D>();
        }

        /// <summary>
        /// 实际切换灯光
        /// </summary>
        /// <param name="season"></param>
        /// <param name="lightShift"></param>
        /// <param name="timeDifference"></param>
        public void ChangeLightShift(ESeason season, LightShift lightShift, float timeDifference)
        {
            currentLightDetails = LightManagerSystem.GetLightDetails(lightType,season, lightShift);

            if (timeDifference < ConfigSettings.lightChangeDuration)
            {
                var colorOffst = (currentLightDetails.lightColor - currentLight.color) / ConfigSettings.lightChangeDuration * timeDifference;
                currentLight.color += colorOffst;
                DOTween.To(() => currentLight.color, c => currentLight.color = c, currentLightDetails.lightColor, ConfigSettings.lightChangeDuration - timeDifference);
                DOTween.To(() => currentLight.intensity, i => currentLight.intensity = i, currentLightDetails.lightAmount, ConfigSettings.lightChangeDuration - timeDifference);
            }
            if (timeDifference >= ConfigSettings.lightChangeDuration)
            {
                currentLight.color = currentLightDetails.lightColor;
                currentLight.intensity = currentLightDetails.lightAmount;
            }
        }
    }
}
