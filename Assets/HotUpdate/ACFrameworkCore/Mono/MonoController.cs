using UnityEngine;
using UnityEngine.Events;

namespace ACFrameworkCore
{
    public class MonoController : MonoBehaviour
    {
        private event UnityAction AwakeEvent;
        private event UnityAction UpdateEvent;
        private event UnityAction FixedUpdateEvent;

        private void Awake()
        {
            AwakeEvent?.Invoke();
        }
        private void Update()
        {
            UpdateEvent?.Invoke();
        }
        private void FixedUpdate()
        {
            FixedUpdateEvent?.Invoke();
        }

        public void OnAddAwakeEvent(UnityAction  unityAction)
        {
            AwakeEvent += unityAction;
        }
        public void OnRemoveAwakeEvent(UnityAction unityAction)
        {
            AwakeEvent -= unityAction;
        }

        public void OnAddUpdateEvent(UnityAction unityAction)
        {
            UpdateEvent += unityAction;
        }
        public void OnRemoveUpdateEvent(UnityAction unityAction)
        {
            UpdateEvent -= unityAction;
        }

        public void OnAddFixedUpdateEvent(UnityAction unityAction)
        {
            FixedUpdateEvent += unityAction;
        }
        public void OnRemoveFixedUpdateEvent(UnityAction unityAction)
        {
            FixedUpdateEvent -= unityAction;
        }
    }
}
