using dnlib.DotNet;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using UnityEngine.Events;
using UnityEngine.UIElements;

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
        public static void AddEventListener<T>(this UnityAction<T> action,string eventName)
        {
            EventComponent.Instance.AddEventListener<T>(eventName, action);
        }
        public static void AddEventListener(this UnityAction action, string eventName)
        {
            EventComponent.Instance.AddEventListener(eventName, action);
        }
        public static void AddEventListener(this string eventName , UnityAction action)
        {
            EventComponent.Instance.AddEventListener(eventName, action);
        }

        public static void RemoveEventListener<T>(this UnityAction<T> action, string eventName)
        {
            EventComponent.Instance.RemoveEventListener<T>(eventName, action);
        }
        public static void RemoveEventListener(this UnityAction action, string eventName)
        {
            EventComponent.Instance.RemoveEventListener(eventName, action);
        }
        public static void RemoveEventListener(this string eventName , UnityAction action)
        {
            EventComponent.Instance.RemoveEventListener(eventName, action);
        }

        public static void TriggerEvent<T>(this string eventName, T info)
        {
            EventComponent.Instance.EventTrigger<T>(eventName, info);
        }
        public static void TriggerEvent(string eventName)
        {
            EventComponent.Instance.EventTrigger(eventName);
        }
    }
}
