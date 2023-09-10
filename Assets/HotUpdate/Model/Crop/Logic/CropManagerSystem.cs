using UnityEngine;
using ACFrameworkCore;
using System.Collections.Generic;

/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    庄稼管理

-----------------------*/

namespace ACFarm
{
    public class CropManagerSystem : ICore
    {
        public static CropManagerSystem Instance;
        private Transform cropParent;       //庄家的父物体，都放在这个下面不会看上去太乱
        private Grid currentGrid;
        private ESeason currentSeason;
        public List<CropDetails> cropDetailsList;
        public void ICroeInit()
        {
            Instance = this;

            ConfigEvent.PlantSeed.AddEventListener<int, TileDetails>(OnPlantSeedEvent);
            ConfigEvent.AfterSceneLoadedEvent.AddEventListener(OnAfterSceneLoadedEvent);
            ConfigEvent.GameDay.AddEventListener<int, ESeason>(OnGameDayEvent);

            //加载数据
            CropDataList_SO cropDataList_S = YooAssetLoadExpsion.YooaddetLoadSync<CropDataList_SO>("CropDataList_SO");
            cropDetailsList = cropDataList_S.cropDetailsList;
            //ACDebug.Log($"需要打印的消息{1}");
        }


        private void OnGameDayEvent(int day, ESeason season)
        {
            currentSeason = season;
        }
        private void OnAfterSceneLoadedEvent()
        {
            //currentGrid = GameObject.FindObjectOfType<Grid>();
            cropParent = SceneTransitionManagerSystem.Instance.cropParent;
        }
        private void OnPlantSeedEvent(int ID, TileDetails tileDetails)
        {
            CropDetails currentCrop = GetCropDetails(ID);
            if (currentCrop != null && SeasonAvailable(currentCrop) && tileDetails.seedItemID == -1)    //用于第一次种植
            {
                tileDetails.seedItemID = ID;
                tileDetails.growthDays = 0;
                //显示农作物
                DisplayCropPlant(tileDetails, currentCrop);
            }
            else if (tileDetails.seedItemID != -1)  //用于刷新地图
            {
                //显示农作物
                DisplayCropPlant(tileDetails, currentCrop);
            }
        }


        /// <summary>
        /// 显示农作物
        /// </summary>
        /// <param name="tileDetails">瓦片地图信息</param>
        /// <param name="cropDetails">种子信息</param>
        private void DisplayCropPlant(TileDetails tileDetails, CropDetails cropDetails)
        {
            //成长阶段
            int growthStages = cropDetails.growthDays.Length;
            int currentStage = 0;
            int dayCounter = cropDetails.TotalGrowthDays;

            //倒序计算当前的成长阶段
            for (int i = growthStages - 1; i >= 0; i--)
            {
                if (tileDetails.growthDays >= dayCounter)
                {
                    currentStage = i;
                    break;
                }
                dayCounter -= cropDetails.growthDays[i];
            }

            //获取当前阶段的Prefab
            GameObject cropPrefab = cropDetails.growthPrefabs[currentStage];
            Sprite cropSprite = cropDetails.growthSprites[currentStage];

            Vector3 pos = new Vector3(tileDetails.girdX + 0.5f, tileDetails.gridY + 0.5f, 0);

            GameObject cropInstance = GameObject.Instantiate(cropPrefab, pos, Quaternion.identity, cropParent);
            cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;
            cropInstance.GetComponent<Crop>().cropDetails = cropDetails;//设置数据
            cropInstance.GetComponent<Crop>().tileDetails = tileDetails;
        }

        /// <summary>
        /// 通过物品ID查找种子信息
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <returns></returns>
        public CropDetails GetCropDetails(int ID)
        {
            return cropDetailsList.Find(c => c.seedItemID == ID);
        }

        /// <summary>
        /// 判断当前季节是否可以种植
        /// </summary>
        /// <param name="crop">种子信息</param>
        /// <returns></returns>
        private bool SeasonAvailable(CropDetails crop)
        {
            for (int i = 0; i < crop.seasons.Length; i++)
            {
                if (crop.seasons[i] == currentSeason)
                    return true;
            }
            return false;
        }


    }
}