using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    Transform拓展
-----------------------*/

namespace ACFrameworkCore
{
    public static class TransformExpansion
    {
        public static T GetChild<T>(this Transform transform, string childName) where T : UnityEngine.Object
        {
            T childTF = transform.Find(childName)?.GetComponent<T>();
            if (childTF != null) return childTF;
            for (int i = 0; i < transform?.childCount; i++)
            {
                childTF = GetChild<T>(transform.GetChild(i), childName); // 2.将任务交给子物体
                if (childTF != null) return childTF;
            }
            return null;
        }
        public static T GetChild<T>(this GameObject gameObject, string childName) where T : UnityEngine.Object
        {
            T childTF = gameObject.transform.Find(childName)?.GetComponent<T>();
            if (childTF != null) return childTF;
            for (int i = 0; i < gameObject.transform?.childCount; i++)
            {
                childTF = GetChild<T>(gameObject.transform.GetChild(i), childName); // 2.将任务交给子物体
                if (childTF != null) return childTF;
            }
            return null;
        }
        public static T GetChildAllPath<T>(this Transform transform, string childPath) where T : UnityEngine.Object
        {
            return transform.Find(childPath)?.GetComponent<T>();
        }
        public static T GetChildAllPath<T>(this GameObject gameObject, string childPath) where T : UnityEngine.Object
        {
            return gameObject.transform.Find(childPath)?.GetComponent<T>();
        }
    }
}
