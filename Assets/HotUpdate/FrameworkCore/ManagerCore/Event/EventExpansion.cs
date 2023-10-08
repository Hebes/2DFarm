using Cysharp.Threading.Tasks;
using System;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    事件监听拓展

-----------------------*/

namespace Core
{
    public static class EventExpansion
    {
        #region 等待
        public static void EventAdd(this string eventName, EventInfoCommon.CommonEvent action)
        {
            CoreEvent.Instance.EventAdd(eventName, action);
        }
        public static void EventRemove(this string eventName, EventInfoCommon.CommonEvent action)
        {
            CoreEvent.Instance.EventRemove(eventName, action);
        }
        public static void EventTrigger(this string eventName)
        {
            CoreEvent.Instance.EventTrigger(eventName);
        }

        public static void EventAdd<T>(this string eventName, EventInfoCommon<T>.CommonEvent action)
        {
            CoreEvent.Instance.EventAdd<T>(eventName, action);
        }

        public static void EventRemove<T>(this string eventName, EventInfoCommon<T>.CommonEvent action)
        {
            CoreEvent.Instance.EventRemove<T>(eventName, action);
        }
        public static void EventTrigger<T>(this string eventName, T info)
        {
            CoreEvent.Instance.EventTrigger<T>(eventName, info);
        }

        public static void EventAdd<T, K>(this string name, EventInfoCommon<T, K>.CommonEvent action)
        {
            CoreEvent.Instance.EventAdd<T, K>(name, action);
        }
        public static void EventRemove<T, K>(this string name, EventInfoCommon<T, K>.CommonEvent action)
        {
            CoreEvent.Instance.EventRemove<T, K>(name, action);
        }
        public static void EventTrigger<T, K>(this string name, T t, K k)
        {
            CoreEvent.Instance.EventTrigger<T, K>(name, t, k);
        }

        public static void EventAdd<T, K, V>(this string name, EventInfoCommon<T, K, V>.CommonEvent action)
        {
            CoreEvent.Instance.EventAdd<T, K, V>(name, action);
        }
        public static void RemoveEventListener<T, K, V>(this string name, EventInfoCommon<T, K, V>.CommonEvent action)
        {
            CoreEvent.Instance.EventRemove<T, K, V>(name, action);
        }
        public static void EventTrigger<T, K, V>(this string name, T t, K k, V v)
        {
            CoreEvent.Instance.EventTrigger<T, K, V>(name, t, k, v);
        }


        public static void EventAdd<T, K, V, N>(this string name, EventInfoCommon<T, K, V, N>.CommonEvent action)
        {
            CoreEvent.Instance.EventAdd<T, K, V, N>(name, action);
        }
        public static void RemoveEventListener<T, K, V, N>(this string name, EventInfoCommon<T, K, V, N>.CommonEvent action)
        {
            CoreEvent.Instance.EventRemove<T, K, V, N>(name, action);
        }
        public static void EventTrigger<T, K, V, N>(this string name, T t, K k, V v, N n)
        {
            CoreEvent.Instance.EventTrigger<T, K, V, N>(name, t, k, v, n);
        }

        public static void EventAdd<T, K, V, N, M>(this string name, EventInfoCommon<T, K, V, N, M>.CommonEvent action)
        {
            CoreEvent.Instance.EventAdd<T, K, V, N, M>(name, action);
        }
        public static void EventRemove<T, K, V, N, M>(this string name, EventInfoCommon<T, K, V, N, M>.CommonEvent action)
        {
            CoreEvent.Instance.EventRemove<T, K, V, N, M>(name, action);
        }
        public static void EventTrigger<T, K, V, N, M>(this string name, T t, K k, V v, N n, M m)
        {
            CoreEvent.Instance.EventTrigger<T, K, V, N, M>(name, t, k, v, n, m);
        }
        #endregion



        #region 返回
        public static void EventAddReturn(this string eventName, EventInfoReturn.EventReturn action)
        {
            CoreEvent.Instance.EventAddReturn(eventName, action);
        }
        public static R EventTriggerReturn<R>(this string eventName) where R : UnityEngine.Object
        {
            return CoreEvent.Instance.EventTriggerReturn<R>(eventName);
        }

        public static void EventAddReturn<T>(this string eventName, EventInfoReturn<T>.EventReturn action)
        {
            CoreEvent.Instance.EventAddReturn<T>(eventName, action);
        }
        public static R EventTriggerReturn<R, T>(this string eventName, T t) where R : UnityEngine.Object
        {
            return CoreEvent.Instance.EventTriggerReturn<R, T>(eventName, t);
        }
        #endregion



        #region 等待
        public static void EventAddAsync(this string eventName, EventInfoAsync.EventAsync action)
        {
            CoreEvent.Instance.EventAddAsync(eventName, action);
        }
        public static async UniTask EventTriggerAsync(this string eventName)
        {
            await CoreEvent.Instance.EventTriggerAsync(eventName);
        }
        public static void EventAddAsync<T>(this string eventName, EventInfoAsync<T>.EventAsync action)
        {
            CoreEvent.Instance.EventAddAsync<T>(eventName, action);
        }

        public static async UniTask EventTriggerAsync<T>(this string eventName, T info)
        {
            await CoreEvent.Instance.EventTriggerAsync<T>(eventName, info);
        }
        public static void EventAddAsync<T, K>(this string eventName, EventInfoAsync<T, K>.EventAsync action)
        {
            CoreEvent.Instance.EventAddAsync<T, K>(eventName, action);
        }
        public static async UniTask EventTriggerAsync<T, K>(this string eventName, T t, K k)
        {
            await CoreEvent.Instance.EventTriggerAsync<T, K>(eventName, t, k);
        }
        #endregion



        //清理
        public static void Clear()
        {
            CoreEvent.Instance.Clear();
        }
    }
}
