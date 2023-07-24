using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace ACFrameworkCore
{
    public class MonoComponent : ICoreComponent
    {
        public static MonoComponent Instance { get; private set; }
        public MonoController monoController { get; set; }

        public void OnCroeComponentInit()
        {
            Instance = this;
            GameObject monoTemp = new GameObject("Mono");
            GameObject.DontDestroyOnLoad(monoTemp);
            monoController = monoTemp.AddComponent<MonoController>();
            DLog.Log("初始化Mono完毕!");
        }


        public void OnAddAwakeEvent(UnityAction unityAction)
        {
            monoController.OnAddAwakeEvent(unityAction);
        }
        public void OnRemoveAwakeEvent(UnityAction unityAction)
        {
            monoController.OnRemoveAwakeEvent(unityAction);
        }

        public void OnAddUpdateEvent(UnityAction unityAction)
        {
            monoController.OnAddUpdateEvent(unityAction);
        }
        public void OnRemoveUpdateEvent(UnityAction unityAction)
        {
            monoController.OnRemoveUpdateEvent(unityAction);
        }


        public Coroutine MonoStartCoroutine(IEnumerator routine)
        {
            return monoController.MonoStartCoroutine(routine);
        }
        public Coroutine MonoStartCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            return monoController.MonoStartCoroutine(methodName, value);
        }
        public Coroutine MonoStartCoroutine(string methodName)
        {
            return monoController.MonoStartCoroutine(methodName);
        }
    }
}
