using System.Collections.Generic;
using UnityEngine;

namespace ACFrameworkCore
{
    [CreateAssetMenu(fileName = "MapData_SO", menuName = "Map/MapData")]
    public class MapData_SO : ScriptableObject
    {
        [SceneName]
        public string sceneName; //场景名称
        public List<TileProperty> tileProperties;//地图块信息
    }
}
