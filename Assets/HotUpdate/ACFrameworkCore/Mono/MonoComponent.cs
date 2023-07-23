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
            monoController = new GameObject("Mono").AddComponent<MonoController>();
            DLog.Log("初始化Mono完毕!");
        }


        public void OnAddAwake(UnityAction fun)
        {
            monoController.OnAddAwake(fun);
        }
        public void OnRemoveAwake(UnityAction fun)
        {
            monoController.OnRemoveAwake(fun);
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
