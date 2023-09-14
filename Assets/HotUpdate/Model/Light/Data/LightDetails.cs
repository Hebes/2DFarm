
/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	灯管数据

-----------------------*/

using UnityEngine;

namespace ACFrameworkCore
{
    [System.Serializable]
    public class LightDetails
    {
        public int ID;
        public ESeason season;
        public LightShift lightShift;
        public Color lightColor;
        public float lightAmount;
        public ELightType lightType;
    }
}
