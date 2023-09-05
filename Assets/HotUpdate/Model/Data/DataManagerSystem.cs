using ACFrameworkCore;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	牧场数据初始化管理系统

-----------------------*/

namespace ACFarm
{
    public class DataManagerSystem : ICore
    {
        public static DataManagerSystem Instance;
        private DataManager dataManager;

        private Dictionary<string, List<IData>> bytesDataDic;//数据

        public void ICroeInit()
        {
            Instance = this;
            dataManager = DataManager.Instance;

            bytesDataDic = new Dictionary<string, List<IData>>();
            //加载数据
            InitData<ItemDetailsData>(ConfigConfigData.ItemDetailsData);
            InitData<PlayerAnimatorsData>(ConfigConfigData.PlayerAnimatorsData);
            InitData<ScheduleDetailsData>(ConfigConfigData.ScheduleDetailsData);
            InitData<SceneRouteDetailsData>(ConfigConfigData.SceneRouteDetailsData);
            InitData<DialogueDetailsData>();
            InitData<ShopDetailsData>();
            InitData<BluePrintDetailsData>();
            InitData<LightDetailsData>();
            Debug.Log("数据初始化完毕");
            GameObject gameObject = new GameObject("DataManager");
            gameObject.AddComponent<ShowDataManager>();
            GameObject.DontDestroyOnLoad(gameObject);

            DataExpansion
        }


    }
}
