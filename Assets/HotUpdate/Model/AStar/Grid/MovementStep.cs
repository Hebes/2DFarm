using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	运动的步骤

-----------------------*/

namespace ACFrameworkCore
{
    public class MovementStep
    {
        public string sceneName;            //当前步骤属于的场景
        public int hour;                    //小时
        public int minute;                  //分
        public int second;                  //秒
        public Vector2Int gridCoordinate;   //网格坐标
    }
}
