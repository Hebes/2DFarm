using System;
using System.Collections.Generic;
using UniFramework.Window;
using UnityEngine;
using YooAsset;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    UI管理类

-----------------------*/


namespace ACFrameworkCore
{

    public enum EUILayer
    {
        Bot = 1,
        Mid = 2,
        Top = 3,
        System = 4,
    }

    public class UIComponent : ICore
    {
        public static UIComponent Instance { get; private set; }

        private Dictionary<string, IUIState> panelDic { get; set; }

        private GameObject Global { get; set; }
        private GameObject canvas { get; set; }

        public GameObject CanvasGO
        {
            get
            {
                if (canvas == null)
                {
                    canvas = Global.transform.Find("Canvas").gameObject;
                    if (canvas == null)
                        Debug.LogError($"当前场景中不存在Canvas");
                }
                return canvas;
            }
        }

        public void ICroeInit()
        {
            Instance = this;
            panelDic = new Dictionary<string, IUIState>();

            //DLog.Log("创建出来的物体名称是: " + (handle1.AssetObject as GameObject).name);
            GameObject GlobalTemp = YooAssetLoadExpsion.YooaddetLoadSync("Global");//加载全局组件
            Global = GameObject.Instantiate(GlobalTemp);
            GameObject.DontDestroyOnLoad(Global);
            DLog.Log("UI管理类初始化成功!");
        }

        /// <summary>
        /// 获取层级
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public Transform GetLayerFather(EUILayer layer)
        {
            return CanvasGO.transform.Find(layer.ToString());
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="layer"></param>
        public void OnCreatUI<T>(string panelName, EUILayer layer) where T : IUIState, new()
        {
            //不存在的话就加载开启
            YooAssetLoadExpsion.YooaddetLoadAsync(panelName, obj =>
            {
                GameObject UIGO = obj.InstantiateSync();
                //GameObject UIGO = GameObject.Instantiate(UIGOTemp);
                UIGO.transform.SetParent(GetLayerFather(layer), false);
                UIGO.transform.localPosition = Vector3.zero;
                UIGO.transform.localScale = Vector3.one;
                (UIGO.transform as RectTransform).offsetMax = Vector2.zero;
                (UIGO.transform as RectTransform).offsetMin = Vector2.zero;

                T t = new T();
                t.UIGO = UIGO;
                t.UIAwake();
                MonoComponent.Instance.OnAddUpdateEvent(t.UIUpdate);

                //设置窗口层级
                WindowAttribute attribute = Attribute.GetCustomAttribute(typeof(T), typeof(WindowAttribute)) as WindowAttribute;
                if (attribute == null)
                    throw new Exception($"Window {typeof(T).FullName} not found {nameof(WindowAttribute)} attribute.");
                UIGO.GetComponent<Canvas>().sortingOrder = attribute.WindowLayer;

                panelDic.Add(panelName, t);
            });
        }

        /// <summary>
        /// 打开UI场景
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panelName"></param>
        /// <param name="layer"></param>
        public void OnOpenUI<T>(string panelName, EUILayer layer) where T : IUIState, new()
        {
            //存在的话就开启
            if (panelDic.ContainsKey(panelName))
                panelDic[panelName].UIOnEnable();
            else
                OnCreatUI<T>(panelName, layer);
        }

        /// <summary>
        /// 获取面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panelName"></param>
        /// <returns></returns>
        public T OnGetUI<T>(string panelName) where T : class
        {
            panelDic.TryGetValue(panelName, out IUIState t);
            if (t == null) return default(T);
            return t as T;
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="panelName"></param>
        public void OnCloseUI(string panelName)
        {
            panelDic.TryGetValue(panelName, out IUIState t);
            if (t == null) return;
            t.UIOnDisable();//关闭面板
            MonoComponent.Instance.OnRemoveUpdateEvent(t.UIUpdate);
            t.UIGO.SetActive(false);
        }

        /// <summary>
        /// 删除面板
        /// </summary>
        /// <param name="panelName"></param>
        public void OnRemoveUI(string panelName)
        {
            panelDic.TryGetValue(panelName, out IUIState t);
            if (t == null) return;
            MonoComponent.Instance.OnRemoveUpdateEvent(t.UIUpdate);
            t.UIOnDestroy();//关闭面板
            GameObject.Destroy(panelDic[panelName].UIGO);//删除面板
            panelDic.Remove(panelName);//字典移除
        }
    }
}
