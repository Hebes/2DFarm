
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
    public class EventManager : SingletonInit<EventManager>, ICore
    {
        public void ICroeInit()
        {
            eventDic = new Dictionary<string, IEventInfo>();
        }
        private Dictionary<string, IEventInfo> eventDic { get; set; }

        //不需要参数的 
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
            (eventDic[name] as EventInfo).Trigger();
        }

        //带1参数的
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
            (eventDic[name] as EventInfo<T>).Trigger(info);
        }

        //带2个参数的
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
            (eventDic[name] as EventInfo<T, K>).Trigger(t, k);
        }

        public void Clear()
        {
            eventDic.Clear();
        }
    }
}
