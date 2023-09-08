using ACFrameworkCore;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	剧情TimeLine界面

-----------------------*/

namespace ACFarm
{
    public  class UIIntroPanel :UIBase
    {
        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fade, EUIMode.Normal, EUILucenyType.Pentrate);
        }
    }
}
