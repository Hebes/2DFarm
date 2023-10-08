using Farm2D;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
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

namespace Core
{
    public class CoreData : ICore
    {
        public static CoreData Instance;
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


            Debug.Log("数据初始化完毕");
            GameObject gameObject = new GameObject("DataManager");
            ShowDataManager showDataManager = gameObject.AddComponent<ShowDataManager>();
            GameObject.DontDestroyOnLoad(gameObject);

            //临时查看的
            showDataManager.ItemDetailsDataList = GetDataList<ItemDetailsData>();
            showDataManager.PlayerAnimatorsDataList = GetDataList<PlayerAnimatorsData>();
            showDataManager.ScheduleDetailsDataList = GetDataList<ScheduleDetailsData>();
            showDataManager.SceneRouteDetailsDataList = GetDataList<SceneRouteDetailsData>();
            showDataManager.DialogueDetailsDataList = GetDataList<DialogueDetailsData>();
            showDataManager.ShopDetailsDataList = GetDataList<ShopDetailsData>();
            showDataManager.BluePrintDetailsDataList = GetDataList<BluePrintDetailsData>();
            showDataManager.LightDetailsDataList = GetDataList<LightDetailsData>();
            showDataManager.SceneSoundItemDetailsDataList = GetDataList<SceneSoundItemDetailsData>();
            showDataManager.SoundDetailsDataList = GetDataList<SoundDetailsData>();
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
            IData data = null;
            if (bytesDataDic.TryGetValue(typeof(T).FullName, out List<IData> value))
                data = value.Find(data => { return data.GetId() == id; });
            return data as T;
        }


        public List<T> GetDataList<T>() where T : class, IData
        {
            List<T> dataListTemp = new List<T>();
            if (bytesDataDic.TryGetValue(typeof(T).FullName, out List<IData> value))
            {
                foreach (IData item in value)
                    dataListTemp.Add(item as T);
            }
            return dataListTemp;
        }
    }
}
