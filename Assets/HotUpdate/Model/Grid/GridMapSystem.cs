

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    地图管理系统

-----------------------*/

namespace ACFrameworkCore
{
    public class GridMapSystem : MonoSingleton<GridMapSystem>
    {
        public RuleTile digTile;
        public RuleTile waterTile;

        public List<MapData_SO> mapDataList;

        private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();//场景名字+坐标和对应的瓦片信息
        private Grid currentGrid;

        private Tilemap digTilemap
        {
            get
            {
                return GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
            }
        }
        private Tilemap waterTilemap
        {
            get
            {
                return GameObject.FindWithTag("Water").GetComponent<Tilemap>();
            }
        }

        private void Start()
        {
            foreach (var mapData in mapDataList)
                InitTileDetailsDict(mapData);
        }
        private void OnEnable()
        {
            ConfigEvent.ExecuteActionAfterAnimation.AddEventListener<Vector3, ItemDetails>(OnExecuteActionAfterAnimation);
            ConfigEvent.SceneAfterLoaded.AddEventListener(OnAfterSceneLoadedEvent);
        }
        private void OnDisable()
        {
            ConfigEvent.ExecuteActionAfterAnimation.RemoveEventListener<Vector3, ItemDetails>(OnExecuteActionAfterAnimation);
            ConfigEvent.SceneAfterLoaded.RemoveEventListener(OnAfterSceneLoadedEvent);
        }

       

        /// <summary>
        /// 根据地图信息生成字典
        /// </summary>
        /// <param name="mapData">地图信息</param>
        private void InitTileDetailsDict(MapData_SO mapData)
        {
            foreach (TileProperty tileProperty in mapData.tileProperties)
            {
                //格子信息
                TileDetails tileDetails = new TileDetails
                {
                    girdX = tileProperty.tileCoordinate.x,
                    gridY = tileProperty.tileCoordinate.y
                };

                //字典的Key
                string key = tileDetails.girdX + "x" + tileDetails.gridY + "y" + mapData.sceneName;

                if (GetTileDetails(key) != null)
                    tileDetails = GetTileDetails(key);

                switch (tileProperty.gridType)
                {
                    case EGridType.Diggable: tileDetails.canDig = tileProperty.boolTypeValue; break;
                    case EGridType.DropItem: tileDetails.canDropItem = tileProperty.boolTypeValue; break;
                    case EGridType.PlaceFurniture: tileDetails.canPlaceFurniture = tileProperty.boolTypeValue; break;
                    case EGridType.NPCObstacle: tileDetails.isNPCObstacle = tileProperty.boolTypeValue; break;
                }

                if (GetTileDetails(key) != null)
                    tileDetailsDict[key] = tileDetails;
                else
                    tileDetailsDict.Add(key, tileDetails);
            }
        }


        /// <summary>
        /// 根据key返回瓦片信息
        /// </summary>
        /// <param name="key">x+y+地图名字</param>
        /// <returns></returns>
        private TileDetails GetTileDetails(string key) => tileDetailsDict.ContainsKey(key) ? tileDetailsDict[key] : null;

        /// <summary>
        /// 根据鼠标网格坐标返回瓦片信息
        /// </summary>
        /// <param name="mouseGridPos">鼠标网格坐标k</param>
        /// <returns></returns>
        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;
            return GetTileDetails(key);
        }

        /// <summary>
        /// 执行实际工具或物品功能
        /// </summary>
        /// <param name="mouseWorldPos">鼠标坐标</param>
        /// <param name="itemDetails">物品信息</param>
        private void OnExecuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);//格子的坐标
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);

            if (currentTile != null)
            {
                //WORKFLOW:物品使用实际功能
                switch ((EItemType)itemDetails.itemType)
                {
                    case EItemType.Commdity:
                       // EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos);
                        break;
                    case EItemType.HoeTool:
                        SetDigGround(currentTile);
                        currentTile.daysSinceDug = 0;
                        currentTile.canDig = false;
                        currentTile.canDropItem = false;
                        //音效
                        break;
                    case EItemType.WaterTool:
                        SetWaterGround(currentTile);
                        currentTile.daysSinceWatered = 0;
                        //音效
                        break;
                }
            }
        }

        private void OnAfterSceneLoadedEvent()
        {
            currentGrid = Object.FindObjectOfType<Grid>();
        }

        /// <summary>
        /// 显示挖坑瓦片
        /// </summary>
        /// <param name="tile"></param>
        private void SetDigGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.girdX, tile.gridY, 0);
            if (digTilemap != null)
                digTilemap.SetTile(pos, digTile);
        }
        /// <summary>
        /// 显示浇水瓦片
        /// </summary>
        /// <param name="tile"></param>
        private void SetWaterGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.girdX, tile.gridY, 0);
            if (waterTilemap != null)
                waterTilemap.SetTile(pos, waterTile);
        }
    }
}
