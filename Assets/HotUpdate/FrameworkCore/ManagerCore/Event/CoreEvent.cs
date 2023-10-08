using Cysharp.Threading.Tasks;
using System.Collections.Generic;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    事件中心模块

-----------------------*/

namespace Core
{
    public class CoreEvent : ICore
    {
        public static CoreEvent Instance;
        private Dictionary<string, IEventInfo> eventDic;

        public void ICroeInit()
        {
            Instance = this;
            eventDic = new Dictionary<string, IEventInfo>();
        }

        #region 普通
        public void EventAdd(string eventName, EventInfoCommon.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon).commonAction += commonAction;
            else
                eventDic.Add(eventName, new EventInfoCommon(commonAction));
        }
        public void EventRemove(string eventName, EventInfoCommon.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon).commonAction -= commonAction;
        }
        public void EventTrigger(string eventName)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon).Trigger();//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }


        public void EventAdd<T>(string eventName, EventInfoCommon<T>.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T>).commonAction += commonAction;
            else
                eventDic.Add(eventName, new EventInfoCommon<T>(commonAction));
        }
        public void EventRemove<T>(string eventName, EventInfoCommon<T>.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T>).commonAction -= commonAction;
        }
        public void EventTrigger<T>(string eventName, T t)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T>).Trigger(t);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }


        public void EventAdd<T, K>(string eventName, EventInfoCommon<T, K>.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K>).commonAction += commonAction;
            else
                eventDic.Add(eventName, new EventInfoCommon<T, K>(commonAction));
        }
        public void EventRemove<T, K>(string eventName, EventInfoCommon<T, K>.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K>).commonAction -= commonAction;
        }
        public void EventTrigger<T, K>(string eventName, T t, K k)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K>).Trigger(t, k);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }



        public void EventAdd<T, K, V>(string eventName, EventInfoCommon<T, K, V>.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K, V>).commonAction += commonAction;
            else
                eventDic.Add(eventName, new EventInfoCommon<T, K, V>(commonAction));
        }
        public void EventRemove<T, K, V>(string eventName, EventInfoCommon<T, K, V>.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K, V>).commonAction -= commonAction;
        }
        public void EventTrigger<T, K, V>(string eventName, T t, K k, V v)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K, V>).Trigger(t, k, v);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }


        public void EventAdd<T, K, V, N>(string eventName, EventInfoCommon<T, K, V, N>.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N>).commonAction += commonAction;
            else
                eventDic.Add(eventName, new EventInfoCommon<T, K, V, N>(commonAction));
        }
        public void EventRemove<T, K, V, N>(string eventName, EventInfoCommon<T, K, V, N>.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N>).commonAction -= commonAction;
        }
        public void EventTrigger<T, K, V, N>(string eventName, T t, K k, V v, N n)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N>).Trigger(t, k, v, n);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }



        public void EventAdd<T, K, V, N, M>(string eventName, EventInfoCommon<T, K, V, N, M>.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N, M>).commonAction += commonAction;
            else
                eventDic.Add(eventName, new EventInfoCommon<T, K, V, N, M>(commonAction));
        }
        public void EventRemove<T, K, V, N, M>(string eventName, EventInfoCommon<T, K, V, N, M>.CommonEvent commonAction)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N, M>).commonAction -= commonAction;
        }
        public void EventTrigger<T, K, V, N, M>(string eventName, T t, K k, V v, N n, M m)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N, M>).Trigger(t, k, v, n, m);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }
        #endregion


        #region 返回值
        public void EventAddReturn(string eventName, EventInfoReturn.EventReturn eventAsync)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoReturn).eventReturn += eventAsync;
            else
                eventDic.Add(eventName, new EventInfoReturn(eventAsync));
        }
        public R EventTriggerReturn<R>(string eventName) where R : UnityEngine.Object
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                return (eventInfo as EventInfoReturn).Trigger<R>();//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            return null;
        }

        public void EventAddReturn<T>(string eventName, EventInfoReturn<T>.EventReturn eventAsync)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoReturn<T>).eventReturn += eventAsync;
            else
                eventDic.Add(eventName, new EventInfoReturn<T>(eventAsync));
        }
        /// <summary>
        /// 事件触发
        /// </summary>
        /// <typeparam name="R">返回的类型</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public R EventTriggerReturn<R, T>(string eventName, T t) where R : UnityEngine.Object
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                return (eventInfo as EventInfoReturn<T>).Trigger<R>(t);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            return null;
        }
        #endregion


        #region 等待
        public void EventAddAsync(string eventName, EventInfoAsync.EventAsync eventAsync)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoAsync).eventAsync += eventAsync;
            else
                eventDic.Add(eventName, new EventInfoAsync(eventAsync));
        }
        public async UniTask EventTriggerAsync(string eventName)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                await (eventInfo as EventInfoAsync).TriggerAsync();//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }


        public void EventAddAsync<T>(string eventName, EventInfoAsync<T>.EventAsync eventAsync)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoAsync<T>).eventAsync += eventAsync;
            else
                eventDic.Add(eventName, new EventInfoAsync<T>(eventAsync));
        }

        public async UniTask EventTriggerAsync<T>(string eventName, T t)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                await (eventInfo as EventInfoAsync<T>).TriggerAsync(t);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }


        public void EventAddAsync<T, K>(string eventName, EventInfoAsync<T, K>.EventAsync eventAsync)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                (eventInfo as EventInfoAsync<T, K>).eventAsync += eventAsync;
            else
                eventDic.Add(eventName, new EventInfoAsync<T, K>(eventAsync));
        }
        public async UniTask EventTriggerAsync<T, K>(string eventName, T t, K k)
        {
            if (eventDic.TryGetValue(eventName, out IEventInfo eventInfo))
                await (eventInfo as EventInfoAsync<T, K>).TriggerAsync(t, k);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }
        #endregion


        //清理
        public void Clear()
        {
            eventDic.Clear();
        }
    }
}
