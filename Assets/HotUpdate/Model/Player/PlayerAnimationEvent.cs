using Core;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	玩家脚步动画音效(在Body动画控制器中添加了这个事件监听)

-----------------------*/

namespace Farm2D
{
    public class PlayerAnimationEvent : MonoBehaviour
    {
        public void FootstepSound()
        {
            ConfigEvent.PlaySoundEvent.EventTrigger(ESoundName.FootStepSoft);
        }
    }
}
