using Cysharp.Threading.Tasks;
using System;

namespace Core
{
    //不等待事件监听
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
        public void Trigger(T obj, K obj2, V obj3)
        {
            actions?.Invoke(obj, obj2, obj3);
        }
    }
    public class EventInfo<T, K, V, N> : IEventInfo
    {
        public event Action<T, K, V, N> actions;
        public EventInfo(Action<T, K, V, N> action)
        {
            actions += action;
        }
        public void Trigger(T obj, K obj2, V obj3, N obj4)
        {
            actions?.Invoke(obj, obj2, obj3, obj4);
        }
    }
    public class EventInfo<T, K, V, N, M> : IEventInfo
    {
        public event Action<T, K, V, N, M> actions;
        public EventInfo(Action<T, K, V, N, M> action)
        {
            actions += action;
        }
        public void Trigger(T obj, K obj2, V obj3, N obj4, M obj5)
        {
            actions?.Invoke(obj, obj2, obj3, obj4, obj5);
        }
    }

    //等待事件监听
    public class EventInfoUniTask : IEventInfo
    {
        public delegate UniTask ActionUniTaskEvent();
        public event ActionUniTaskEvent actionUniTaskEvent;

        public EventInfoUniTask(ActionUniTaskEvent actionUniTaskEvent)
        {
            this.actionUniTaskEvent += actionUniTaskEvent;
        }
        public async UniTask TriggerUniTask()
        {
            Delegate[] actionUniTaskEventDelegate = actionUniTaskEvent.GetInvocationList();
            await UniTask.WhenAll(Array.ConvertAll(actionUniTaskEventDelegate, del => ((ActionUniTaskEvent)del)()));
        }
    }
    public class EventInfoUniTask<T> : IEventInfo
    {
        public delegate UniTask ActionUniTaskEvent(T t);
        public event ActionUniTaskEvent actionUniTaskEvent;

        public EventInfoUniTask(ActionUniTaskEvent actionUniTaskEvent)
        {
            this.actionUniTaskEvent += actionUniTaskEvent;
        }
        public async UniTask TriggerUniTask(T obj)
        {
            Delegate[] actionUniTaskEventDelegate = actionUniTaskEvent.GetInvocationList();
            await UniTask.WhenAll(Array.ConvertAll(actionUniTaskEventDelegate, del => ((ActionUniTaskEvent)del)(obj)));
        }
    }
    public class EventInfoUniTask<T, K> : IEventInfo
    {
        public delegate UniTask ActionUniTaskEvent(T t,K k);
        public event ActionUniTaskEvent actionUniTaskEvent;
        public EventInfoUniTask(ActionUniTaskEvent actionUniTaskEvent)
        {
            this.actionUniTaskEvent += actionUniTaskEvent;
        }
        public async UniTask TriggerUniTask(T obj1, K obj2)
        {
            Delegate[] actionUniTaskEventDelegate = actionUniTaskEvent.GetInvocationList();
            await UniTask.WhenAll(Array.ConvertAll(actionUniTaskEventDelegate, del => ((ActionUniTaskEvent)del)(obj1, obj2)));
        }
    }
}
