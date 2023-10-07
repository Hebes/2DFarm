using UnityEngine;

namespace Core
{
    //内存中的
    public class SingletonBaseInit<T> where T : ICore, new()
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
    public class SingletonBase<T> where T :  new()
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

    //通过new物体创建的
    public class SingletonNewMonoInit<T> : MonoBehaviour where T : MonoBehaviour, ICore
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
    public class SingletonNewMono<T> : MonoBehaviour where T : MonoBehaviour
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

    //已有物体创建的
    public class SinglentMono<T> : MonoBehaviour where T : SinglentMono<T>
    {
        private static T instance;
        public static T Instance => instance;
        protected virtual void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
                instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }

    }
}
