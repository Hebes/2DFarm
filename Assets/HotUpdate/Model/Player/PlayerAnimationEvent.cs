using ACFrameworkCore;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	玩家脚步动画音效

-----------------------*/

namespace ACFarm
{
    public class AnimationEvent : MonoBehaviour
    {
        public void FootstepSound()
        {
            ConfigEvent.PlaySoundEvent.EventTrigger(ESoundName.FootStepSoft);
        }
    }
}
