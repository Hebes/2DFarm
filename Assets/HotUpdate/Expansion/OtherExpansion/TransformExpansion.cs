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
        public static Transform GetChild(this Transform transform, string childName)
        {
            Transform childTF = transform.Find(childName);
            if (childTF != null) return childTF;
            for (int i = 0; i < transform?.childCount; i++)
            {
                childTF = GetChild(transform.GetChild(i), childName); // 2.将任务交给子物体
                if (childTF != null)
                    return childTF;
            }
            return null;
        }
        public static GameObject GetChild(this GameObject gameObject, string childName)
        {
            return GetChild(gameObject.transform, childName).gameObject;
        }
        public static T GetChildComponent<T>(this Transform transform, string childName) where T : UnityEngine.Object
        {
            return GetChild(transform, childName)?.GetComponent<T>();
        }
        public static T GetChildComponent<T>(this GameObject gameObject, string childName) where T : UnityEngine.Object
        {
            return GetChild(gameObject.transform, childName)?.GetComponent<T>();
        }
        public static T AddChildComponent<T>(this GameObject gameObject, string childName) where T : Component
        {
            Transform t = GetChild(gameObject.transform, childName);
            return t?.GetComponent<T>() != null ? t.GetComponent<T>() : gameObject.AddComponent<T>();
        }

        public static void ClearChild(this Transform transform, params int[] Number)
        {
            if (transform.childCount <= 0) return;
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < Number.Length; j++)
                {
                    if (i == Number[j])
                        break;
                    else
                        GameObject.Destroy(transform.GetChild(i).gameObject);
                }
            }
        }
        public static void ClreatChildToPool()
        {

        }

        public static T GetChildAllPath<T>(this Transform transform, string childPath) where T : UnityEngine.Object
        {
            return transform.Find(childPath)?.GetComponent<T>();
        }
        public static T GetChildAllPath<T>(this GameObject gameObject, string childPath) where T : UnityEngine.Object
        {
            return gameObject.transform.Find(childPath)?.GetComponent<T>();
        }

        /// <summary>
        /// 给子节点添加父对象
        /// </summary>
        /// <param name="parents">父对象的方位</param>
        /// <param name="child">子对象的方法</param>
        public static void AddChildNodeToParentNode(Transform parents, Transform child)
        {
            child.SetParent(parents, false);
            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one;
            child.localEulerAngles = Vector3.zero;
        }
    }
}
