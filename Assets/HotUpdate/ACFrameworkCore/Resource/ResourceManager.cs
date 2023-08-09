﻿using System;
using UnityEngine.Events;
using YooAsset;

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

        //同步加载
        public T LoadAsset<T>(string ResName) where T : UnityEngine.Object
        {
            return iload.LoadAsset<T>(ResName);
        }
        
        //异步加载
        public AssetOperationHandle LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object
        {
           return iload.LoadAssetAsync<T>(assetName);
        }
        public T LoadAssetAsyncAsT<T>(string assetName) where T : UnityEngine.Object
        {
            return iload.LoadAssetAsyncAsT<T>(assetName);
        }


        /// <summary>
        /// 同步加载子资源对象
        /// 例如：通过TexturePacker创建的图集，如果需要访问图集的精灵对象，可以通过子对象加载接口。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public T LoadSubAssets<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            return null;
        }

        /// <summary>
        /// 异步加载子资源对象
        /// 例如：通过TexturePacker创建的图集，如果需要访问图集的精灵对象，可以通过子对象加载接口。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void LoadSubAssetsAsync<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {

        }

        /// <summary>
        /// 同步加载资源包内所有资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void LoadAllAssets<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {

        }

        /// <summary>
        /// 异步加载资源包内所有资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void LoadAllAssetsAsync<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {

        }

        /// <summary>
        /// 同步获取原生文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public RawFileOperationHandle LoadRawFile<T>(string ResName) where T : class
        {
            return iload.LoadRawFile<T>(ResName);
        }

        /// <summary>
        /// 异步获取原生文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void LoadRawFileAsync<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object
        {

        }
    }
}