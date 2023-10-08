using Time = UnityEngine.Time;

namespace Core
{
    public enum EMonoType
    {
        Updata,
        FixedUpdate,
    }

    public class CoreMono : ICore
    {
        public static CoreMono Instance;

        public void ICroeInit()
        {
            Instance = this;
            MonoController monoController = MonoController.Instance;
            Debug.Log("初始化Mono完毕!");
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="m_Time">0暂停1不暂停</param>
        public void Pause(float m_Time)
        {
            Time.timeScale = m_Time;
        }
    }
}
