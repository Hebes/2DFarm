/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    Ui面板拓展类

-----------------------*/

namespace ACFrameworkCore
{
    public static class UIManagerExpansion
    {
        public static T ShwoUIPanel<T>(this string uiFormName)where T : UIBase, new()
        {
           return UIManager.Instance.ShwoUIPanel<T>(uiFormName);
        }
        public static void CloseUIPanel(this string uiFormName)
        {
            UIManager.Instance.CloseUIForms(uiFormName);
        }
    }
}
