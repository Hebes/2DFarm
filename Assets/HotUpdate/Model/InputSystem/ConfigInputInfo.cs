/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    配置按键信息

-----------------------*/

namespace ACFrameworkCore
{
    public class ConfigInputInfo
    {
        //默认按键
        public string upDefault = "<Keyboard>/w";
        public string downDefault = "<Keyboard>/s";
        public string leftDefault = "<Keyboard>/a";
        public string rightDefault = "<Keyboard>/d";
        public string fireDefault = "<Mouse>/leftButton";
        public string jumpDefault = "<Keyboard>/space";

        //临时按键
        public string upTemp = "<Keyboard>/w";
        public string downTemp = "<Keyboard>/s";
        public string leftTemp = "<Keyboard>/a";
        public string rightTemp = "<Keyboard>/d";
        public string fireTemp = "<Mouse>/leftButton";
        public string jumpTemp = "<Keyboard>/space";
        //当前按键
        public string upCurrent = "<Keyboard>/w";
        public string downCurrent = "<Keyboard>/s";
        public string leftCurrent = "<Keyboard>/a";
        public string rightCurrent = "<Keyboard>/d";
        public string fireCurrent = "<Mouse>/leftButton";
        public string jumpCurrent = "<Keyboard>/space";
    }
}
