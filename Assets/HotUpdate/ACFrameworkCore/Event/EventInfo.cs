using System;

namespace ACFrameworkCore
{
    public class EventInfo : IEventInfo
    {
        public event Action actions;
        public EventInfo(Action action)
        {
            actions += action;
        }
        public void Trigger()
        {
            actions?.Invoke();
        }
    }

    public class EventInfo<T> : IEventInfo
    {
        public event Action<T> actions;
        public EventInfo(Action<T> action)
        {
            actions += action;
        }
        public void Trigger(T obj)
        {
            actions?.Invoke(obj);
        }
    }

    public class EventInfo<T,K> : IEventInfo
    {
        public event Action<T, K> actions;
        public EventInfo(Action<T, K> action)
        {
            actions += action;
        }
        public void Trigger(T obj,K obj2)
        {
            actions?.Invoke(obj, obj2);
        }
    }
}
