using System;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    格子的属性

-----------------------*/

namespace ACFrameworkCore
{
    [Serializable]
    public class TileProperty
    {

        /// <summary> 时间坐标 </summary>
        public Vector2Int tileCoordinate;
        public EGridType gridType;
        public bool boolTypeValue;
    }
}
