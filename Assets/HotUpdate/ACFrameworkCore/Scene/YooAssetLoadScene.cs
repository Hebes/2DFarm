using System.Collections;
using UnityEngine.Events;
using YooAsset;

namespace ACFrameworkCore
{
    public class YooAssetLoadScene : ISceneLoad
    {
        public readonly string packageName = "PC";
        public const string LoadingEvenName = "进度条更新";

        public void LoadScene(string SceneName)
        {
            //SceneManager.LoadScene(SceneName);
        }
        public void LoadSceneCommon(string SceneName, UnityAction unityAction)
        {
            //SceneManager.LoadScene(SceneName);
            MonoManager.Instance.StartCoroutine(ReallyLoadSceneCommon(SceneName, unityAction));
        }
        public void LoadSceneAsync(string SceneName, IEnumerator enumerator)
        {
            MonoManager.Instance.StartCoroutine(ReallyLoadSceneIEnumerator(SceneName, enumerator));
        }
        public void LoadSceneAsync(string SceneName, UnityAction unityAction)
        {
            MonoManager.Instance.StartCoroutine(ReallyLoadSceneAsynUnityAction(SceneName, unityAction));
        }
        public void LoadSceneIEnumerator(string SceneName, UnityAction unityAction)
        {
            MonoManager.Instance.StartCoroutine(ReallyLoadSceneAsynUnityAction(SceneName, unityAction));
        }

        IEnumerator ReallyLoadSceneAsynUnityAction(string SceneName, UnityAction unityAction)
        {
            //AsyncOperation ao = SceneManager.LoadSceneAsync(SceneName);
            //while (!ao.isDone)
            //{
            //    EventComponent.Instance.EventTrigger("进度条更新", ao.progress);
            //    yield return ao.progress;
            //}
            //unityAction();

            var package = YooAssets.GetPackage(packageName);
            var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
            bool suspendLoad = false;
            SceneOperationHandle handle = package.LoadSceneAsync(SceneName, sceneMode, suspendLoad);

            while (!handle.IsDone)
            {
                EventExpansion.EventTrigger(LoadingEvenName, handle.Progress);//触发事件
                yield return handle.Progress;
            }
            unityAction?.Invoke();
            // 释放资源
            package.UnloadUnusedAssets();
        }
        IEnumerator ReallyLoadSceneIEnumerator(string SceneName, IEnumerator enumerator)
        {
            var package = YooAssets.GetPackage(packageName);
            var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
            bool suspendLoad = false;
            SceneOperationHandle handle = package.LoadSceneAsync(SceneName, sceneMode, suspendLoad);

            while (!handle.IsDone)
            {
                EventExpansion.EventTrigger(LoadingEvenName, handle.Progress);//触发事件
                yield return handle.Progress;
            }
            yield return enumerator;

            // 释放资源
            package.UnloadUnusedAssets();
        }

        IEnumerator ReallyLoadSceneCommon(string SceneName, UnityAction unityAction)
        {
            var package = YooAssets.GetPackage(packageName);
            var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Additive;
            bool suspendLoad = false;
            SceneOperationHandle handle = package.LoadSceneAsync(SceneName, sceneMode, suspendLoad);

            while (!handle.IsDone)
            {
                EventExpansion.EventTrigger(LoadingEvenName, handle.Progress);//触发事件
                yield return handle.Progress;
            }
            unityAction?.Invoke();
            handle.ActivateScene();
            // 释放资源
            //package.UnloadUnusedAssets();
        }
    }
}
