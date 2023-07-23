using System.Collections;
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
#if UNITY_EDITOR
        //public readonly string packageName = "PC";
#endif
        public readonly string packageName = "PC";


        public T LoadAsset<T>(string ResName) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(packageName);
            AssetOperationHandle handle = package.LoadAssetSync<T>(ResName);
            return handle.AssetObject as T;
        }

        public void LoadAssetAsync<T>(string SceneName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            MonoComponent.Instance.MonoStartCoroutine(ResLoadAsync(SceneName, callback));
        }

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

        IEnumerator ResLoadAsync<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(packageName);
            AssetOperationHandle handle = package.LoadAssetAsync<T>(ResName);
            yield return handle;
            callback?.Invoke(handle.AssetObject as T);

            GameObject go = handle.InstantiateSync();
            Debug.Log($"Prefab name is {go.name}");
        }
    }
}
