/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    加载拓展类

-----------------------*/

using Cysharp.Threading.Tasks;

namespace ACFrameworkCore
{
    public static class ResourceExtension
    {
        //同步加载
        public static T Load<T>(this string assetName) where T : UnityEngine.Object
        {
            return ResourceManager.Instance.Load<T>(assetName);
        }

        //异步加载
        public static UniTask<T> LoadAsyncUniTask<T>(this string assetName) where T : UnityEngine.Object
        {
            return ResourceManager.Instance.LoadAsyncUniTack<T>(assetName);
        }

        //资源释放
        public static void UnloadAssets()
        {
            ResourceManager.Instance.UnloadAssets();
        }
    }
}
