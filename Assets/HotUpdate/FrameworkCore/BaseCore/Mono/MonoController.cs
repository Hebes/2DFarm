using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    Mono生命周期监听

-----------------------*/

namespace Core
{
    public class MonoController : SingletonNewMono<MonoController>
    {
        private event UnityAction UpdateEvent;
        private event UnityAction FixedUpdateEvent;
        private Dictionary<string, Coroutine> CoroutineDic = new Dictionary<string, Coroutine>();


        private void Update() => UpdateEvent?.Invoke();
        private void FixedUpdate() => FixedUpdateEvent?.Invoke();


        public void AddMonEvent(EMonoType eMonoType, UnityAction unityAction)
        {
            switch (eMonoType)
            {
                case EMonoType.Updata: UpdateEvent += unityAction; break;
                case EMonoType.FixedUpdate: FixedUpdateEvent += unityAction; break;
            }
        }

        public void RemoveMonoEvent(EMonoType monoType, UnityAction unityAction)
        {
            switch (monoType)
            {
                case EMonoType.Updata: UpdateEvent -= unityAction; break;
                case EMonoType.FixedUpdate: FixedUpdateEvent -= unityAction; break;
            }
        }

        public void AddCoroutine(string coroutineKey, IEnumerator coroutine)
        {
            Coroutine coroutine1 = StartCoroutine(coroutine);
            CoroutineDic.Add(coroutineKey, coroutine1);
        }

        public void RemoveCoroutine(string coroutineKey)
        {
            if (CoroutineDic.TryGetValue(coroutineKey, out Coroutine coroutine))
                StopCoroutine(coroutine);
        }
    }
}
