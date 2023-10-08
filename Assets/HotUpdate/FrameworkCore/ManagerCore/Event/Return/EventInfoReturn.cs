using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	有返回值的

-----------------------*/

namespace Core
{
    public class EventInfoReturn : IEventInfo
    {
        public delegate Object EventReturn();
        public event EventReturn eventReturn;

        public EventInfoReturn(EventReturn eventReturn)
        {
            this.eventReturn += eventReturn;
        }

        public R Trigger<R>() where R : UnityEngine.Object
        {
            return eventReturn?.Invoke() as R;
        }
    }

    public class EventInfoReturn<T> : IEventInfo
    {
        public delegate Object EventReturn(T t);
        public event EventReturn eventReturn;

        public EventInfoReturn(EventReturn eventReturn)
        {
            this.eventReturn += eventReturn;
        }

        public R Trigger<R>(T t) where R : UnityEngine.Object
        {
            return eventReturn?.Invoke(t) as R;
        }
    }
}
