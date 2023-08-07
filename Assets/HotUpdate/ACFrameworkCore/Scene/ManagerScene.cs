using System;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ACFrameworkCore
{
    public class ManagerScene : SingletonInit<ManagerScene>, ICore
    {
        private ISceneLoad sceneLoad;
        public void ICroeInit()
        {
            sceneLoad = new YooAssetLoadScene();
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="SceneName">场景名称</param>
        /// <param name="unityAction">加载完毕后的回调</param>
        public void LoadSceneAsync(string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            Action<SceneOperationHandle> action = null, bool suspendLoad = false, int priority = 100)
        {
            sceneLoad.LoadSceneAsync(SceneName, loadSceneMode, action, suspendLoad, priority);
        }
    }
}
