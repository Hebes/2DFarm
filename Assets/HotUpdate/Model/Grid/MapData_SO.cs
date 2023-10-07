using System.Collections.Generic;
using UnityEngine;

namespace Farm2D
{
    [CreateAssetMenu(fileName = "MapData_SO", menuName = "Map/MapData")]
    public class MapData_SO : ScriptableObject
    {
        [SceneName]
        public string sceneName; //场景名称
        public List<TileProperty> tileProperties;//地图块信息
        public int gridWidth;                   //地图宽度  地图中用s建框选地图获取信息Y
        public int gridHeight;                  //地图高度
        public int originX;                     //地图原点x 鼠标地图选中左下角的的格子看x
        public int originY;                     //地图原点Y 鼠标地图选中左下角的的格子看Y
    }
}
