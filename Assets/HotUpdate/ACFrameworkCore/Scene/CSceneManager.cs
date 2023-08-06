using UnityEngine.Events;

namespace ACFrameworkCore
{
    public class SceneManager : SingletonInit<SceneManager>, ICore
    {
        private ISceneLoad sceneLoad;
        public void ICroeInit()
        {
            sceneLoad = new YooAssetLoadScene();
        }

        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="SceneName"></param>
        public void LoadScene(string SceneName)
        {
            sceneLoad.LoadScene(SceneName);
        }

        public void LoadSceneCommon(string SceneName, UnityAction unityAction = null)
        {
            sceneLoad.LoadSceneCommon(SceneName, unityAction);
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="SceneName">场景名称</param>
        /// <param name="unityAction">加载完毕后的回调</param>
        public void LoadSceneAsyn(string SceneName, UnityAction unityAction = null)
        {
            sceneLoad.LoadSceneAsync(SceneName, unityAction);
        }
    }
}
