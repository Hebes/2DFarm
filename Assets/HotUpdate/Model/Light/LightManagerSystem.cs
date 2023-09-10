﻿using System;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	灯管管理系统

-----------------------*/

namespace ACFrameworkCore
{
    public class LightManagerSystem : ICore
    {
        public static LightManagerSystem Instance;
        public List<LightDetails> lightPattenList;      //灯光数据

        private LightControl[] sceneLights;
        private LightShift currentLightShift;
        private ESeason currentSeason;
        private float timeDifference = ConfigSettings.lightChangeDuration;

        public void ICroeInit()
        {
            Instance = this;
            lightPattenList = new List<LightDetails>();
            List<LightDetailsData> lightDetailsDatasList = DataExpansion.GetDataList<LightDetailsData>();
            foreach (LightDetailsData lightDetailsData in lightDetailsDatasList)
            {
                LightDetails lightDetails = new LightDetails();
                lightDetails.ID = lightDetailsData.ID;
                lightDetails.season = (ESeason)lightDetailsData.season;
                lightDetails.lightShift = (LightShift)lightDetailsData.lightShift;
                lightDetails.lightColor = new Color(
                    lightDetailsData.lightColorR / 255f,
                    lightDetailsData.lightColorG / 255f,
                    lightDetailsData.lightColorB / 255f,
                    lightDetailsData.lightColorA / 255f);
                lightDetails.lightAmount = lightDetailsData.lightAmount;
                lightPattenList.Add(lightDetails);
            }

            ConfigEvent.AfterSceneLoadedEvent.AddEventListener(OnAfterSceneLoadedEvent);
            ConfigEvent.LightShiftChangeEvent.AddEventListener<ESeason, LightShift, float>(OnLightShiftChangeEvent);
            ConfigEvent.StartNewGameEvent.AddEventListener<int>(OnStartNewGameEvent);
        }



        //事件监听
        private void OnStartNewGameEvent(int obj)
        {
            currentLightShift = LightShift.Morning;
        }
        private void OnAfterSceneLoadedEvent()
        {
            sceneLights = GameObject.FindObjectsOfType<LightControl>();

            //lightcontrol 改变灯光的方法
            foreach (LightControl light in sceneLights)
                light.ChangeLightShift(currentSeason, currentLightShift, timeDifference);
        }
        private void OnLightShiftChangeEvent(ESeason season, LightShift lightShift, float timeDifference)
        {
            currentSeason = season;
            this.timeDifference = timeDifference;
            if (currentLightShift != lightShift)
            {
                currentLightShift = lightShift;

                if (sceneLights != null)
                {
                    //TODO 本来sceneLights!=null是没有的
                    //lightcontrol 改变灯光的方法
                    foreach (LightControl light in sceneLights)
                        light.ChangeLightShift(currentSeason, currentLightShift, timeDifference);
                }
            }
        }



        //公共静态接口
        public static LightDetails GetLightDetails(ELightType lightType, ESeason season, LightShift lightShift)
        {
            LightDetails lightDetails = Instance.lightPattenList.Find(l => l.season == season && l.lightShift == lightShift);
            return lightDetails;
        }
        public static LightDetails GetLightDetails(int id)
        {
            LightDetails lightDetails = Instance.lightPattenList.Find(l => l.ID == id);
            return lightDetails;
        }
    }
}
