using UnityEngine.Events;
using YooAsset;
using Cysharp.Threading.Tasks;
using System;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    资源加载

-----------------------*/

namespace ACFrameworkCore
{
    /// <summary>
    /// 加载资源的方式
    /// </summary>
    public enum ELoadType
    {
        ReResources,
        YooAsset,
    }

    public class ResourceManager : ICore
    {
        public static ResourceManager Instance;
        private IResload iload;
        public void ICroeInit()
        {
            Instance = this;
            iload = new YooAssetResLoad();
        }

        //加载资源
        public T Load<T>(string ResName) where T : UnityEngine.Object
        {
            return iload.Load<T>(ResName);
        }
        public UniTask<T> LoadAsyncUniTack<T>(string assetName) where T : UnityEngine.Object
        {
            return iload.LoadAsyncUniTack<T>(assetName);
        }

        //加载子资源对象
        public T LoadSub<T>(string location, string ResName) where T : UnityEngine.Object
        {
            return iload.LoadSub<T>(location, ResName);
        }
        public void LoadSubAsync<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {

        }

        //加载所有资源
        public void LoadAll<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {

        }
        public void LoadAllAsync<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {

        }

        //加载原生文件
        public RawFileOperationHandle LoadRawFile<T>(string ResName) where T : class
        {
            return iload.LoadRawFile<T>(ResName);
        }
        public UniTask<RawFileOperationHandle> LoadRawFileAsync<T>(string ResName) where T : UnityEngine.Object
        {
            return iload.LoadRawFileAsync<T>(ResName);
        }

        public void UnloadAssets()
        {
            iload.UnloadAssets();
        }
    }
}
