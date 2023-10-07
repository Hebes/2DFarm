using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core
{
    public static class ToggleExpansion
    {
        public static void AddToggleListener(this Transform tf, UnityAction<bool> listenedAction, ToggleGroup group = null)
        {
            //添加或获取组件
            Toggle trigger = null;
            trigger = tf.GetComponent<Toggle>();
            trigger = trigger == null ? tf.gameObject.AddComponent<Toggle>() : trigger;
            trigger.onValueChanged.AddListener(listenedAction);
            trigger.group = group;
        }
    }
}
