﻿using System;
using UnityEngine.Events;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    事件监听拓展

-----------------------*/

namespace ACFrameworkCore
{
    public static class EventExpansion
    {
        //不需要参数
        public static void AddEventListener(this Action action, string eventName)
        {
            EventManager.Instance.AddEventListener(eventName, action);
        }
        public static void AddEventListener(this string eventName, Action action)
        {
            EventManager.Instance.AddEventListener(eventName, action);
        }
        public static void RemoveEventListener(this Action action, string eventName)
        {
            EventManager.Instance.RemoveEventListener(eventName, action);
        }
        public static void RemoveEventListener(this string eventName, Action action)
        {
            EventManager.Instance.RemoveEventListener(eventName, action);
        }
        public static void EventTrigger(string eventName)
        {
            EventManager.Instance.EventTrigger(eventName);
        }

        //一个参数
        public static void AddEventListener<T>(this Action<T> action, string eventName)
        {
            EventManager.Instance.AddEventListener<T>(eventName, action);
        }
        public static void RemoveEventListener<T>(this Action<T> action, string eventName)
        {
            EventManager.Instance.RemoveEventListener<T>(eventName, action);
        }
        public static void EventTrigger<T>(this string eventName, T info)
        {
            EventManager.Instance.EventTrigger<T>(eventName, info);
        }

        //2个参数
        public static void AddEventListener<T, K>(this string name, Action<T, K> action)
        {
            EventManager.Instance.AddEventListener<T, K>(name, action);
        }
        public static void RemoveEventListener<T, K>(this string name, Action<T, K> action)
        {
            EventManager.Instance.RemoveEventListener<T, K>(name, action);
        }
        public static void EventTrigger<T, K>(this string name, T t, K k)
        {
            EventManager.Instance.EventTrigger<T, K>(name, t, k);
        }

        public static void Clear()
        {
            EventManager.Instance.Clear();
        }
    }
}
