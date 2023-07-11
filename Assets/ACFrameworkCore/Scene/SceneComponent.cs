using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ACFrameworkCore
{
    public class SceneComponent : ICoreComponent
    {
        public static SceneComponent Instance { get; private set; } 
        public void OnCroeComponentInit()
        {
            Instance=this;
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
        public void LoadSceneAsyn(string name, UnityAction fun)
        {
            MonoComponent.Instance.monoController.MonoStartCoroutine(ReallyLoadSceneAsyn(name, fun));
        }
        private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction fun)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(name);
            while (!ao.isDone)
            {
                EventComponent.Instance.EventTrigger("进度条更新", ao.progress);
                yield return ao.progress;
            }
            fun();
        }

    }
}
