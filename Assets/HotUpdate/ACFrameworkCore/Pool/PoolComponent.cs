using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YooAsset;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    对象池模块

-----------------------*/


namespace ACFrameworkCore
{

    public class PoolManager : SingletonInit<PoolManager>,ICore
    {
        public void ICroeInit()
        {
            poolDic = new Dictionary<string, PoolData>();
        }

        public Dictionary<string, PoolData> poolDic { get; private set; }
        private GameObject poolObj { get; set; }

        /// <summary>
        /// 往外拿东西
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void GetObj(string name, UnityAction<GameObject> callBack)
        {
            //有抽屉 并且抽屉里有东西
            if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
            {
                callBack?.Invoke(poolDic[name].GetObj());
            }
            else
            {
                AssetOperationHandle handle = YooAssetLoadExpsion.YooaddetLoadSyncAOH(name);
                handle.Completed += handleTemp => 
                {
                    GameObject go = handleTemp.InstantiateSync();
                    go.name = name;
                    callBack?.Invoke(go);
                };
            }
        }

        /// <summary>
        /// 换暂时不用的东西给我
        /// </summary>
        public void PushObj(string name, GameObject obj)
        {
            if (poolObj == null) poolObj = new GameObject("Pool");
            if (poolDic.ContainsKey(name))//里面有抽屉
                poolDic[name].PushObj(obj);
            else//里面没有抽屉
                poolDic.Add(name, new PoolData(obj, poolObj));
        }

        /// <summary>
        /// 清空缓存池的方法 
        /// 主要用在 场景切换时
        /// </summary>
        public void Clear()
        {
            foreach (var Key in poolDic.Keys)
                poolDic[Key].poolList.ForEach((go) => { GameObject.Destroy(go); });
            poolDic.Clear();
            poolObj = null;
        }

        
    }
}
