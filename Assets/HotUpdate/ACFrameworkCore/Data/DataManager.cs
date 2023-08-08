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
        private Dictionary<string, List<IData>> bytesDic;
        private Dictionary<string, IData> tttDic;
        public void ICroeInit()
        {
            bytesDic = new Dictionary<string, List<IData>>();
            tttDic = new Dictionary<string, IData>();
            InitData();
        }


        private void InitData()
        {
            GetData<ItemDetails>(ConfigBytes.BytesItemDetails);
            GetData<TowerInfo>(ConfigBytes.BytesTowerInfo);
            Debug.Log("数据初始化完毕");
        }

        public void GetData<T>(string fileName) where T : IData
        {
            RawFileOperationHandle handle = YooAssetLoadExpsion.YooaddetLoadRawFileAsync(fileName);
            byte[] fileData = handle.GetRawFileData();
            List<T> itemDetailsList = BinaryAnalysis.LoopGetData<T>(fileData);
            foreach (var item in itemDetailsList)
            {
                if (bytesDic.ContainsKey(typeof(T).FullName))
                {
                    bytesDic[typeof(T).FullName].Add(item);
                }
                else
                {
                    bytesDic.Add(typeof(T).FullName, new List<IData> { item });
                }
            }
        }
    }
}
