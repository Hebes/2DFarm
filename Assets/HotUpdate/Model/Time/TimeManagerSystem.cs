using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    时间系统

-----------------------*/

namespace ACFrameworkCore
{

    public class TimeManagerSystem : ICore
    {
        public static TimeManagerSystem Instance;
        private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
        private ESeason gameSeason = ESeason.春天;
        private int monthInSeason = 3;  //每个季度有多少个月
        public bool gameClockPause;     //时间的暂停(true表示暂停)
        private float tiktime;          //计时器
        private float timeDifference;   //灯光时间差

        public TimeSpan GameTime => new TimeSpan(gameHour, gameMinute, gameSecond);

        public void ICroeInit()
        {
            Instance = this;

            MonoManager.Instance.OnAddUpdateEvent(OnUpdate);

            ConfigEvent.BeforeSceneUnloadEvent.AddEventListener(OnBeforeSceneUnloadEvent);
            ConfigEvent.AfterSceneLoadedEvent.AddEventListener(OnAfterSceneLoadedEvent);
            ConfigEvent.UpdateGameStateEvent.AddEventListener<EGameState>(OnUpdateGameStateEvent);
            ConfigEvent.StartNewGameEvent.AddEventListener<int>(OnStartNewGameEvent);
            ConfigEvent.EndGameEvent.AddEventListener(OnEndGameEvent);
        }

        private void OnBeforeSceneUnloadEvent()
        {
            // gameClockPause = true;
        }

        private void OnUpdateGameStateEvent(EGameState gameState)
        {
            gameClockPause = gameState == EGameState.Pause;
        }

        private void OnAfterSceneLoadedEvent()
        {
            // gameClockPause = false;
            ConfigEvent.GameDate.EventTrigger(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            ConfigEvent.GameMinute.EventTrigger(gameMinute, gameHour, gameDay, gameSeason);
            //切换灯光
            ConfigEvent.LightShiftChangeEvent.EventTrigger(gameSeason, GetCurrentLightShift(), timeDifference);
        }
        private void OnStartNewGameEvent(int obj)
        {
            NewGameTime();
            // gameClockPause = false;
        }
        
        private void OnEndGameEvent()
        {
            gameClockPause = true;
        }


        private void OnUpdate()
        {
            if (!gameClockPause)
            {
                tiktime += Time.deltaTime;
                if (tiktime >= ConfigSettings.secondThreshold)
                {
                    tiktime -= ConfigSettings.secondThreshold;
                    UpdateGameTime();
                }
            }

            //时间增加
            if (Input.GetKey(KeyCode.T))
            {
                for (int i = 0; i < 120; i++)
                {
                    UpdateGameTime();
                }
            }

            //日期增加
            if (Input.GetKey(KeyCode.G))
            {
                gameDay++;
                ConfigEvent.GameDay.EventTrigger(gameDay, gameSeason);
                ConfigEvent.GameDate.EventTrigger(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
        }

        /// <summary>用于初始化时间</summary>
        private void NewGameTime()
        {
            gameSecond = 0;
            gameMinute = 0;
            gameHour = 7;
            gameDay = 1;
            gameMonth = 1;
            gameYear = 2022;
            gameSeason = ESeason.春天;
        }
        /// <summary>时间更新</summary>
        private void UpdateGameTime()
        {
            gameSecond++;
            if (gameSecond > ConfigSettings.secondHold)
            {
                gameMinute++;
                gameSecond = 0;

                if (gameMinute > ConfigSettings.minuteHold)
                {
                    gameHour++;
                    gameMinute = 0;

                    if (gameHour > ConfigSettings.hourHold)
                    {
                        gameDay++;
                        gameHour = 0;

                        if (gameDay > ConfigSettings.dayHold)
                        {
                            gameDay = 1;
                            gameMonth++;

                            if (gameMonth > 12)
                                gameMonth = 1;

                            monthInSeason--;
                            if (monthInSeason == 0)
                            {
                                monthInSeason = 3;

                                int seasonNumber = (int)gameSeason;
                                seasonNumber++;

                                if (seasonNumber > ConfigSettings.seasonHold)
                                {
                                    seasonNumber = 0;
                                    gameYear++;
                                }

                                gameSeason = (ESeason)seasonNumber;

                                if (gameYear > 9999)
                                {
                                    gameYear = 2022;
                                }
                            }
                            //用来刷新地图和农作物生长
                            ConfigEvent.GameDay.EventTrigger(gameDay, gameSeason);
                        }
                    }
                    ConfigEvent.GameDate.EventTrigger(gameHour, gameDay, gameMonth, gameYear, gameSeason);
                }
                ConfigEvent.GameMinute.EventTrigger(gameMinute, gameHour, gameDay, gameSeason);
                ConfigEvent.LightShiftChangeEvent.EventTrigger(gameSeason, GetCurrentLightShift(), timeDifference);//切换灯光
            }
            //Debug.Log("Second 秒：" + gameSecond + "Minute 分：" + gameMinute);
        }


        /// <summary>
        /// 返回lightshift同时计算时间差
        /// </summary>
        /// <returns></returns>
        private LightShift GetCurrentLightShift()
        {
            if (GameTime >= ConfigSettings.morningTime && GameTime < ConfigSettings.nightTime)
            {
                timeDifference = (float)(GameTime - ConfigSettings.morningTime).TotalMinutes;
                return LightShift.Morning;
            }

            if (GameTime < ConfigSettings.morningTime || GameTime >= ConfigSettings.nightTime)
            {
                timeDifference = Mathf.Abs((float)(GameTime - ConfigSettings.nightTime).TotalMinutes);
                // Debug.Log(timeDifference);
                return LightShift.Night;
            }

            return LightShift.Morning;
        }

        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.timeDict = new Dictionary<string, int>();
            saveData.timeDict.Add("gameYear", gameYear);
            saveData.timeDict.Add("gameSeason", (int)gameSeason);
            saveData.timeDict.Add("gameMonth", gameMonth);
            saveData.timeDict.Add("gameDay", gameDay);
            saveData.timeDict.Add("gameHour", gameHour);
            saveData.timeDict.Add("gameMinute", gameMinute);
            saveData.timeDict.Add("gameSecond", gameSecond);

            return saveData;
        }

        public void RestoreData(GameSaveData saveData)
        {
            gameYear = saveData.timeDict["gameYear"];
            gameSeason = (ESeason)saveData.timeDict["gameSeason"];
            gameMonth = saveData.timeDict["gameMonth"];
            gameDay = saveData.timeDict["gameDay"];
            gameHour = saveData.timeDict["gameHour"];
            gameMinute = saveData.timeDict["gameMinute"];
            gameSecond = saveData.timeDict["gameSecond"];
        }
    }
}
