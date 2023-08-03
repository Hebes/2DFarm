using UnityEngine;

namespace ACFrameworkCore
{
    public interface ISingletonInit
    {
        public void Init();
    }


    public class SingletonInit<T> where T : ISingletonInit, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    instance.Init();
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


    public class MonoSingletonInit<T> : MonoBehaviour where T : MonoBehaviour, ISingletonInit
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
                    instance.Init();
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
