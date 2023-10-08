using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Debug = Core.Debug;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    Unity加载

-----------------------*/

namespace Farm2D
{
    public class UnityResLoad : IResLoad
    {
        public T Load<T>(string AssetName) where T : UnityEngine.Object
        {
            T t = Resources.Load<T>(AssetName);
            if (t == null)
                Debug.Error($"资源为空{AssetName}");
            return t;
        }

        public T[] LoadAll<T>(string AssetName) where T : UnityEngine.Object
        {
            T[] values = Resources.LoadAll<T>(AssetName);
            if (values == null && values.Length <= 0)
                Debug.Error($"资源为空{AssetName}");
            return values;
        }

        public UniTask<T[]> LoadAllAsync<T>(string location) where T : UnityEngine.Object
        {
            return default;
        }

        public async UniTask<T> LoadAsync<T>(string AssetName) where T : UnityEngine.Object
        {
            ResourceRequest t = Resources.LoadAsync<T>(AssetName);
            await t.ToUniTask();
            if (t.isDone == false)
                Debug.Error($"资源为空{AssetName}");
            return t.asset as T;
        }

        public T LoadSub<T>(string location, string AssetName) where T : UnityEngine.Object
        {
            return null;
        }

        public UniTask<T> LoadSubAsync<T>(string location, string AssetName) where T : UnityEngine.Object
        {
            return default;
        }

        public void ReleaseAsset(string AssetName = null)
        {
        }

        public void UnloadAssets()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}
