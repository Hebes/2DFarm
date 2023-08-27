using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    单个地图的信息

-----------------------*/

namespace ACFrameworkCore
{
    [ExecuteInEditMode]//在编辑的模式下运行
    public class GridMap : MonoBehaviour
    {
        public MapData_SO mapData;      //地图数据
        public EGridType gridType;      //网格类型
        private Tilemap currentTilemap; //瓦片地图

        private void OnEnable()
        {
            if (Application.IsPlaying(this)) return;//是否在播放
            currentTilemap = GetComponent<Tilemap>(); 
            if (mapData != null)
                mapData.tileProperties.Clear();
        }
        private void OnDisable()
        {
            if (Application.IsPlaying(this)) return;//是否在播放
            currentTilemap = GetComponent<Tilemap>();
            UpdateTileProperties();
#if UNITY_EDITOR
            if (mapData != null)
                EditorUtility.SetDirty(mapData);//用来实时保存和修改
#endif
        }

        /// <summary>
        /// 更新瓦片信息
        /// </summary>
        private void UpdateTileProperties()
        {
            if (!Application.IsPlaying(this))
            {
                if (mapData != null)
                {
                    //压缩磁贴映射的原点和大小到磁贴存在的边界。 获取实际大小的格子
                    currentTilemap.CompressBounds();
                    // 已绘制范围的左 下角坐标
                    Vector3Int startPos = currentTilemap.cellBounds.min;
                    //已绘制范围的右上角坐标
                    Vector3Int endPos = currentTilemap.cellBounds.max;
                    //每个格子的坐标
                    for (int x = startPos.x; x < endPos.x; x++)
                    {
                        for (int y = startPos.y; y < endPos.y; y++)
                        {
                            TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));
                            if (tile != null)
                            {
                                TileProperty newTile = new TileProperty
                                {
                                    tileCoordinate = new Vector2Int(x, y),
                                    gridType = this.gridType,
                                    boolTypeValue = true
                                };
                                mapData.tileProperties.Add(newTile);
                            }
                        }
                    }
                }
            }
        }
    }
}
