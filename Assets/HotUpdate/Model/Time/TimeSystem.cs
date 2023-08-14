using UnityEngine;

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

    public class TimeSystem : ICore
    {
        private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
        private ESeason eGameSeason = ESeason.春天;
        private int monthInSeason = 3;  //每个季度有多少个月
        public bool gameCLockPause;     //时间的暂停
        private float tiktime;          //计时器

        public void ICroeInit()
        {
            NewGameTime();

            GameObject TimeGo = new GameObject("TimeGo");

            MonoManager.Instance.OnAddUpdateEvent(OnUpdate);
            ConfigEvent.GameDate.EventTrigger(gameHour, gameDay, gameMonth, gameYear, eGameSeason);
            ConfigEvent.GameMinute.EventTrigger(gameMinute, gameHour);
        }

        private void OnUpdate()
        {
            if (!gameCLockPause)
            {
                tiktime += Time.deltaTime;
                if (tiktime >= ConfigSettings.secondThreshold)
                {
                    tiktime -= ConfigSettings.secondThreshold;
                    UpdateGameTime();
                }
            }

            if (Input.GetKey(KeyCode.T))
            {
                for (int i = 0; i < 120; i++)
                {
                    UpdateGameTime();
                }
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
            eGameSeason = ESeason.春天;
        }
        /// <summary>时间更新</summary>
        private void UpdateGameTime()
        {
            gameSecond++;//秒钟++
            if (gameSecond > ConfigSettings.secondHold)
            {
                gameMinute++;//分

                gameSecond = 0;
                if (gameMinute > ConfigSettings.minuteHold)
                {
                    gameHour++;//小时
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
                                monthInSeason = 3;
                            int seasonNumber = (int)eGameSeason;
                            seasonNumber++;

                            if (seasonNumber > ConfigSettings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++;
                            }
                            eGameSeason = (ESeason)seasonNumber;

                            if (gameYear > 9999)
                            {
                                gameYear = 2022;
                            }
                        }
                    }
                }
                ConfigEvent.GameDate.EventTrigger(gameHour, gameDay, gameMonth, gameYear, eGameSeason);
            }
            ConfigEvent.GameMinute.EventTrigger(gameMinute, gameHour);
            //Debug.Log("Second 秒：" + gameSecond + "Minute 分：" + gameMinute);
        }
    }
}
