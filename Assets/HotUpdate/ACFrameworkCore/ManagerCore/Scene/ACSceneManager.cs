using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ACFrameworkCore
{
    public class ACSceneManager : ICore
    {
        public static ACSceneManager Instance;
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

        public async UniTask<SceneOperationHandle> LoadSceneAsync(string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool suspendLoad = false, int priority = 100)
        {
            return await sceneLoad.LoadSceneAsync(SceneName, loadSceneMode, suspendLoad, priority);
        }
        public void SetActivateScene(string scnenName)
        {
            Dictionary<string, SceneOperationHandle> ttt = sceneLoad.GetManagerDic() as Dictionary<string, SceneOperationHandle>;
            ttt.TryGetValue(scnenName, out SceneOperationHandle result);
            result.ActivateScene();
        }

        public void UnloadAsync(string scnenName)
        {
            Dictionary<string, SceneOperationHandle> ttt = sceneLoad.GetManagerDic() as Dictionary<string, SceneOperationHandle>;
            ttt.TryGetValue(scnenName, out SceneOperationHandle result);
            UnloadSceneOperation operation = result.UnloadAsync();
            ttt.Remove(scnenName);
        }

        public async UniTask<SceneOperationHandle> ChangeScene(string oldScene, string newScene, LoadSceneMode loadSceneMode)
        {
            UnloadAsync(oldScene);
            return await LoadSceneAsync(newScene, loadSceneMode, false, 100);
        }
    }
}
