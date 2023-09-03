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
        public static T Load<T>(this string assetName) where T : UnityEngine.Object
        {
            return ResourceManager.Instance.Load<T>(assetName);
        }
        public static UniTask<T> LoadAsyncUniTask<T>(this string assetName) where T : UnityEngine.Object
        {
            return ResourceManager.Instance.LoadAsyncUniTack<T>(assetName);
        }
        public static T LoadSub<T>(string location, string ResName) where T : UnityEngine.Object
        {
            return ResourceManager.Instance.LoadSub<T>(location, ResName);
        }

        public static T LoadOrSub<T>(string assetName, string ResName = null) where T : UnityEngine.Object
        {
            if (ResName == null || ConfigExcelCommon.Null.Equals(assetName))
                return Load<T>(ResName);
            return LoadSub<T>(assetName, ResName);
        }

        //资源释放
        public static void UnloadAssets()
        {
            ResourceManager.Instance.UnloadAssets();
        }
    }
}
