using System;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	玩家身体动画类型

-----------------------*/

namespace ACFrameworkCore
{
    [Serializable]
    public class AnimatorType
    {
        public EPartType ePartType;
        public EPartName ePartName;
        public AnimatorOverrideController overrideController;
    }
}
