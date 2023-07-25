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
        public HashSet<AssetOperationHandle> assetOperationHandles = new HashSet<AssetOperationHandle>();

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
            MonoComponent.Instance.MonoStartCoroutine(ResLoadAsync(SceneName, callback));
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


        public void LoadAll(string path)
        {
        }
        public void LoadAllAssets<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
        {
        }
        public void LoadAllAssetsAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
        {
        }

        public void LoadAsync(string path)
        {
        }
        public void LoadAsync<T>(string path) where T : UnityEngine.Object
        {
        }

        public void LoadRawFile<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
        {
        }
        public void LoadRawFileAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
        {
        }

        public T LoadSubAssets<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
        {
            return null;
        }
        public void LoadSubAssetsAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
        {

        }

        #region 资源卸载

        public void UnloadAssetsIEnumerator<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {

            //assetOperationHandles.TryGetValue()

            //AssetOperationHandle handle = assetOperationHandles
            //handle.Release();
        }

        //IEnumerator UnloadAssets<T>(string ResName, UnityAction<T> callback)
        //{
        //    var package = GetPakckage();
        //    AssetOperationHandle handle = package.LoadAssetAsync<T>(ResName);
        //    handle.na
        //    yield return handle;
        //    callback?.Invoke(handle as T);

        //}

        #endregion

    }
}
