using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ACFrameworkCore
{
    public class YooAssetLoadScene : ISceneLoad
    {
        public readonly string packageName = "PC";

        public void LoadScene(string SceneName)
        {
            //SceneManager.LoadScene(SceneName);
        }

        public void LoadSceneAsync(string SceneName, IEnumerator enumerator)
        {
            MonoComponent.Instance.monoController.MonoStartCoroutine(ReallyLoadSceneIEnumerator(SceneName, enumerator));
        }
        public void LoadSceneAsync(string SceneName, UnityAction unityAction)
        {
            MonoComponent.Instance.MonoStartCoroutine(ReallyLoadSceneAsynUnityAction(SceneName, unityAction));
        }

        public void LoadSceneIEnumerator(string SceneName, UnityAction unityAction)
        {
            MonoComponent.Instance.MonoStartCoroutine(ReallyLoadSceneAsynUnityAction(SceneName, unityAction));
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
                EventComponent.Instance.EventTrigger("进度条更新", handle.Progress);
                yield return handle.Progress;
            }
            unityAction?.Invoke();
            // 释放资源
            package.UnloadUnusedAssets();
        }

        private IEnumerator ReallyLoadSceneIEnumerator(string SceneName, IEnumerator enumerator)
        {
            var package = YooAssets.GetPackage(packageName);
            var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
            bool suspendLoad = false;
            SceneOperationHandle handle = package.LoadSceneAsync(SceneName, sceneMode, suspendLoad);

            while (!handle.IsDone)
            {
                EventComponent.Instance.EventTrigger("进度条更新", handle.Progress);
                yield return handle.Progress;
            }
            yield return enumerator;

            // 释放资源
            package.UnloadUnusedAssets();
        }
    }
}
