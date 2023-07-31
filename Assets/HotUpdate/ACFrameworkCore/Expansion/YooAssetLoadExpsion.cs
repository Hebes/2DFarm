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

namespace ACFrameworkCore
{
    public static class YooAssetLoadExpsion
    {
        /// <summary>
        /// 异步加载资源拓展方法
        /// </summary>
        /// <param name="GOName"></param>
        public static void YooaddetLoadAsync(this string GOName, Action<AssetOperationHandle> action = null)
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage(Config.YooAseetPackage);
            AssetOperationHandle handle1 = package.LoadAssetAsync<GameObject>(GOName);
            handle1.Completed += obj => { action?.Invoke(obj); };
        }

        /// <summary>
        /// 同步加载资源拓展方法
        /// </summary>
        /// <param name="GOName"></param>
        /// <returns></returns>
        public static GameObject YooaddetLoadSync(this string GOName)
        {
            //TODO 后续要从配置中读取 或者直接配置
            var package = YooAssets.GetPackage("PC");
            AssetOperationHandle handle1 = package.LoadAssetSync<GameObject>(GOName);
            return (GameObject)handle1.AssetObject;
            //handle1.Completed += obj => { obj.InstantiateSync(); };
        }
    }
}
