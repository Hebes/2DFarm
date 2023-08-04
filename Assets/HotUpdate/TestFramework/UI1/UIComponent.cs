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
    需要参考:
    https://blog.csdn.net/qq_31480839/article/details/106023788
    https://blog.csdn.net/qq_35030499/article/details/83241388
    https://blog.csdn.net/qq_37436859/article/details/91042315
    https://www.bilibili.com/video/BV1GP4y127NP?p=61&vd_source=541350093e1daa8eebb09b34588484e7

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

    public class UIComponent : SingletonInit<UIComponent>,ISingletonInit
    {
        public void Init()
        {
            //panelDic = new Dictionary<string, IUIState>();
            //AssetOperationHandleList = new Dictionary<string, AssetOperationHandle>();

            ////DLog.Log("创建出来的物体名称是: " + (handle1.AssetObject as GameObject).name);
            //GameObject GlobalTemp = YooAssetLoadExpsion.YooaddetLoadSync("Global");//加载全局组件
            //Global = GameObject.Instantiate(GlobalTemp);
            //GameObject.DontDestroyOnLoad(Global);
            //DLog.Log("UI管理类初始化成功!");
        }

        private Dictionary<string, IUIState> panelDic { get; set; }
        private Dictionary<string, AssetOperationHandle> AssetOperationHandleList { get; set; }//资源句柄

        private GameObject Global { get; set; }
        private GameObject Canvas => Global.transform.Find("Canvas").gameObject;

        public Transform GetLayerFather(EUILayer layer)
        {
            return Canvas.transform.Find(layer.ToString());
        }

        public void OnUICreatAsync<T>(string panelName, EUILayer layer, Action action) where T : IUIState, new()
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
                CMonoManager.Instance.OnAddUpdateEvent(t.UIUpdate);

                //设置窗口层级
                WindowAttribute attribute = Attribute.GetCustomAttribute(typeof(T), typeof(WindowAttribute)) as WindowAttribute;
                if (attribute == null)
                    throw new Exception($"Window {typeof(T).FullName} not found {nameof(WindowAttribute)} attribute.");
                UIGO.GetComponent<Canvas>().sortingOrder = attribute.WindowLayer;

                panelDic.Add(panelName, t);
                AssetOperationHandleList.Add(panelName, obj);
                action?.Invoke();
            });
        }
        public void OnOpenUIAsync<T>(string panelName, EUILayer layer, Action action = null) where T : IUIState, new()
        {
            //存在的话就开启
            if (panelDic.ContainsKey(panelName))
                panelDic[panelName].UIOnEnable();
            else
                OnUICreatAsync<T>(panelName, layer, action);
        }

        public void OnOpenUI(string panelName)
        {
            //if (!panelDic.ContainsKey(panelName)) return;
            //panelDic[panelName]
        }
        public T OnUIGet<T>(string panelName) where T : class
        {
            panelDic.TryGetValue(panelName, out IUIState t);
            if (t == null) return default(T);
            return t as T;
        }
        public bool OnUIExist(string panelName)
        {
            panelDic.TryGetValue(panelName, out IUIState t);
            return t != null;
        }
        public void OnCloseUI(string panelName)
        {
            panelDic.TryGetValue(panelName, out IUIState t);
            if (t == null) return;
            t.UIOnDisable();//关闭面板
            CMonoManager.Instance.OnRemoveUpdateEvent(t.UIUpdate);
            t.UIGO.SetActive(false);
        }
        public void OnDestroyUI(string panelName)
        {
            panelDic.TryGetValue(panelName, out IUIState t);
            if (t == null) return;
            CMonoManager.Instance.OnRemoveUpdateEvent(t.UIUpdate);
            t.UIOnDestroy();//关闭面板
            //销毁资源
            GameObject.Destroy(panelDic[panelName].UIGO);//删除面板
            panelDic.Remove(panelName);//字典移除
            AssetOperationHandleList.TryGetValue(panelName, out AssetOperationHandle assetOperationHandle);
            assetOperationHandle.Release();
        }

       
    }
}
