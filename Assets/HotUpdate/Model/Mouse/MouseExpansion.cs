using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	鼠标帮助类

-----------------------*/

namespace ACFarm
{
    public static class MouseExpansion
    {
        public static Sprite GetMouseSprite(this string mouseName)
        {
            return MouseManagerSystem.Instance.GetMouseSprite(mouseName);
        }
    }
}
