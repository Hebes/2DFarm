using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ACFrameworkCore
{
    public class YooAssetLoadScene : ISceneLoad
    {
        public readonly string packageName = "PC";
        public const string LoadingEvenName = "进度条更新";

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="SceneName">场景名称</param>
        /// <param name="action"></param>
        /// <param name="loadSceneMode">场景加载模式</param>
        /// <param name="suspendLoad">场景加载到90%自动挂起</param>
        /// <param name="priority">优先级</param>
        /// <returns></returns>
        public async UniTask LoadSceneAsync(string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            Action<SceneOperationHandle> action = null, bool suspendLoad = false, int priority = 100)
        {
            //这里
            var package = YooAssets.GetPackage(packageName);
            SceneOperationHandle handle = package.LoadSceneAsync(SceneName, loadSceneMode, suspendLoad);
            while (!handle.IsDone)
            {
                LoadingEvenName.EventTrigger(handle.Progress);//触发事件
                await UniTask.Yield();
            }
            action?.Invoke(handle);
            // 释放资源
            //package.UnloadUnusedAssets();
        }
    }
}
