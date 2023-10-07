using Cysharp.Threading.Tasks;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    加载拓展类

-----------------------*/


namespace Core
{
    public static class LoadResExtension
    {
        public static T Load<T>(this string assetName) where T : UnityEngine.Object
        {
            return CoreResource.Instance.Load<T>(assetName);
        }
        public static UniTask<T> LoadAsync<T>(this string assetName) where T : UnityEngine.Object
        {
            return CoreResource.Instance.LoadAsyncUniTack<T>(assetName);
        }
        public static T LoadSub<T>(string location, string ResName) where T : UnityEngine.Object
        {
            return CoreResource.Instance.LoadSub<T>(location, ResName);
        }

        //资源释放
        public static void UnloadAssets()
        {
            CoreResource.Instance.UnloadAssets();
        }


        //特殊封装
        public static T LoadOrSub<T>(string assetName, string ResName) where T : UnityEngine.Object
        {
            T t = null;
            if (assetName.Equals("null"))
                t = Load<T>(ResName);
            else
                t = LoadSub<T>(assetName, ResName);
            if (t == null)
                Debug.Error($"没有资源请检查yooasset{ResName}");
            return t;
        }
    }
}
