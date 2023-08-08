/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    加载拓展类

-----------------------*/

using YooAsset;

namespace ACFrameworkCore
{
    public static class ResourceExtension
    {

        public static void Load<T>(this string path) where T : UnityEngine.Object
        {
            //ResComponent.Insatance.OnLoad<T>(path);

        }

        //public static void OnLoadAll(string path)
        //{
        //    ResComponent.Insatance.OnLoadAll(path);
        //}

        public static void LoadAsync(string path)
        {
            //ResComponent.Insatance.OnLoadAsync(path);
        }

        //异步加载
        public static AssetOperationHandle LoadAsync<T>(string assetName) where T : UnityEngine.Object
        {
            return ResourceManager.Instance.LoadAssetAsync<T>(assetName);
        }
        public static T LoadAsyncAsT<T>(string assetName) where T : UnityEngine.Object
        {
           return ResourceManager.Instance.LoadAssetAsyncAsT<T>(assetName);
        }
    }
}
