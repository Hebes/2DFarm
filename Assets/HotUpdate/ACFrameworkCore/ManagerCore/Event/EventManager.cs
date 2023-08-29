using Cysharp.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
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
            try
            {
                if (eventDic.ContainsKey(name))
                    (eventDic[name] as EventInfo).actions += action;
                else
                    eventDic.Add(name, new EventInfo(action));
            }
            catch (Exception e)
            {
                ACDebug.Error($"监听的参数异常请检查{e}");
                throw e;
            }

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
            try
            {
                if (eventDic.ContainsKey(name))
                    (eventDic[name] as EventInfo<T>).actions += action;
                else
                    eventDic.Add(name, new EventInfo<T>(action));
            }
            catch (Exception e)
            {
                ACDebug.Error($"监听的参数异常请检查{e}");
                throw e;
            }
        }
        public void RemoveEventListener<T>(string name, Action<T> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T>).actions -= action;
        }
        public void EventTrigger<T>(string name, T t)
        {
            if (!eventDic.ContainsKey(name)) return;
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            (eventDic[name] as EventInfo<T>).Trigger(t);
        }
        public void AddEventListener<T, K>(string name, Action<T, K> action)
        {
            try
            {
                if (eventDic.ContainsKey(name))
                    (eventDic[name] as EventInfo<T, K>).actions += action;
                else
                    eventDic.Add(name, new EventInfo<T, K>(action));
            }
            catch (Exception e)
            {
                ACDebug.Error($"监听的参数异常请检查{e}");
                throw e;
            }
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
            try
            {
                if (eventDic.ContainsKey(name))
                    (eventDic[name] as EventInfo<T, K, V>).actions += action;
                else
                    eventDic.Add(name, new EventInfo<T, K, V>(action));
            }
            catch (Exception e)
            {

                ACDebug.Error($"监听的参数异常请检查{e}");
                throw e;
            }
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
        public void AddEventListener<T, K, V, N>(string name, Action<T, K, V, N> action)
        {
            try
            {
                if (eventDic.ContainsKey(name))
                    (eventDic[name] as EventInfo<T, K, V, N>).actions += action;
                else
                    eventDic.Add(name, new EventInfo<T, K, V, N>(action));
            }
            catch (Exception e)
            {
                ACDebug.Error($"监听的参数异常请检查{e}");
                throw e;
            }
        }
        public void RemoveEventListener<T, K, V, N>(string name, Action<T, K, V, N> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T, K, V, N>).actions -= action;
        }
        public void EventTrigger<T, K, V, N>(string name, T t, K k, V v, N n)
        {
            if (!eventDic.ContainsKey(name)) return;
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            (eventDic[name] as EventInfo<T, K, V, N>).Trigger(t, k, v, n);
        }
        public void AddEventListener<T, K, V, N, M>(string name, Action<T, K, V, N, M> action)
        {
            try
            {
                if (eventDic.ContainsKey(name))
                    (eventDic[name] as EventInfo<T, K, V, N, M>).actions += action;
                else
                    eventDic.Add(name, new EventInfo<T, K, V, N, M>(action));

            }
            catch (Exception e)
            {

                ACDebug.Error($"监听的参数异常请检查{e}");
                throw e;
            }
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
            try
            {
                if (eventDic.ContainsKey(name))
                    (eventDic[name] as EventInfoUniTask<T>).actionUniTaskEvent += action;
                else
                    eventDic.Add(name, new EventInfoUniTask<T>(action));
            }
            catch (Exception e)
            {
                ACDebug.Error($"监听的参数异常请检查{e}");
                throw e;
            }
        }
        public async UniTask EventTriggerUniTask<T>(string name, T t)
        {
            if (!eventDic.ContainsKey(name)) return;
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            await (eventDic[name] as EventInfoUniTask<T>).TriggerUniTask(t);
        }
        public void AddEventListenerUniTask<T, K>(string name, EventInfoUniTask<T, K>.ActionUniTaskEvent action)
        {
            try
            {
                if (eventDic.ContainsKey(name))
                    (eventDic[name] as EventInfoUniTask<T, K>).actionUniTaskEvent += action;
                else
                    eventDic.Add(name, new EventInfoUniTask<T, K>(action));
            }
            catch (Exception e)
            {
                ACDebug.Error($"监听的参数异常请检查{e}");
                throw e;
            }
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
