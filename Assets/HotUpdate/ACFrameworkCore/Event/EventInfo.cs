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

    public class EventInfo<T, K> : IEventInfo
    {
        public event Action<T, K> actions;
        public EventInfo(Action<T, K> action)
        {
            actions += action;
        }
        public void Trigger(T obj, K obj2)
        {
            actions?.Invoke(obj, obj2);
        }
    }
    public class EventInfo<T, K, V> : IEventInfo
    {
        public event Action<T, K, V> actions;
        public EventInfo(Action<T, K, V> action)
        {
            actions += action;
        }
        public void Trigger(T obj, K obj2,V obj3)
        {
            actions?.Invoke(obj, obj2, obj3);
        }
    }
}
