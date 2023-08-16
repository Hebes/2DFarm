/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    鼠标样式管理

-----------------------*/

namespace ACFrameworkCore
{
    //鼠标类型
    public enum EMouseType
    {
        None = 0,//默认
        Cilck = 1,//点击
    }

    public class MouseSystem : ICore
    {
        public static MouseSystem Instance;
        public void ICroeInit()
        {
            Instance = this;
            //打开UI界面
            ConfigUIPanel.UICursorPanel.ShwoUIPanel<UICursorPanel>();
        }
    }
}
