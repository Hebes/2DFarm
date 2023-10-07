using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using YooAsset;

namespace Core
{
    public class YooAssetLoadScene : ISceneLoad
    {
        public const string LoadingEvenName = "进度条更新";

        public Dictionary<string, SceneOperationHandle> sceneSceneOperationHandleDic = new Dictionary<string, SceneOperationHandle>();

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="SceneName">场景名称</param>
        /// <param name="loadSceneMode">场景加载模式</param>
        /// <param name="suspendLoad">场景加载到90%自动挂起</param>
        /// <param name="priority">优先级</param>
        /// <returns></returns>
        public async UniTask<SceneOperationHandle> LoadSceneAsync(string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            bool suspendLoad = false, int priority = 100)
        {
            var package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
            SceneOperationHandle handle = package.LoadSceneAsync(SceneName, loadSceneMode, suspendLoad);
            await handle.ToUniTask(Progress.Create<float>((progress) =>
            {
                LoadingEvenName.EventTrigger(handle.SceneObject.name, handle.Progress);//触发事件
            }));
            //while (!handle.IsDone)
            //{
            //    LoadingEvenName.EventTrigger(handle.Progress);//触发事件
            //    await UniTask.Yield();
            //}
            if (handle.Status == EOperationStatus.Succeed)
            {
                if (sceneSceneOperationHandleDic.ContainsKey(SceneName))
                {
                    //await sceneSceneOperationHandleDic[SceneName].UnloadAsync();
                    sceneSceneOperationHandleDic[SceneName] = handle;
                }
                else
                {
                    sceneSceneOperationHandleDic.Add(SceneName, handle);
                }
                package.UnloadUnusedAssets();
                Debug.Log($"加载场景成功:{handle.SceneObject.name}");
            }
            return handle;
        }

        public object GetManagerDic()
        {
            return sceneSceneOperationHandleDic;
        }
    }
}
