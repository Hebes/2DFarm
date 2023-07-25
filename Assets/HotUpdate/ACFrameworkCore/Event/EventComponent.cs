
using System.Collections.Generic;
using UnityEngine.Events;

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
    public class EventComponent : ICoreComponent
    {
        public static EventComponent Instance { get; private set; }
        private Dictionary<string, IEventInfo> eventDic { get; set; }

        public void CroeComponentInit()
        {
            Instance = this;
            eventDic = new Dictionary<string, IEventInfo>();
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">准备用来处理事件 的委托函数</param>
        public void AddEventListener<T>(string name, UnityAction<T> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T>).actions += action;
            else
                eventDic.Add(name, new EventInfo<T>(action));
        }

        /// <summary>
        /// 监听不需要参数传递的事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void AddEventListener(string name, UnityAction action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo).actions += action;
            else
                eventDic.Add(name, new EventInfo(action));
        }

        /// <summary>
        /// 移除对应的事件监听
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">对应之前添加的委托函数</param>
        public void RemoveEventListener<T>(string name, UnityAction<T> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T>).actions -= action;
        }

        /// <summary>
        /// 移除不需要参数的事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void RemoveEventListener(string name, UnityAction action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo).actions -= action;
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="name">哪一个名字的事件触发了</param>
        public void EventTrigger<T>(string name, T info)
        {
            if (!eventDic.ContainsKey(name)) return;
            if ((eventDic[name] as EventInfo<T>).actions != null)
                (eventDic[name] as EventInfo<T>).actions.Invoke(info);
        }

        /// <summary>
        /// 事件触发（不需要参数的）
        /// </summary>
        /// <param name="name"></param>
        public void EventTrigger(string name)
        {
            if (!eventDic.ContainsKey(name)) return;
            if ((eventDic[name] as EventInfo).actions != null)
                (eventDic[name] as EventInfo).actions.Invoke();
        }

        /// <summary>
        /// 清空事件中心
        /// 主要用在 场景切换时
        /// </summary>
        public void Clear()
        {
            eventDic.Clear();
        }
    }
}
