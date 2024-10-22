using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using YooAsset;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    场景加载管理

-----------------------*/

namespace Core
{
    public class CoreScene : ICore
    {
        public static CoreScene Instance;
        private ISceneLoad sceneLoad;
        public void ICroeInit()
        {
            Instance = this;
            sceneLoad = new YooAssetLoadScene();
            //YooAssetLoadScene.LoadingEvenName.AddEventListener<string, float>((sceneName, progress) =>
            //{
            //    ACDebug.Log(sceneName + "当前的进度是:" + progress.ToString());
            //});
        }

        public async UniTask<SceneHandle> LoadSceneAsync(string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool suspendLoad = false, int priority = 100)
        {
            return await sceneLoad.LoadSceneAsync(SceneName, loadSceneMode, suspendLoad, priority);
        }
        public void SetActivateScene(string scnenName)
        {
            Dictionary<string, SceneHandle> ttt = sceneLoad.GetManagerDic() as Dictionary<string, SceneHandle>;
            ttt.TryGetValue(scnenName, out SceneHandle result);
            result.ActivateScene();
        }

        public void UnloadAsync(string scnenName)
        {
            Dictionary<string, SceneHandle> ttt = sceneLoad.GetManagerDic() as Dictionary<string, SceneHandle>;
            if (ttt.TryGetValue(scnenName, out SceneHandle result))
                result.UnloadAsync();
            ttt.Remove(scnenName);
        }

        public async UniTask<SceneHandle> ChangeScene(string oldScene, string newScene, LoadSceneMode loadSceneMode)
        {
            UnloadAsync(oldScene);
            return await LoadSceneAsync(newScene, loadSceneMode, false, 100);
        }
    }
}
