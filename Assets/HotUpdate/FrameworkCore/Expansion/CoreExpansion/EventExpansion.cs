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
        //不等待
        public static void AddEventListener(this Action action, string eventName)
        {
            CoreEvent.Instance.AddEventListener(eventName, action);
        }
        public static void AddEventListener(this string eventName, Action action)
        {
            CoreEvent.Instance.AddEventListener(eventName, action);
        }
        public static void RemoveEventListener(this Action action, string eventName)
        {
            CoreEvent.Instance.RemoveEventListener(eventName, action);
        }
        public static void RemoveEventListener(this string eventName, Action action)
        {
            CoreEvent.Instance.RemoveEventListener(eventName, action);
        }
        public static void EventTrigger(this string eventName)
        {
            CoreEvent.Instance.EventTrigger(eventName);
        }
        public static void AddEventListener<T>(this Action<T> action, string eventName)
        {
            CoreEvent.Instance.AddEventListener<T>(eventName, action);
        }
        public static void AddEventListener<T>(this string eventName, Action<T> action)
        {
            CoreEvent.Instance.AddEventListener<T>(eventName, action);
        }
        public static void RemoveEventListener<T>(this Action<T> action, string eventName)
        {
            CoreEvent.Instance.RemoveEventListener<T>(eventName, action);
        }
        public static void RemoveEventListener<T>(this string eventName, Action<T> action)
        {
            CoreEvent.Instance.RemoveEventListener<T>(eventName, action);
        }
        public static void EventTrigger<T>(this string eventName, T info)
        {
            CoreEvent.Instance.EventTrigger<T>(eventName, info);
        }
        public static void AddEventListener<T, K>(this string name, Action<T, K> action)
        {
            CoreEvent.Instance.AddEventListener<T, K>(name, action);
        }
        public static void RemoveEventListener<T, K>(this string name, Action<T, K> action)
        {
            CoreEvent.Instance.RemoveEventListener<T, K>(name, action);
        }
        public static void EventTrigger<T, K>(this string name, T t, K k)
        {
            CoreEvent.Instance.EventTrigger<T, K>(name, t, k);
        }
        public static void AddEventListener<T, K, V>(this string name, Action<T, K, V> action)
        {
            CoreEvent.Instance.AddEventListener<T, K, V>(name, action);
        }
        public static void RemoveEventListener<T, K, V>(this string name, Action<T, K, V> action)
        {
            CoreEvent.Instance.RemoveEventListener<T, K, V>(name, action);
        }
        public static void EventTrigger<T, K, V>(this string name, T t, K k, V v)
        {
            CoreEvent.Instance.EventTrigger<T, K, V>(name, t, k, v);
        }
        public static void AddEventListener<T, K, V, N>(this string name, Action<T, K, V, N> action)
        {
            CoreEvent.Instance.AddEventListener<T, K, V, N>(name, action);
        }
        public static void RemoveEventListener<T, K, V, N>(this string name, Action<T, K, V, N> action)
        {
            CoreEvent.Instance.RemoveEventListener<T, K, V, N>(name, action);
        }
        public static void EventTrigger<T, K, V, N>(this string name, T t, K k, V v, N n)
        {
            CoreEvent.Instance.EventTrigger<T, K, V, N>(name, t, k, v, n);
        }
        public static void AddEventListener<T, K, V, N, M>(this string name, Action<T, K, V, N, M> action)
        {
            CoreEvent.Instance.AddEventListener<T, K, V, N, M>(name, action);
        }
        public static void RemoveEventListener<T, K, V, N, M>(this string name, Action<T, K, V, N, M> action)
        {
            CoreEvent.Instance.RemoveEventListener<T, K, V, N, M>(name, action);
        }
        public static void EventTrigger<T, K, V, N, M>(this string name, T t, K k, V v, N n, M m)
        {
            CoreEvent.Instance.EventTrigger<T, K, V, N, M>(name, t, k, v, n, m);
        }

        //等待
        public static void AddEventListenerUniTask(this string eventName, EventInfoUniTask.ActionUniTaskEvent action)
        {
            CoreEvent.Instance.AddEventListenerUniTask(eventName, action);
        }
        public static async UniTask EventTriggerUniTask(this string eventName)
        {
            await CoreEvent.Instance.EventTriggerUniTask(eventName);
        }
        public static void AddEventListenerUniTask<T>(this string eventName, EventInfoUniTask<T>.ActionUniTaskEvent action)
        {
            CoreEvent.Instance.AddEventListenerUniTask<T>(eventName, action);
        }
        public static void RemoveEventListenerUniTask<T>(this string eventName, EventInfoUniTask<T>.ActionUniTaskEvent action)
        {
            CoreEvent.Instance.RemoveEventListenerUniTask<T>(eventName, action);
        }
        public static async UniTask EventTriggerUniTask<T>(this string eventName, T info)
        {
            await CoreEvent.Instance.EventTriggerUniTask<T>(eventName, info);
        }
        public static void AddEventListenerUniTask<T, K>(this string eventName, EventInfoUniTask<T, K>.ActionUniTaskEvent action)
        {
            CoreEvent.Instance.AddEventListenerUniTask<T, K>(eventName, action);
        }
        public static async UniTask EventTriggerUniTask<T, K>(this string eventName, T t, K k)
        {
            await CoreEvent.Instance.EventTriggerUniTask<T, K>(eventName, t, k);
        }

        //清理
        public static void Clear()
        {
            CoreEvent.Instance.Clear();
        }
    }
}
