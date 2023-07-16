using System;
using System.Collections.Generic;
using UnityEngine;

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

    public class UIComponent : ICoreComponent
    {
        public static UIComponent Instance { get; private set; }
        private Dictionary<string, BaseUI> panelDic { get; set; }
        private GameObject canvas;


        public GameObject CanvasTf
        {
            get 
            {
                if (canvas == null)
                {
                    canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
                    if (canvas == null)
                        Debug.LogError($"当前场景中不存在Canvas");
                }
                return canvas;
            }
        }

        public void OnCroeComponentInit()
        {
            Instance = this;
            panelDic = new Dictionary<string, BaseUI>();
            //GameObject.DontDestroyOnLoad(CanvasTf);
            OnCreatLayer();
        }

        /// <summary>
        /// 创建层级
        /// </summary>
        private void OnCreatLayer()
        {
            foreach (EUILayer layer in Enum.GetValues(typeof(EUILayer)))
            {
                var layerGo = new GameObject(layer.ToString(), typeof(RectTransform));
                var rect = layerGo.GetComponent<RectTransform>();
                rect.SetParent(CanvasTf.transform);
                rect.anchoredPosition = Vector3.zero;
            }
        }

        public Transform GetLayerFather(EUILayer layer)
        {
           return CanvasTf.transform.Find(layer.ToString());
        }

        public void OnOpenUI<T>(string panelName, EUILayer layer) where T : BaseUI
        {
            if (panelDic.ContainsKey(panelName))
            {
                panelDic[panelName].StartUI();
                return;
            }

            ResComponent.Insatance.OnLoadAsync<GameObject>("UI/" + panelName, obj =>
            {

                Transform father = GetLayerFather(layer);

                obj.transform.SetParent(father,false);

                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;

                (obj.transform as RectTransform).offsetMax = Vector2.zero;
                (obj.transform as RectTransform).offsetMin = Vector2.zero;

                T panel = obj.GetComponent<T>();
                panel.OpenUI();
                panelDic.Add(panelName, panel);
            });
        }

        public void OnHideUI(string panelName)
        {
            if (!panelDic.ContainsKey(panelName)) return;
            panelDic[panelName].HideUI();//关闭面板
            panelDic[panelName].UIGo.SetActive(false);
        }

        public void OnRemoveUI(string panelName)
        {
            if (!panelDic.ContainsKey(panelName)) return;
            panelDic[panelName].HideUI();//关闭面板
            panelDic[panelName].UIGo.SetActive(false);
            panelDic[panelName].RemoveUI();//移除面板
            GameObject.Destroy(panelDic[panelName].UIGo);
            panelDic.Remove(panelName);
        }
    }
}
