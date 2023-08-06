using UnityEngine;

namespace ACFrameworkCore
{
    public class SingletonInit<T> where T : ICore, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    instance.ICroeInit();
                }
                return instance;
            }
        }
    }
    public class Singleton<T> where T :  new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();
                return instance;
            }
        }
    }


    public class MonoSingletonInit<T> : MonoBehaviour where T : MonoBehaviour, ICore
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    instance = obj.AddComponent<T>();
                    instance.ICroeInit();
                    DontDestroyOnLoad(obj);
                }

                return instance;
            }
        }
    }
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }

                return instance;
            }
        }
    }
}
