

using dnlib;
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
    public class GridMapSystem : MonoBehaviour
    {
        public static GridMapSystem Instance;
        public RuleTile digTile;
        public RuleTile waterTile;

        public List<MapData_SO> mapDataList;

        private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();//场景名字+坐标和对应的瓦片信息
        private Dictionary<string, bool> firstLoadDict = new Dictionary<string, bool>();//场景是否第一次加载
        private List<ReapItem> itemsInRadius;//杂草列表
        private Grid currentGrid;

        private Tilemap digTilemap;
        private Tilemap waterTilemap;
        private ESeason currentSeason;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            foreach (MapData_SO mapData in mapDataList)
            {
                firstLoadDict.Add(mapData.sceneName, true);
                InitTileDetailsDict(mapData);
            }
        }
        private void OnEnable()
        {
            ConfigEvent.ExecuteActionAfterAnimation.AddEventListener<Vector3, ItemDetailsData>(OnExecuteActionAfterAnimation);
            ConfigEvent.AfterSceneLoadedEvent.AddEventListener(OnAfterSceneLoadedEvent);
            ConfigEvent.GameDay.AddEventListener<int, ESeason>(OnGameDayEvent);
            ConfigEvent.RefreshCurrentMap.AddEventListener(RefreshMap);
        }
        private void OnDisable()
        {
            ConfigEvent.ExecuteActionAfterAnimation.RemoveEventListener<Vector3, ItemDetailsData>(OnExecuteActionAfterAnimation);
            ConfigEvent.AfterSceneLoadedEvent.RemoveEventListener(OnAfterSceneLoadedEvent);
            ConfigEvent.GameDay.RemoveEventListener<int, ESeason>(OnGameDayEvent);
            ConfigEvent.RefreshCurrentMap.RemoveEventListener(RefreshMap);
        }


        /// <summary>
        /// 根据地图信息生成字典
        /// </summary>
        /// <param name="mapData"></param>
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
        public TileDetails GetTileDetails(string key)
        {
            return tileDetailsDict.ContainsKey(key) ? tileDetailsDict[key] : null;
        }

        /// <summary>
        /// 根据鼠标网格坐标返回瓦片信息
        /// </summary>
        /// <param name="mouseGridPos">鼠标网格坐标k</param>
        /// <returns></returns>
        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            return GetTileDetails(key);
        }

        /// <summary>
        /// 执行实际工具或物品功能
        /// </summary>
        /// <param name="mouseWorldPos">鼠标坐标</param>
        /// <param name="itemDetails">物品信息</param>
        private void OnExecuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetailsData itemDetails)
        {
            Vector3Int mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);//格子的坐标
            TileDetails currentTile = GetTileDetailsOnMousePosition(mouseGridPos);

            if (currentTile == null)
            {
                ACDebug.Error("当前的地图信息是空的");
                return;
            }
            Crop currentCrop = GetCropObject(mouseWorldPos);
            //WORKFLOW:物品使用实际功能
            switch ((EItemType)itemDetails.itemType)
            {
                case EItemType.Seed:
                    ConfigEvent.PlantSeed.EventTrigger(itemDetails.itemID, currentTile);
                    ConfigEvent.UIItemDropItem.EventTrigger(itemDetails.itemID, mouseWorldPos, EItemType.Seed,1);
                    ConfigEvent.PlaySoundEvent.EventTrigger(ESoundName.Plant);
                    break;
                case EItemType.Commdity:
                    ConfigEvent.UIItemDropItem.EventTrigger(itemDetails.itemID, mouseWorldPos, EItemType.Commdity, 10000);
                    break;
                case EItemType.Furniture:
                    //在地图上生成物品 ItemManager
                    //移除当前物品（图纸）InventoryManager
                    //移除资源物品 InventoryManger
                    ConfigEvent.BuildFurniture.EventTrigger( itemDetails.itemID, mouseWorldPos);
                    break;
                case EItemType.HoeTool:
                    SetDigGround(currentTile);
                    currentTile.daysSinceDug = 0;
                    currentTile.canDig = false;
                    currentTile.canDropItem = false;
                    //音效
                    ConfigEvent.PlaySoundEvent.EventTrigger(ESoundName.Hoe);
                    break;
                case EItemType.ChopTool:
                case EItemType.BreakTool:
                    //执行收割方法
                    currentCrop?.ProcessToolAction(itemDetails, currentCrop.tileDetails);
                    break;
                case EItemType.ReapTool:
                    var reapCount = 0;
                    for (int i = 0; i < itemsInRadius.Count; i++)
                    {
                        ConfigEvent.ParticleEffect.EventTrigger(EParticleEffectType.GrassEffect, itemsInRadius[i].transform.position + Vector3.up);
                        itemsInRadius[i].SpawnHarvestItems();
                        Destroy(itemsInRadius[i].gameObject);
                        reapCount++;
                        if (reapCount >= ConfigSettings.reapAmount)//限制销毁杂草收割的数量
                            break;
                    }
                    ConfigEvent.PlaySoundEvent.EventTrigger(ESoundName.Reap);
                    break;
                case EItemType.WaterTool:
                    SetWaterGround(currentTile);
                    currentTile.daysSinceWatered = 0;
                    //音效
                    ConfigEvent.PlaySoundEvent.EventTrigger(ESoundName.Water);
                    break;
                case EItemType.CollectTool:
                    //执行收割方法
                    ACDebug.Log($"收获物品的信息是{currentCrop.cropDetails.producedItemID[0]}");
                    currentCrop?.ProcessToolAction(itemDetails, currentTile);
                    ConfigEvent.PlaySoundEvent.EventTrigger(ESoundName.Basket);
                    break;
                case EItemType.ReapableSceney:
                    break;
                default:
                    break;
            }
            UpdateTileDetails(currentTile);
        }

        /// <summary>
        /// 场景加载之后需要执行的方法
        /// </summary>
        private void OnAfterSceneLoadedEvent()
        {
            currentGrid = Object.FindObjectOfType<Grid>();
            digTilemap = GameObject.FindWithTag(ConfigTag.TagDig).GetComponent<Tilemap>();
            waterTilemap = GameObject.FindWithTag(ConfigTag.TagWater).GetComponent<Tilemap>();

            if (firstLoadDict[UnityEngine.SceneManagement.SceneManager.GetActiveScene().name])
            {
                //预先生成农作物
                ConfigEvent.GenerateCrop.EventTrigger();
                firstLoadDict[UnityEngine.SceneManagement.SceneManager.GetActiveScene().name] = false;
            }

            RefreshMap();
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


        /// <summary>
        /// 每天执行1次
        /// </summary>
        /// <param name="day"></param>
        /// <param name="season"></param>
        private void OnGameDayEvent(int day, ESeason season)
        {
            currentSeason = season;
            foreach (KeyValuePair<string, TileDetails> tile in tileDetailsDict)
            {
                if (tile.Value.daysSinceWatered > -1)
                    tile.Value.daysSinceWatered = -1;
                if (tile.Value.daysSinceDug > -1)
                    tile.Value.daysSinceDug++;
                //超期消除挖坑
                if (tile.Value.daysSinceDug > 5 && tile.Value.seedItemID == -1)
                {
                    tile.Value.daysSinceDug = -1;
                    tile.Value.canDig = true;
                    tile.Value.growthDays = -1;
                }
                if (tile.Value.seedItemID != -1)//已经中了东西了
                    tile.Value.growthDays++;
            }
            RefreshMap();
        }

        /// <summary>
        /// 更新瓦片信息
        /// </summary>
        /// <param name="tileDetails"></param>
        public void UpdateTileDetails(TileDetails tileDetails)
        {
            string key = tileDetails.girdX + "x" + tileDetails.gridY + "y" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (tileDetailsDict.ContainsKey(key))
                tileDetailsDict[key] = tileDetails;
            else
                tileDetailsDict.Add(key, tileDetails);
        }

        /// <summary>
        /// 刷新当前地图
        /// </summary>
        private void RefreshMap()
        {
            if (digTilemap != null)
                digTilemap.ClearAllTiles();
            if (waterTilemap != null)
                waterTilemap.ClearAllTiles();
            //刷新植物的生长日期,显示植物对应的成长阶段
            foreach (Crop crop in FindObjectsOfType<Crop>())
                Destroy(crop.gameObject);

            DisplayMap(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// 显示地图瓦片
        /// </summary>
        /// <param name="sceneName">场景名字</param>
        private void DisplayMap(string sceneName)
        {
            foreach (KeyValuePair<string, TileDetails> tile in tileDetailsDict)
            {
                string key = tile.Key;
                TileDetails tileDetails = tile.Value;

                if (key.Contains(sceneName))
                {
                    if (tileDetails.daysSinceDug > -1)
                        SetDigGround(tileDetails);
                    if (tileDetails.daysSinceWatered > -1)
                        SetWaterGround(tileDetails);
                    if (tileDetails.seedItemID > -1)//有种子信息的话，用于刷新种子的成长周期
                        ConfigEvent.PlantSeed.EventTrigger(tileDetails.seedItemID, tileDetails);
                }
            }
        }

        /// <summary>
        /// 通过物理方法判断鼠标点击位置的农作物
        /// </summary>
        /// <param name="mouseWorldPos">鼠标坐标</param>
        /// <returns></returns>
        public Crop GetCropObject(Vector3 mouseWorldPos)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mouseWorldPos);

            Crop currentCrop = null;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Crop>())
                    currentCrop = colliders[i].GetComponent<Crop>();
            }
            return currentCrop;
        }

        /// <summary>
        /// 返回工具范围内的杂草
        /// </summary>
        /// <param name="tool">物品信息</param>
        /// <returns></returns>
        public bool HaveReapableItemsInRadius(Vector3 mouseWorldPos, ItemDetailsData tool)
        {
            itemsInRadius = new List<ReapItem>();
            //射线检测
            Collider2D[] colliders = new Collider2D[20];
            Physics2D.OverlapCircleNonAlloc(mouseWorldPos, tool.itemUseRadiue, colliders);

            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i] != null)
                    {
                        if (colliders[i].GetComponent<ReapItem>())
                        {
                            var item = colliders[i].GetComponent<ReapItem>();
                            itemsInRadius.Add(item);
                        }
                    }
                }
            }
            return itemsInRadius.Count > 0;
        }

        /// <summary>
        /// 根据场景名字构建网格范围，输出范围和原点
        /// </summary>
        /// <param name="sceneName">场景名字</param>
        /// <param name="gridDimensions">网格范围</param>
        /// <param name="gridOrigin">网格原点</param>
        /// <returns>是否有当前场景的信息</returns>
        public bool GetGridDimensions(string sceneName, out Vector2Int gridDimensions, out Vector2Int gridOrigin)
        {
            gridDimensions = Vector2Int.zero;
            gridOrigin = Vector2Int.zero;

            foreach (var mapData in mapDataList)
            {
                if (mapData.sceneName == sceneName)
                {
                    gridDimensions.x = mapData.gridWidth;
                    gridDimensions.y = mapData.gridHeight;

                    gridOrigin.x = mapData.originX;
                    gridOrigin.y = mapData.originY;

                    return true;
                }
            }
            return false;
        }

        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.tileDetailsDict = this.tileDetailsDict;
            saveData.firstLoadDict = this.firstLoadDict;
            return saveData;
        }

        public void RestoreData(GameSaveData saveData)
        {
            this.tileDetailsDict = saveData.tileDetailsDict;
            this.firstLoadDict = saveData.firstLoadDict;
        }
    }
}
