using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    事件中心模块

-----------------------*/

namespace ACFrameworkCore
{
    public class EventManager : ICore
    {
        public static EventManager Instance;
        private Dictionary<string, IEventInfo> eventDic;

        public void ICroeInit()
        {
            Instance = this;
            eventDic = new Dictionary<string, IEventInfo>();
        }

        //不等待
        public void AddEventListener(string name, Action action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo).actions += action;
            else
                eventDic.Add(name, new EventInfo(action));
        }
        public void RemoveEventListener(string name, Action action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo).actions -= action;
        }
        public void EventTrigger(string name)
        {
            if (!eventDic.ContainsKey(name)) return;
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            (eventDic[name] as EventInfo).Trigger();
        }
        public void AddEventListener<T>(string name, Action<T> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T>).actions += action;
            else
                eventDic.Add(name, new EventInfo<T>(action));
        }
        public void RemoveEventListener<T>(string name, Action<T> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T>).actions -= action;
        }
        public void EventTrigger<T>(string name, T info)
        {
            if (!eventDic.ContainsKey(name)) return;
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            (eventDic[name] as EventInfo<T>).Trigger(info);
        }
        public void AddEventListener<T, K>(string name, Action<T, K> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T, K>).actions += action;
            else
                eventDic.Add(name, new EventInfo<T, K>(action));
        }
        public void RemoveEventListener<T, K>(string name, Action<T, K> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T, K>).actions -= action;
        }
        public void EventTrigger<T, K>(string name, T t, K k)
        {
            if (!eventDic.ContainsKey(name)) return;
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            (eventDic[name] as EventInfo<T, K>).Trigger(t, k);
        }
        public void AddEventListener<T, K, V>(string name, Action<T, K, V> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T, K, V>).actions += action;
            else
                eventDic.Add(name, new EventInfo<T, K, V>(action));
        }
        public void RemoveEventListener<T, K, V>(string name, Action<T, K, V> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T, K, V>).actions -= action;
        }
        public void EventTrigger<T, K, V>(string name, T t, K k, V v)
        {
            if (!eventDic.ContainsKey(name)) return;
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            (eventDic[name] as EventInfo<T, K, V>).Trigger(t, k, v);
        }
        public void AddEventListener<T, K, V, N, M>(string name, Action<T, K, V, N, M> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T, K, V, N, M>).actions += action;
            else
                eventDic.Add(name, new EventInfo<T, K, V, N, M>(action));
        }
        public void RemoveEventListener<T, K, V, N, M>(string name, Action<T, K, V, N, M> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T, K, V, N, M>).actions -= action;
        }
        public void EventTrigger<T, K, V, N, M>(string name, T t, K k, V v, N n, M m)
        {
            if (!eventDic.ContainsKey(name)) return;
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            (eventDic[name] as EventInfo<T, K, V, N, M>).Trigger(t, k, v, n, m);
        }

        //等待
        public void AddEventListenerUniTask<T>(string name, EventInfoUniTask<T>.ActionUniTaskEvent action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfoUniTask<T>).actionUniTaskEvent += action;
            else
                eventDic.Add(name, new EventInfoUniTask<T>(action));
        }
        public async UniTask EventTriggerUniTask<T>(string name, T info)
        {
            if (!eventDic.ContainsKey(name)) return;
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            await (eventDic[name] as EventInfoUniTask<T>).TriggerUniTask(info);
        }
        public void AddEventListenerUniTask<T, K>(string name, EventInfoUniTask<T, K>.ActionUniTaskEvent action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfoUniTask<T, K>).actionUniTaskEvent += action;
            else
                eventDic.Add(name, new EventInfoUniTask<T, K>(action));
        }
        public async UniTask EventTriggerUniTask<T, K>(string name, T t, K k)
        {
            if (!eventDic.ContainsKey(name)) return;
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            await (eventDic[name] as EventInfoUniTask<T, K>).TriggerUniTask(t, k);
        }

        //清理
        public void Clear()
        {
            eventDic.Clear();
        }
    }
}
