using Cysharp.Threading.Tasks;
using System.Collections.Generic;
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

namespace Core
{
    public class YooAssetResLoad : IResLoad
    {
        public HashSet<AssetHandle> assetHashSet = new HashSet<AssetHandle>();

        public T Load<T>(string AssetName) where T : UnityEngine.Object
        {
            ResourcePackage package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetHandle handle = package.LoadAssetSync<T>(AssetName);
            if (handle.Status == EOperationStatus.Succeed)
                return handle.AssetObject as T;
            Debug.Error($"资源加载失败,请检查资源名称:{AssetName}");
            return null;
        }
        public async UniTask<T> LoadAsync<T>(string AssetName) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetHandle handle = package.LoadAssetAsync<T>(AssetName);
            assetHashSet.Add(handle);
            await handle.ToUniTask();
            if (handle.Status == EOperationStatus.Succeed)
                return handle.AssetObject as T;
            Debug.Error($"资源加载失败,请检查资源名称:{AssetName}");
            return null;
        }

        public T[] LoadAll<T>(string AssetName) where T : UnityEngine.Object
        {
            ResourcePackage package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AllAssetsHandle handle = package.LoadAllAssetsAsync<T>(AssetName);
            return handle.AllAssetObjects as T[];
        }


        public async UniTask<T[]> LoadAllAsync<T>(string location) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AllAssetsHandle handle = package.LoadAllAssetsAsync<T>(location);
            await handle.ToUniTask();
            return handle.AllAssetObjects as T[];
        }

        public T LoadSub<T>(string location, string ResName) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            SubAssetsHandle handle = package.LoadSubAssetsSync<T>(location);
            var sprite = handle.GetSubAssetObject<T>(ResName);
            return sprite;
        }

        public async UniTask<T> LoadSubAsync<T>(string location, string AssetName) where T : UnityEngine.Object
        {
            //var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            //SubAssetsOperationHandle handle = package.LoadSubAssetsAsync<T>(location);
            //await handle.ToUniTask();
            //var sprite = handle.GetSubAssetObject<T>(ResName);
            await UniTask.Yield();
            return null;
        }
        
        public T LoadAssetAsyncAsT<T>(string assetName) where T : UnityEngine.Object
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            AssetHandle handle = package.LoadAssetAsync<T>(assetName);
            handle.WaitForAsyncComplete();
            if (handle.Status == EOperationStatus.Succeed)
            {
                return handle as T;
            }
            else
            {
                Debug.Error($"没有加载到预制体!{assetName}");
                return null;
            }
        }

        //原生文件加载
        public RawFileHandle LoadRawFile<T>(string location) where T : class
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            RawFileHandle handle = package.LoadRawFileSync(location);
            if (handle.Status == EOperationStatus.Succeed)
                return handle;
            else
                Debug.Error($"加载原生文件失败,请检查路径{location}");
            return handle;
            //byte[] fileData = handle.GetRawFileData();
            //string fileText = handle.GetRawFileText();
            //string filePath = handle.GetRawFilePath();
        }

        //public async UniTask<RawFileOperationHandle> LoadRawFileAsync<T>(string location) where T : UnityEngine.Object
        //{
        //    var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
        //    RawFileOperationHandle handle = package.LoadRawFileAsync(location);
        //    await handle.ToUniTask();
        //    if (handle.Status == EOperationStatus.Succeed)
        //        return handle;
        //    else
        //        Debug.Error($"加载原生文件失败,请检查路径{location}");
        //    return handle;
        //    //byte[] fileData = handle.GetRawFileData();
        //    //string fileText = handle.GetRawFileText();
        //    //string filePath = handle.GetRawFilePath();
        //}


        //资源卸载和释放
        public void ReleaseAsset(string ResName = null)
        {
            foreach (var item in assetHashSet)
            {
                item.Release();
            }
            //AssetOperationHandle assetTemp = assetHashSet.First((go) => { return go.AssetObject.name == ResName; });
            //if (assetTemp != null) { ACDebug.Error($"没有找到{ResName}的资源!"); }
            //assetTemp.Release();
        }

        //资源释放
        public void UnloadAssets()
        {
            ReleaseAsset();
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            package.UnloadUnusedAssetsAsync();
        }
    }
}
