using ACFrameworkCore;
using System.Collections.Generic;

/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    数据槽位

-----------------------*/

namespace ACFarm
{
    public class DataSlot
    {
        /// <summary>
        /// 进度条，String是GUID
        /// </summary>
        public Dictionary<string, GameSaveData> dataDict = new Dictionary<string, GameSaveData>();

        //用来UI显示进度详情
        public string DataTime
        {
            get
            {
                var key = string.Empty;// TimeManagerSystem.Instance.GUID;

                if (dataDict.ContainsKey(key))
                {
                    var timeData = dataDict[key];
                    return timeData.timeDict["gameYear"] + "年/" + (ESeason)timeData.timeDict["gameSeason"] + "/" + timeData.timeDict["gameMonth"] + "月/" + timeData.timeDict["gameDay"] + "日/";
                }
                else return string.Empty;
            }
        }

        public string DataScene
        {
            get
            {
                var key = string.Empty;// TimeManagerSystem.Instance.GUID;
                //var key = SceneTransitionSystem.Instance.GUID;
                if (dataDict.ContainsKey(key))
                {
                    var transitionData = dataDict[key];
                    return transitionData.dataSceneName switch
                    {
                        "00.Start" => "海边",
                        "01.Field" => "农场",
                        "02.Home" => "小木屋",
                        "03.Stall" => "市场",
                        "04.Path" => "小径",
                        "05.House01" => "Trace的家",
                        _ => string.Empty
                    };
                }
                else return string.Empty;
            }
        }
    }
}
