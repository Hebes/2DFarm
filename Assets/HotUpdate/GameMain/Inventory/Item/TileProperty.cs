using System;
using UnityEngine;

namespace ACFrameworkCore
{
    /// <summary>
    /// 格子的属性
    /// </summary>
    [Serializable]
    public class TileProperty
    {
        /// <summary>
        /// 时间坐标
        /// </summary>
        public Vector2Int tileCoordinate;
        public EGridType eGridType;
        public bool boolTypeValue;
    }
}
