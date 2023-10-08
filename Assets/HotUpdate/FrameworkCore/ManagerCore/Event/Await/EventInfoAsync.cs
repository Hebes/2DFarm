using Cysharp.Threading.Tasks;
using System;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	等待监听

-----------------------*/

namespace Core
{


    //等待事件监听
    public class EventInfoAsync : IEventInfo
    {
        public delegate UniTask EventAsync();
        public event EventAsync eventAsync;

        public EventInfoAsync(EventAsync eventAsync)
        {
            this.eventAsync += eventAsync;
        }
        public async UniTask TriggerAsync()
        {
            Delegate[] actionUniTaskEventDelegate = eventAsync.GetInvocationList();
            await UniTask.WhenAll(Array.ConvertAll(actionUniTaskEventDelegate, del => ((EventAsync)del)()));
        }
    }
    public class EventInfoAsync<T> : IEventInfo
    {
        public delegate UniTask EventAsync(T t);
        public event EventAsync eventAsync;

        public EventInfoAsync(EventAsync eventAsync)
        {
            this.eventAsync += eventAsync;
        }
        public async UniTask TriggerAsync(T obj)
        {
            Delegate[] actionUniTaskEventDelegate = eventAsync.GetInvocationList();
            await UniTask.WhenAll(Array.ConvertAll(actionUniTaskEventDelegate, del => ((EventAsync)del)(obj)));
        }
    }
    public class EventInfoAsync<T, K> : IEventInfo
    {
        public delegate UniTask EventAsync(T t, K k);
        public event EventAsync eventAsync;
        public EventInfoAsync(EventAsync eventAsync)
        {
            this.eventAsync += eventAsync;
        }
        public async UniTask TriggerAsync(T obj1, K obj2)
        {
            Delegate[] actionUniTaskEventDelegate = eventAsync.GetInvocationList();
            await UniTask.WhenAll(Array.ConvertAll(actionUniTaskEventDelegate, del => ((EventAsync)del)(obj1, obj2)));
        }
    }
}
