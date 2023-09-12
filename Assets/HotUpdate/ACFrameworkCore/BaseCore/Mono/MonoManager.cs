using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using Time = UnityEngine.Time;
using ACFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace ACFarm
{
    public class MonoManager : ICore
    {
        public static MonoManager Instance;
        private MonoController monoController;
        private GameObject monoTemp;
        public void ICroeInit()
        {
            Instance = this;
            monoTemp = new GameObject("Mono");
            if (monoTemp == null)
                Debug.Log("monoTemp空");
            else
                Debug.Log("monoTemp不空");
            monoController = monoTemp.AddComponent<MonoController>();
            
            if (monoTemp.GetComponent(typeof(MonoController)) == null)
                Debug.Log("monoController空");
            else
                Debug.Log("monoController不空");
            GameObject.DontDestroyOnLoad(monoTemp);
            ACDebug.Log("初始化Mono完毕!");
        }


        private float m_Time = 0f;

        public void OnAddAwakeEvent(UnityAction unityAction)
        {
            monoController.OnAddAwakeEvent(unityAction);
        }
        public void OnRemoveAwakeEvent(UnityAction unityAction)
        {
            monoController.OnRemoveAwakeEvent(unityAction);
        }

        public void OnAddUpdateEvent(UnityAction unityAction)
        {
            //if (monoController == null)
            //{
            //    Debug.Log("monoController空");
            //}
            //else
            //{
            //    Debug.Log("monoController不空");
            //}
            monoController.OnAddUpdateEvent(unityAction);
        }
        public void OnRemoveUpdateEvent(UnityAction unityAction)
        {
            monoController.OnRemoveUpdateEvent(unityAction);
        }

        public void OnAddFixedUpdateEvent(UnityAction unityAction)
        {
            monoController.OnAddFixedUpdateEvent(unityAction);
        }
        public void OnRemoveFixedUpdateEvent(UnityAction unityAction)
        {
            monoController.OnRemoveFixedUpdateEvent(unityAction);
        }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return monoController.StartCoroutine(routine);
        }
        public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            return monoController.StartCoroutine(methodName, value);
        }
        public Coroutine StartCoroutine(string methodName)
        {
            return monoController.StartCoroutine(methodName);
        }
        public void MonoStopCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            monoController.StopCoroutine(methodName);
        }
        public void MonoStopCoroutine(IEnumerator routine)
        {
            monoController.StopCoroutine(routine);
        }

        public void MonoStopCoroutine(Coroutine routine)
        {
            monoController.StopCoroutine(routine);
        }

        public void Pause()
        {
            m_Time = Time.timeScale;
            Time.timeScale = 0f;//会影响UpData的Time.DataTime,但是Update函数仍在执行 和 FixedUpdate
        }
        public void UnPause(float m_Time)
        {
            Time.timeScale = m_Time;
            this.m_Time = Time.timeScale;
        }
    }
}
