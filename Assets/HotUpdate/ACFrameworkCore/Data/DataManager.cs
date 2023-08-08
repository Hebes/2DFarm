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
            //InitData<ItemDetails>(ConfigBytes.BytesItemDetails);
            //InitData<TowerInfo>(ConfigBytes.BytesTowerInfo);
            Debug.Log("数据初始化完毕");
        }

        public void InitData<T>(string fileName) where T : IData
        {
            RawFileOperationHandle handle = YooAssetLoadExpsion.YooaddetLoadRawFileAsync(fileName);
            byte[] fileData = handle.GetRawFileData();
            List<T> itemDetailsList = BinaryAnalysis.LoopGetData<T>(fileData);
            foreach (var item in itemDetailsList)
            {
                if (bytesDataDic.ContainsKey(typeof(T).FullName))
                    bytesDataDic[typeof(T).FullName].Add(item);
                else
                    bytesDataDic.Add(typeof(T).FullName, new List<IData> { item });
            }
        }


        public T GetDataOne<T>(int id) where T : class, IData
        {
            if (!bytesDataDic.ContainsKey(typeof(T).FullName)) return null;
            IData data = bytesDataDic[typeof(T).FullName].Find(data => { return data.GetId() == id; });
            return data == null ? null : data as T;
        }
        public List<T> GetDataList<T>() where T : class, IData
        {
            if (!bytesDataDic.ContainsKey(typeof(T).FullName)) return null;
            List<IData> dataListTemp = bytesDataDic[typeof(T).FullName];
           return  dataListTemp as List<T>;
        }
    }
}
