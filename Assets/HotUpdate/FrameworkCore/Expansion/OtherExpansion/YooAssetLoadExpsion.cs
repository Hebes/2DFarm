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

namespace Core
{
    public static class YooAssetLoadExpsion
    {
        //异步加载资源拓展方法
        public static void YooaddetLoadAsync(this string assetName, Action<AssetOperationHandle> action = null)
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetOperationHandle handle = package.LoadAssetAsync<GameObject>(assetName);
            handle.Completed += obj => { action?.Invoke(obj); };
        }
        public static AssetOperationHandle YooaddetLoadAsync(this string assetName)
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            return package.LoadAssetAsync<GameObject>(assetName);
        }
        public static T YooaddetLoadAsyncAsT<T>(this string assetName) where T : UnityEngine.Object
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetOperationHandle handle = package.LoadAssetAsync<T>(assetName);
            handle.WaitForAsyncComplete();
            return handle.Status == EOperationStatus.Succeed ? handle.AssetObject as T : null;
        }

        public static async UniTask<T> YooaddetLoadAsyncUniTask<T>(this string assetName) where T : UnityEngine.Object
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetOperationHandle handle = package.LoadAssetAsync<T>(assetName);
            await handle.ToUniTask();
            return handle.Status == EOperationStatus.Succeed ? handle.AssetObject as T : null;
        }
        public static AssetOperationHandle YooaddetLoadAsync<T>(this string assetName) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetOperationHandle handle = package.LoadAssetAsync<T>(assetName);
            handle.WaitForAsyncComplete();
            return handle.Status == EOperationStatus.Succeed ? handle : null;
            //await UniTask.WaitUntilValueChanged(handle, x => handle.Status == EOperationStatus.Succeed);
        }

        //异步加载二进制文件
        public static RawFileOperationHandle YooaddetLoadRawFileAsync(this string fileName)
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            RawFileOperationHandle handle = package.LoadRawFileAsync(fileName);
            handle.WaitForAsyncComplete();
            return handle.Status == EOperationStatus.Succeed ? handle : null;
        }

        //同步加载资源拓展方法
        public static T YooaddetLoadSync<T>(this string GOName) where T : UnityEngine.Object
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetOperationHandle handle1 = package.LoadAssetSync<T>(GOName);
            return (T)handle1.AssetObject;
        }
        public static AssetOperationHandle YooaddetLoadSyncAOH(this string GOName)
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            return package.LoadAssetSync<GameObject>(GOName);
        }

        //子对象加载
        public static async UniTask<Sprite> LoadSubAssetsAsyncUniTask(this string assetName,string childAssetsName)
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            SubAssetsOperationHandle handle = package.LoadSubAssetsAsync<Sprite>(assetName);
            await handle.ToUniTask();
            Sprite sprite = handle.GetSubAssetObject<Sprite>(childAssetsName);
            return sprite != null ? sprite : null;
        }
    }
}
