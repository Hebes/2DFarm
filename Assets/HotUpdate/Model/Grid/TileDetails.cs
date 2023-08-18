/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    地图格子信息

-----------------------*/

namespace ACFrameworkCore
{
    [System.Serializable]
    public class TileDetails
    {
        public int girdX, gridY;
        /// <summary>能否挖坑</summary>
        public bool canDig;
        /// <summary>能否扔东西</summary>
        public bool canDropItem;
        /// <summary>能否放置家具</summary>
        public bool canPlaceFurniture;
        /// <summary>是否是NPC的障碍</summary>
        public bool isNPCObstacle;
        /// <summary>记录这个坐标是否被挖过</summary>
        public int daysSinceDug = -1;
        /// <summary>记录这个坐标是否被浇水</summary>
        public int daysSinceWatered = -1;
        /// <summary>种子信息</summary>
        public int seedItemID = -1;
        /// <summary>成长了多少天了</summary>
        public int growthDays = -1;
        /// <summary>上一次收割过了多少天</summary>
        public int daysSinceLastHarvest = -1;
    }
}
