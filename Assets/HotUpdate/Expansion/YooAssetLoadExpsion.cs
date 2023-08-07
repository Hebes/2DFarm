using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using YooAsset;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    YooAsset拓展方法

-----------------------*/

namespace ACFrameworkCore
{
    public static class YooAssetLoadExpsion
    {
        //异步加载资源拓展方法
        public static void YooaddetLoadAsync(this string GOName, Action<AssetOperationHandle> action = null)
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetOperationHandle handle = package.LoadAssetAsync<GameObject>(GOName);
            handle.Completed += obj => { action?.Invoke(obj); };
        }
        public static AssetOperationHandle YooaddetLoadAsync(this string GOName)
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            return package.LoadAssetAsync<GameObject>(GOName);
        }
        public static AssetOperationHandle YooaddetLoadAsync<T>(this string GOName) where T : UnityEngine.Object
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            return package.LoadAssetAsync<T>(GOName);
        }
        public static T YooaddetLoadAsyncObj<T>(this string GOName) where T : UnityEngine.Object
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetOperationHandle handle = package.LoadAssetAsync<T>(GOName);
            handle.WaitForAsyncComplete();
            return handle.AssetObject as T;
        }

        public static async UniTask<AssetOperationHandle> YooaddetLoadUniTaskAsync<T>(this string assetName) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetOperationHandle handle = package.LoadAssetAsync<T>(assetName);
            //handle.WaitForAsyncComplete();
            await handle.ToUniTask();
            if (handle.Status == EOperationStatus.Succeed)
            {
                return handle;
                //var obj = handle.InstantiateAsync();
                //await obj.ToUniTask();
                //if (obj.Result == null)
                //{
                //    Debug.Log("加载预制体为空");
                //}
            }
            else
            {
                Debug.Log("加载预制体为空");
                return null;
            }
        }

        //同步加载资源拓展方法
        public static GameObject YooaddetLoadSyncGO(this string GOName)
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetOperationHandle handle1 = package.LoadAssetSync<GameObject>(GOName);
            return (GameObject)handle1.AssetObject;
        }
        public static AssetOperationHandle YooaddetLoadSyncAOH(this string GOName)
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            return package.LoadAssetSync<GameObject>(GOName);
        }
    }
}
