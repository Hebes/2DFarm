using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using YooAsset;
using Cysharp.Threading.Tasks;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    yooAsset具体实现类
    https://www.yooasset.com/docs/guide-runtime/CodeTutorial3

-----------------------*/

namespace ACFrameworkCore
{
    public class YooAssetResLoad : IResload
    {
        public HashSet<AssetOperationHandle> assetHashSet = new HashSet<AssetOperationHandle>();

        //资源加载
        public T LoadAsset<T>(string ResName) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetOperationHandle handle = package.LoadAssetSync<T>(ResName);
            return handle.AssetObject as T;
        }
        public async UniTask<T> LoadAssetAsync<T>(string AssetName, Action<AssetOperationHandle> callback) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetOperationHandle handle = package.LoadAssetAsync<T>(AssetName);
            await handle.ToUniTask();
            callback?.Invoke(handle);
            return handle.AssetObject as T;
            //GameObject go = handle.InstantiateSync();//创建物体
        }

        //资源包内所有对象加载
        public T[] LoadAllAssets<T>(string location, UnityAction<T[]> callback) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AllAssetsOperationHandle handle = package.LoadAllAssetsAsync<T>(location);
            return handle.AllAssetObjects as T[];
        }
        public async UniTask<UnityEngine.Object[]> LoadAllAssetsAsync<T>(string location, UnityAction<UnityEngine.Object[]> callback) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AllAssetsOperationHandle handle = package.LoadAllAssetsAsync<T>(location);
            await handle.ToUniTask();
            callback?.Invoke(handle.AllAssetObjects as T[]);
            return handle.AllAssetObjects;
        }

        //原生文件加载
        public RawFileOperationHandle LoadRawFile<T>(string location) where T : class
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            RawFileOperationHandle handle = package.LoadRawFileSync(location);
            return handle;
            //byte[] fileData = handle.GetRawFileData();
            //string fileText = handle.GetRawFileText();
            //string filePath = handle.GetRawFilePath();
        }
        public async UniTask LoadRawFileAsync<T>(string location, UnityAction<RawFileOperationHandle> callback) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            RawFileOperationHandle handle = package.LoadRawFileAsync(location);
            await handle.ToUniTask();
            callback?.Invoke(handle);
            //byte[] fileData = handle.GetRawFileData();
            //string fileText = handle.GetRawFileText();
            //string filePath = handle.GetRawFilePath();
        }

        //子对象加载
        public T LoadSubAssets<T>(string location, string ResName) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            SubAssetsOperationHandle handle = package.LoadSubAssetsSync<T>(location);
            var sprite = handle.GetSubAssetObject<T>(ResName);
            return sprite;
        }
        public async UniTask LoadSubAssetsAsync<T>(string location, string ResName, UnityAction<T> callback = null) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            SubAssetsOperationHandle handle = package.LoadSubAssetsAsync<T>(location);
            await handle.ToUniTask();
            var sprite = handle.GetSubAssetObject<T>(ResName);
            callback?.Invoke(sprite);
        }

        //资源卸载和释放
        public void ReleaseAssetIEnumerator<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            AssetOperationHandle assetTemp = assetHashSet.First((go) => { return go.AssetObject.name == ResName; });
            if (assetTemp != null) { ACDebug.Error($"没有找到{ResName}的资源!"); }
            assetTemp.Release();
        }
        public void UnloadAssets()
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            package.UnloadUnusedAssets();
        }

        //通过资源标签来获取资源信息列表
        public AssetInfo[] GetAssetInfosByTag(string tag)
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetInfo[] assetInfos = package.GetAssetInfos(tag);
            return assetInfos;
        }

        #region 配置文件加载范例
        // 自定义的配置文件
        //        public class MyGameConfig : ScriptableObject
        //        {
        //           ...
        //        }

        //        IEnumerator Start()
        //        {
        //            string location = "Assets/GameRes/config/gameConfig.asset";
        //            AssetOperationHandle handle = package.LoadAssetFileAsync(location);
        //            yield return handle;
        //            MyGameConfig gameCOnfig = handle.AssetObject as MyGameConfig;
        //        }
        #endregion
    }
}
