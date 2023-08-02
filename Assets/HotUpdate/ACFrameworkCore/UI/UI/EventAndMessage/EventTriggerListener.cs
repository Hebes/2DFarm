using UnityEngine;
using UnityEngine.EventSystems;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    实现对于任何对象的监听处理。
    https://www.cnblogs.com/LiuGuozhu/p/7125662.html

-----------------------*/

namespace ACFrameworkCore
{
    public class EventTriggerListener : EventTrigger
    {
        public delegate void VoidDelegate(GameObject go);
        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;

        /// <summary>
        /// 得到“监听器”组件
        /// </summary>
        /// <param name="go">监听的游戏对象</param>
        /// <returns>
        /// 监听器
        /// </returns>
        public static EventTriggerListener Get( GameObject go)
        {
            EventTriggerListener lister = go.GetComponent<EventTriggerListener>();
            if (lister == null)
                lister = go.AddComponent<EventTriggerListener>();
            return lister;
        }
        public static EventTriggerListener Get(Transform go)
        {
            EventTriggerListener lister = go.GetComponent<EventTriggerListener>();
            if (lister == null)
                lister = go.gameObject.AddComponent<EventTriggerListener>();
            return lister;
        }

        public override void OnPointerClick(PointerEventData eventData) => onClick?.Invoke(gameObject);
        public override void OnPointerDown(PointerEventData eventData) => onDown?.Invoke(gameObject);
        public override void OnPointerEnter(PointerEventData eventData) => onEnter?.Invoke(gameObject);
        public override void OnPointerExit(PointerEventData eventData) => onExit?.Invoke(gameObject);
        public override void OnPointerUp(PointerEventData eventData) => onUp?.Invoke(gameObject);
        public override void OnSelect(BaseEventData eventBaseData) => onSelect?.Invoke(gameObject);
        public override void OnUpdateSelected(BaseEventData eventBaseData) => onUpdateSelect?.Invoke(gameObject);
    }
}
