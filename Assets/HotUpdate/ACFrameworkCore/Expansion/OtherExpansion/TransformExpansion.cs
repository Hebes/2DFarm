using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        //获取子物体
        public static T AddChildComponent<T>(this GameObject gameObject, string childName) where T : Component
        {
            Transform t = GetChild(gameObject.transform, childName);
            return t?.GetComponent<T>() != null ? t.GetComponent<T>() : gameObject.AddComponent<T>();
        }
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
        public static T GetChild<T>(this GameObject gameObject, string childName)where T : Component
        {
            return GetChild(gameObject.transform, childName).GetComponent<T>();
        }
        public static T GetChildComponent<T>(this Transform transform, string childName) where T : UnityEngine.Object
        {
            return GetChild(transform, childName)?.GetComponent<T>();
        }
        public static T GetChildComponent<T>(this Component  component, string childName) where T : UnityEngine.Object
        {
            return GetChild(component.transform, childName)?.GetComponent<T>();
        }
        public static T GetChildComponent<T>(this GameObject gameObject, string childName) where T : UnityEngine.Object
        {
            return GetChild(gameObject.transform, childName)?.GetComponent<T>();
        }
        public static T GetChildComponent<T>(this GameObject gameObject, int i)
        {
           return gameObject.transform.GetChild(i).GetComponent<T>();
        }

        //清除子物体
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

        //通过全路径获取组件
        public static T GetChildAllPath<T>(this Transform transform, string childPath) where T : UnityEngine.Object
        {
            return transform.Find(childPath)?.GetComponent<T>();
        }
        public static T GetChildAllPath<T>(this GameObject gameObject, string childPath) where T : UnityEngine.Object
        {
            return gameObject.transform.Find(childPath)?.GetComponent<T>();
        }

        //给子节点添加父对象
        public static void AddChildNodeToParentNode(Transform parents, Transform child)
        {
            child.SetParent(parents, false);
            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one;
            child.localEulerAngles = Vector3.zero;
        }

        //快捷获取
        public static Image GetImage(this GameObject gameObject)
        {
           return gameObject.GetComponent<Image>();
        }
        public static Image GetImage(this Transform transform)
        {
            return transform.GetComponent<Image>();
        }
        public static Text GetText(this GameObject gameObject)
        {
            return gameObject.GetComponent<Text>();
        }
        public static Text GetText(this Transform transform)
        {
            return transform.GetComponent<Text>();
        }
        public static TextMeshProUGUI GetTextMeshPro(this GameObject gameObject)
        {
            return gameObject.GetComponent<TextMeshProUGUI>();
        }
        public static TextMeshProUGUI GetTextMeshPro(this Transform transform)
        {
            return transform.GetComponent<TextMeshProUGUI>();
        }
        public static InputField GetInputField(this GameObject gameObject)
        {
            return gameObject.GetComponent<InputField>();
        }
        public static InputField GetInputField(this Transform transform)
        {
            return transform.GetComponent<InputField>();
        }
        public static Button GetButton(this GameObject gameObject)
        {
            return gameObject.GetComponent<Button>();
        }
        public static Button GetButton(this Transform transform)
        {
            return transform.GetComponent<Button>();
        }


        //关闭显示
        public static void SetActive(this Image image,bool value)
        {
            image.gameObject.SetActive(value);
        }
        public static void SetActive(this Transform transform, bool value)
        {
            transform.gameObject.SetActive(value);
        }
    }
}
