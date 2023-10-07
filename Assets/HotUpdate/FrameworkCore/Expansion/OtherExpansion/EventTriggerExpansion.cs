using System;
using UnityEngine;
using UnityEngine.EventSystems;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    EventTrigger拓展

-----------------------*/

namespace Core
{
    public static  class EventTriggerExpansion
    {
        /// <summary>
        /// 为EventTrigger的事件类型绑定Action方法
        /// </summary>
        /// <param name="trigger">EventTrigger组件对象</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="listenedAction">要执行的方法</param>
        public static void AddEventTriggerListener(this EventTrigger trigger, EventTriggerType eventType, Action<PointerEventData> listenedAction)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(data => listenedAction.Invoke((PointerEventData)data));
            trigger.triggers.Add(entry);
        }
        /// <summary>
        /// 添加EventTrigger组件
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="eventType"></param>
        /// <param name="listenedAction"></param>
        public static void AddEventTriggerListener(this Transform tf, EventTriggerType eventType, Action<PointerEventData> listenedAction)
        {
            //添加或获取组件
            EventTrigger eventTrigger = tf.GetComponent<EventTrigger>() == null ? tf.gameObject.AddComponent<EventTrigger>() : tf.GetComponent<EventTrigger>();
            //添加事件监听
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(data => listenedAction.Invoke((PointerEventData)data));
            eventTrigger.triggers.Add(entry);
        }

        /// <summary>
        /// 添加EventTrigger组件
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="eventType"></param>
        /// <param name="listenedAction"></param>
        public static void AddEventTriggerListener(this GameObject go, EventTriggerType eventType, Action<PointerEventData> listenedAction)
        {
            //添加或获取组件
            EventTrigger eventTrigger = go.GetComponent<EventTrigger>() == null ? go.AddComponent<EventTrigger>() : go.GetComponent<EventTrigger>();
            //添加事件监听
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(data => listenedAction.Invoke((PointerEventData)data));
            eventTrigger.triggers.Add(entry);
        }
    }
}
