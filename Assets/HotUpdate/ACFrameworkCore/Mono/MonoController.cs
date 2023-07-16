using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace ACFrameworkCore
{
    public class MonoController : MonoBehaviour
    {
        private event UnityAction AwakeEvent;

        private void Awake()
        {
            AwakeEvent?.Invoke();
        }
        private void Update()
        {

        }
        private void FixedUpdate()
        {

        }

        public void OnAddAwake(UnityAction fun)
        {
            AwakeEvent += fun;
        }
        public void OnRemoveAwake(UnityAction fun)
        {
            AwakeEvent -= fun;
        }


        public Coroutine MonoStartCoroutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }
        public Coroutine MonoStartCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            return StartCoroutine(methodName, value);
        }
        public Coroutine MonoStartCoroutine(string methodName)
        {
            return StartCoroutine(methodName);
        }
    }
}
