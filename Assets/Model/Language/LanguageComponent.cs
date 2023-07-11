/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    初始化语言字典

-----------------------*/

namespace ACFrameworkCore
{
    public class LanguageComponent
    {
        private static LanguageComponent instance;

        public static LanguageComponent Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LanguageComponent();
                    return instance;
                }
                else
                    return instance;
            }
        }

        public void OnChangeLanguage()
        {
            LanaguageBridge.Instance.OnLanguageChange();
        }

        public void OnSetLanguageDic()
        {
            
        }
    }
}
