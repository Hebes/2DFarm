/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    设置相关

-----------------------*/

namespace ACFrameworkCore
{
    public class ConfigSettings
    {
        public const float itemFadeDuretion = .35f;         //项目淡出时间
        public const float targetAlpha = .45f;              //渐变的目标值

        //时间相关
        public const float secondThreshold = 0.01f;         //数值越小时间越快
        public const int secondHold = 59;                   //多少秒
        public const int minuteHold = 59;                   //多少分钟
        public const int hourHold = 23;                     //一天多少小时
        public const int dayHold = 30;                      //一个月有多少时间
        public const int seasonHold = 3;                    //季节
        public const float fadeDuretion = 1f;             //Loading画面的结束需要的时间
    }
}
