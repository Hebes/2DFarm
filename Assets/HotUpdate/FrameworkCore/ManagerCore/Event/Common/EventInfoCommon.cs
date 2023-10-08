/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    普通事件监听

-----------------------*/

namespace Core
{
    public class EventInfoCommon : IEventInfo
    {
        public delegate void CommonEvent();
        public event CommonEvent commonAction;
        public EventInfoCommon(CommonEvent commonAction)
        {
            this.commonAction += commonAction;
        }
        public void Trigger()
        {
            commonAction?.Invoke();
        }
    }

    public class EventInfoCommon<T> : IEventInfo
    {
        public delegate void CommonEvent(T t);
        public event CommonEvent commonAction;

        public EventInfoCommon(CommonEvent commonAction)
        {
            this.commonAction += commonAction;
        }
        public void Trigger(T obj)
        {
            commonAction?.Invoke(obj);
        }
    }

    public class EventInfoCommon<T, K> : IEventInfo
    {
        public delegate void CommonEvent(T t, K k);
        public event CommonEvent commonAction;

        public EventInfoCommon(CommonEvent commonAction)
        {
            this.commonAction += commonAction;
        }
        public void Trigger(T obj, K obj2)
        {
            commonAction?.Invoke(obj, obj2);
        }
    }

    public class EventInfoCommon<T, K, V> : IEventInfo
    {
        public delegate void CommonEvent(T t, K k, V v);
        public event CommonEvent commonAction;

        public EventInfoCommon(CommonEvent commonAction)
        {
            this.commonAction += commonAction;
        }
        public void Trigger(T t, K k, V v)
        {
            commonAction?.Invoke(t, k, v);
        }
    }

    public class EventInfoCommon<T, K, V, N> : IEventInfo
    {
        public delegate void CommonEvent(T t, K k, V v, N n);
        public event CommonEvent commonAction;

        public EventInfoCommon(CommonEvent commonAction)
        {
            this.commonAction += commonAction;
        }
        public void Trigger(T t, K k, V v, N n)
        {
            commonAction?.Invoke(t, k, v, n);
        }
    }

    public class EventInfoCommon<T, K, V, N, M> : IEventInfo
    {
        public delegate void CommonEvent(T t, K k, V v, N n,M m);
        public event CommonEvent commonAction;

        public EventInfoCommon(CommonEvent commonAction)
        {
            this.commonAction += commonAction;
        }
        public void Trigger(T t, K k, V v, N n, M m)
        {
            commonAction?.Invoke(t, k, v, n, m);
        }
    }
}
