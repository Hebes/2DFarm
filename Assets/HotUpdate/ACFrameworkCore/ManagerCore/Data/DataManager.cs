using System.Collections.Generic;
using UnityEngine;
using YooAsset;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据加载管理

-----------------------*/

namespace ACFrameworkCore
{
    public class DataManager : ICore
    {
        public static DataManager Instance;
        private Dictionary<string, List<IData>> bytesDataDic;//数据

        public void ICroeInit()
        {
            Instance = this;
            bytesDataDic = new Dictionary<string, List<IData>>();

            //加载数据
            InitData<ItemDetailsData>();                //物品数据
            InitData<PlayerAnimatorsData>();            //玩家动画数据
            InitData<ScheduleDetailsData>();            //计划详细信息
            InitData<SceneRouteDetailsData>();          //场景路径数据
            InitData<DialogueDetailsData>();            //对话数据
            InitData<ShopDetailsData>();                //商店数据
            InitData<BluePrintDetailsData>();           //建造数据
            InitData<LightDetailsData>();               //灯光数据
            InitData<SceneSoundItemDetailsData>();      //场景音乐数据
            InitData<SoundDetailsData>();               //音乐数据


            //ACDebug.Log($"一共{bytesDataDic.Count}条数据");
            Debug.Log("数据初始化完毕");
            GameObject gameObject = new GameObject("DataManager");
            ShowDataManager showDataManager = gameObject.AddComponent<ShowDataManager>();
            GameObject.DontDestroyOnLoad(gameObject);


            ItemDetailsData itemDetailsData = GetDataOne<ItemDetailsData>(1001);
            ACDebug.Log($"获取的数据是{itemDetailsData.name}数据");

            //TODO 这里需要
            //showDataManager.ItemDetailsDataList = GetDataListAsT<ItemDetailsData>();
            //showDataManager.PlayerAnimatorsDataList = this.GetDataListThis<PlayerAnimatorsData>();
            //showDataManager.ScheduleDetailsDataList = this.GetDataListThis<ScheduleDetailsData>();
            //showDataManager.SceneRouteDetailsDataList = this.GetDataListThis<SceneRouteDetailsData>();
            //showDataManager.DialogueDetailsDataList = this.GetDataListThis<DialogueDetailsData>();
            //showDataManager.ShopDetailsDataList = this.GetDataListThis<ShopDetailsData>();
            //showDataManager.BluePrintDetailsDataList = this.GetDataListThis<BluePrintDetailsData>();
            //showDataManager.LightDetailsDataList = this.GetDataListThis<LightDetailsData>();
            //showDataManager.SceneSoundItemDetailsDataList = this.GetDataListThis<SceneSoundItemDetailsData>();
            //showDataManager.SoundDetailsDataList = this.GetDataListThis<SoundDetailsData>();
        }

        //初始化数据
        public void InitData<T>(string fileName) where T : IData
        {
            RawFileOperationHandle handle = YooAssetLoadExpsion.YooaddetLoadRawFileAsync(fileName);
            byte[] fileData = handle.GetRawFileData();
            List<IData> itemDetailsList = BinaryAnalysis.GetData<T>(fileData);
            if (bytesDataDic.ContainsKey(typeof(T).FullName))
                bytesDataDic[typeof(T).FullName] = itemDetailsList;
            bytesDataDic.Add(typeof(T).FullName, itemDetailsList);
        }
        public void InitData<T>() where T : IData
        {
            RawFileOperationHandle handle = YooAssetLoadExpsion.YooaddetLoadRawFileAsync(typeof(T).FullName);
            byte[] fileData = handle.GetRawFileData();
            List<IData> itemDetailsList = BinaryAnalysis.GetData<T>(fileData);
            if (bytesDataDic.ContainsKey(typeof(T).FullName))
                bytesDataDic[typeof(T).FullName] = itemDetailsList;
            bytesDataDic.Add(typeof(T).FullName, itemDetailsList);
        }


        //获取数据
        public T GetDataOne<T>(int id) where T : class, IData
        {
            if (!bytesDataDic.ContainsKey(typeof(T).FullName))
            {
                ACDebug.Log($"未能找到数据请先初始化{typeof(T).FullName}");
                return null;
            }

            IData data = bytesDataDic[typeof(T).FullName].Find(data => { return data.GetId() == id; });
            return data == null ? null : data as T;
        }
        public List<IData> GetDataList<T>() where T : class, IData
        {
            if (!bytesDataDic.ContainsKey(typeof(T).FullName))
            {
                ACDebug.Log($"未能找到数据请先初始化{typeof(T).FullName}");
                return null;
            }

            List<IData> dataListTemp = bytesDataDic[typeof(T).FullName];
            return dataListTemp;
        }

        public List<T> GetDataListAsT<T>() where T : class, IData
        {
            List<T> dataListTemp = new List<T>();
            if (!bytesDataDic.ContainsKey(typeof(T).FullName))
            {
                ACDebug.Log($"未能找到数据请先初始化{typeof(T).FullName}");
                return null;
            }
            foreach (IData item in bytesDataDic[typeof(T).FullName])
                dataListTemp.Add(item as T);
            return dataListTemp;
        }
    }
}
