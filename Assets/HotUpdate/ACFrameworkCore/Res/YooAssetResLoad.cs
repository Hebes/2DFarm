using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using YooAsset;

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


        public ResourcePackage GetPakckage()
        {
#if UNITY_EDITOR
            //return YooAssets.GetPackage(packageName);
#endif
            return YooAssets.GetPackage("PC");
        }

        #region 资源加载
        public T LoadAsset<T>(string ResName) where T : UnityEngine.Object
        {
            var package = GetPakckage();
            AssetOperationHandle handle = package.LoadAssetSync<T>(ResName);
            return handle.AssetObject as T;
        }
        public void LoadAssetAsyncIEnumerator<T>(string SceneName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            MonoManager.Instance.StartCoroutine(ResLoadAsync(SceneName, callback));
        }
        public void LoadAssetAsyncDelegate<T>(string ResName, System.Action<T> callback) where T : UnityEngine.Object
        {
            var package = GetPakckage();
            AssetOperationHandle handle = package.LoadAssetAsync<T>(ResName);
            handle.Completed += action => { callback?.Invoke(action.AssetObject as T); };
        }
        public async void LoadAssetAsyncTask<T>(string ResName, Action<T> callback) where T : UnityEngine.Object
        {
            var package = GetPakckage();
            AssetOperationHandle handle = package.LoadAssetAsync<T>(ResName);
            await handle.Task;
            callback?.Invoke(handle.AssetObject as T);
        }
        IEnumerator ResLoadAsync<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            var package = GetPakckage();
            AssetOperationHandle handle = package.LoadAssetAsync<T>(ResName);
            yield return handle;
            callback?.Invoke(handle.AssetObject as T);

            GameObject go = handle.InstantiateSync();
            Debug.Log($"Prefab name is {go.name}");
        }
        #endregion

        #region 资源包内所有对象加载
        public T[] LoadAllAssets<T>(string location, UnityAction<T[]> callback) where T : UnityEngine.Object
        {
            var package = GetPakckage();
            AllAssetsOperationHandle handle = package.LoadAllAssetsAsync<T>(location);
            return handle.AllAssetObjects as T[];
        }
        public void LoadAllAssetsAsyncIEnumerator<T>(string location, UnityAction<T[]> callback) where T : UnityEngine.Object
        {
            MonoManager.Instance.StartCoroutine(AllAssetsAsync(location, callback));
        }
        IEnumerator AllAssetsAsync<T>(string location, UnityAction<T[]> callback) where T : UnityEngine.Object
        {
            var package = GetPakckage();
            AllAssetsOperationHandle handle = package.LoadAllAssetsAsync<T>(location);
            yield return handle;
            callback?.Invoke(handle.AllAssetObjects as T[]);
        }
        #endregion

        #region 原生文件加载
        public RawFileOperationHandle LoadRawFile<T>(string location) where T : class
        {
            var package = GetPakckage();
            RawFileOperationHandle handle = package.LoadRawFileAsync(location);
            return handle;
            //byte[] fileData = handle.GetRawFileData();
            //string fileText = handle.GetRawFileText();
            //string filePath = handle.GetRawFilePath();
        }
        public void LoadRawFileAsync<T>(string location, UnityAction<RawFileOperationHandle> callback) where T : UnityEngine.Object
        {
            MonoManager.Instance.StartCoroutine(RawFileAsync(location, callback));
        }
        IEnumerator RawFileAsync(string location, UnityAction<RawFileOperationHandle> callback)
        {
            var package = GetPakckage();
            RawFileOperationHandle handle = package.LoadRawFileAsync(location);
            yield return handle;
            callback?.Invoke(handle);
            //byte[] fileData = handle.GetRawFileData();
            //string fileText = handle.GetRawFileText();
            //string filePath = handle.GetRawFilePath();
        }
        #endregion

        #region 子对象加载
        public T LoadSubAssets<T>(string location, string ResName) where T : UnityEngine.Object
        {
            var package = GetPakckage();
            SubAssetsOperationHandle handle = package.LoadSubAssetsSync<T>(location);
            var sprite = handle.GetSubAssetObject<T>(ResName);
            return sprite;
        }
        public void LoadSubAssetsAsyncIEnumerator<T>(string location, string ResName, UnityAction<T> callback = null) where T : UnityEngine.Object
        {
            MonoManager.Instance.StartCoroutine(SubAssetsAsync(location, ResName, callback));
        }
        IEnumerator SubAssetsAsync<T>(string location, string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            var package = GetPakckage();
            SubAssetsOperationHandle handle = package.LoadSubAssetsAsync<T>(location);
            yield return handle;
            var sprite = handle.GetSubAssetObject<T>(ResName);
            callback?.Invoke(sprite);
        }
        #endregion

        #region 资源卸载和释放
        public void ReleaseAssetIEnumerator<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            AssetOperationHandle assetTemp = assetHashSet.First((go) => { return go.AssetObject.name == ResName; });
            if (assetTemp != null) { DLog.Error($"没有找到{ResName}的资源!"); }
            assetTemp.Release();
        }
        public void UnloadAssets()
        {
            var package = GetPakckage();
            package.UnloadUnusedAssets();
        }
        #endregion

        #region 通过资源标签来获取资源信息列表
        /// <summary>
        /// 通过资源标签来获取资源信息列表
        /// </summary>
        /// <param name="tag">资源标签</param>
        /// <returns></returns>
        public AssetInfo[] GetAssetInfosByTag(string tag)
        {
            var package = GetPakckage();
            AssetInfo[] assetInfos = package.GetAssetInfos(tag);
            return assetInfos;
        }
        #endregion

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
