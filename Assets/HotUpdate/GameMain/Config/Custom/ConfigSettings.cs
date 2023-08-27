/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    设置相关

-----------------------*/

using System;
using UnityEngine;

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

        //割草数量限制
        public const int reapAmount = 2;

        //NPC网格移动
        public const float gridCellSize = 1;
        public const float gridCellDiagonalSize = 1.41f;
        public const float pixelSize = 0.05f;   //20*20 占 1 unit
        public const float animationBreakTime = 5f; //动画间隔时间
        public const int maxGridSize = 9999;

        //灯光
        public const float lightChangeDuration = 25f;
        public static TimeSpan morningTime = new TimeSpan(5, 0, 0);
        public static TimeSpan nightTime = new TimeSpan(19, 0, 0);

        public static Vector3 playerStartPos = new Vector3(-1.7f, -5f, 0);
        public const int playerStartMoney = 100;
    }
}
