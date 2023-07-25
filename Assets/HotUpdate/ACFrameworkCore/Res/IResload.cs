/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    资源加载接口

-----------------------*/

using System;
using UnityEngine;
using UnityEngine.Events;
using YooAsset;

namespace ACFrameworkCore
{
    public interface IResload
    {
        /// <summary>
        /// 同步加载资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string ResName) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载资源对象(协程加载)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void LoadAssetAsyncIEnumerator<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载资源对象(委托加载)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ResName"></param>
        /// <param name="callback"></param>
        public void LoadAssetAsyncDelegate<T>(string ResName, Action<T> callback) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载资源对象(Task加载)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ResName"></param>
        /// <param name="callback"></param>
        public void LoadAssetAsyncTask<T>(string ResName, Action<T> callback) where T : UnityEngine.Object;

        /// <summary>
        /// 同步加载子资源对象
        /// 例如：通过TexturePacker创建的图集，如果需要访问图集的精灵对象，可以通过子对象加载接口。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public T LoadSubAssets<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载子资源对象
        /// 例如：通过TexturePacker创建的图集，如果需要访问图集的精灵对象，可以通过子对象加载接口。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void LoadSubAssetsAsync<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object;

        /// <summary>
        /// 同步加载资源包内所有资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void LoadAllAssets<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载资源包内所有资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void LoadAllAssetsAsync<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object;

        /// <summary>
        /// 同步获取原生文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void LoadRawFile<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object;

        /// <summary>
        /// 异步获取原生文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void LoadRawFileAsync<T>(string ResName, UnityAction<T> callback) where T : UnityEngine.Object;


        /// <summary>
        /// 资源卸载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ResName">资源的名称</param>
        /// <param name="callback">资源卸载后执行的方法</param>
        public void UnloadAssetsIEnumerator<T>(string ResName, UnityAction<T> callback = null) where T : UnityEngine.Object;

        /// <summary>
        /// 加载全部
        /// </summary>
        public void LoadAll(string ResName);

        /// <summary>
        /// 异步加载
        /// </summary>
        public void LoadAsync(string paResNameth);

        public void LoadAsync<T>(string ResName) where T : UnityEngine.Object;

    }
}
