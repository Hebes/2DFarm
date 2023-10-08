using UnityEngine.Events;
using YooAsset;
using Cysharp.Threading.Tasks;
using static UnityEditor.FilePathAttribute;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    资源加载

-----------------------*/

namespace Core
{
    public enum ELoadType
    {
        ReResources,
        YooAsset,
    }

    public class CoreResource : ICore
    {
        public static CoreResource Instance;
        private IResLoad iload;

        public void ICroeInit()
        {
            Instance = this;
            iload = new YooAssetResLoad();
        }

        public T Load<T>(string ResName) where T : UnityEngine.Object
        {
            return iload.Load<T>(ResName);
        }
        public UniTask<T> LoadAsync<T>(string assetName) where T : UnityEngine.Object
        {
            return iload.LoadAsync<T>(assetName);
        }

        public T LoadSub<T>(string location, string ResName) where T : UnityEngine.Object
        {
            return iload.LoadSub<T>(location, ResName);
        }

        public void LoadAll<T>(string ResName) where T : UnityEngine.Object
        {
            iload.LoadAll<T>(ResName);
        }

        public void UnloadAssets()
        {
            iload.UnloadAssets();
        }
    }
}
