/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    工具栏

-----------------------*/

namespace ACFrameworkCore
{
    public class ActionBarPanel : UIBase
    {
        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);

            //RigisterButtonObjectEvent("Back",
            //  p =>
            //  {
            //      CloseUIForm();
            //  });
        }
    }
}
