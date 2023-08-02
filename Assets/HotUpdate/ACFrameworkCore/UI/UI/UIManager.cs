using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public class UIManager : ICore
    {
        public static UIManager Instance = null;
        public Transform CanvasTransfrom = null;                //UI根节点    
        public Camera UICamera = null;                                  //UI摄像机
        public Camera MainCamera = null;                               //主摄像机

        private Dictionary<string, UIBase> _DicALLUIForms;          //缓存所有UI窗体
        private Dictionary<string, UIBase> _DicCurrentShowUIForms;  //当前显示的UI窗体
        private Stack<UIBase> _StaCurrentUIForms;                   //定义“栈”集合,存储显示当前所有[反向切换]的窗体类型
        private Dictionary<string, AssetOperationHandle> YooAssetHdnleDic;//资源加载句柄
        private Transform Normal = null;                        //全屏幕显示的节点
        private Transform Fixed = null;                         //固定显示的节点
        private Transform PopUp = null;                         //弹出节点

        public void ICroeInit()
        {
            Instance = this;
            //字段初始化
            _DicALLUIForms = new Dictionary<string, UIBase>();
            _DicCurrentShowUIForms = new Dictionary<string, UIBase>();
            _StaCurrentUIForms = new Stack<UIBase>();
            YooAssetHdnleDic = new Dictionary<string, AssetOperationHandle>();
            InitRoot();
            DLog.Log("UI管理初始化完毕");
        }

        private void InitRoot()
        {
            AssetOperationHandle handle = YooAssetLoadExpsion.YooaddetLoadAsync("Global");
            handle.Completed += go =>
            {
                //实例化
                CanvasTransfrom = go.InstantiateSync().transform;
                GameObject.DontDestroyOnLoad(CanvasTransfrom);
                //获取子节点
                Normal = GetUITypeTransform(EUIType.Normal);
                Fixed = GetUITypeTransform(EUIType.Fixed);
                PopUp = GetUITypeTransform(EUIType.PopUp);
                UICamera = CanvasTransfrom.GetChildComponent<Camera>("UICamera");
                MainCamera = CanvasTransfrom.GetChildComponent<Camera>("MainCamera");
            };
            //创建Canvas
            //GameObject go = new GameObject("UICanvasRoot");
            //go.layer = LayerMask.NameToLayer("UI");
            //go.AddComponent<RectTransform>();

            //Canvas can = go.AddComponent<Canvas>();
            //can.renderMode = RenderMode.ScreenSpaceCamera;
            //can.pixelPerfect = true;

            //go.AddComponent<GraphicRaycaster>();

            //CanvasScaler canvasScaler = go.AddComponent<CanvasScaler>();
            //canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            ////canvasScaler.referenceResolution=new Vector2((float)Screen.width, (float)Screen.height);
            //canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            //Instance.root = go.transform;

            //GameObject camObj = new GameObject("UICamera");
            //camObj.layer = LayerMask.NameToLayer("UI");
            //camObj.transform.parent = go.transform;
            //camObj.transform.localPosition = new Vector3(0, 0, -100f);
            //Camera cam = camObj.AddComponent<Camera>();
            //cam.clearFlags = CameraClearFlags.Depth;
            //cam.orthographic = true;
            //cam.farClipPlane = 200f;
            //can.worldCamera = cam;
            //cam.cullingMask = 1 << 5;
            //cam.nearClipPlane = -50f;
            //cam.farClipPlane = 50f;

            //m_Instance.uiCamera = cam;

            ////add audio listener
            //camObj.AddComponent<AudioListener>();
            ////camObj.AddComponent<GUILayer>();

            //CanvasScaler cs = go.AddComponent<CanvasScaler>();
            //cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            //cs.referenceResolution = new Vector2(1136f, 640f);
            //cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            //GameObject subRoot;

            //subRoot = CreateSubCanvasForRoot(go.transform, 0);
            //subRoot.name = "NormalRoot";
            //m_Instance.normalRoot = subRoot.transform;
            //m_Instance.normalRoot.transform.localScale = Vector3.one;

            //subRoot = CreateSubCanvasForRoot(go.transform, 250);
            //subRoot.name = "FixedRoot";
            //m_Instance.fixedRoot = subRoot.transform;
            //m_Instance.fixedRoot.transform.localScale = Vector3.one;

            //subRoot = CreateSubCanvasForRoot(go.transform, 500);
            //subRoot.name = "PopupRoot";
            //m_Instance.popupRoot = subRoot.transform;
            //m_Instance.popupRoot.transform.localScale = Vector3.one;

            ////add Event System
            //GameObject esObj = GameObject.Find("EventSystem");
            //if (esObj != null)
            //    GameObject.DestroyImmediate(esObj);

            //GameObject eventObj = new GameObject("EventSystem");
            //eventObj.layer = LayerMask.NameToLayer("UI");
            //eventObj.transform.SetParent(go.transform);
            //eventObj.AddComponent<EventSystem>();
            //eventObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
        static GameObject CreateSubCanvasForRoot(Transform root, int sort)
        {
            //GameObject go = new GameObject("canvas");
            //go.transform.parent = root;
            //go.layer = LayerMask.NameToLayer("UI");

            //RectTransform rect = go.AddComponent<RectTransform>();
            //rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            //rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            //rect.anchorMin = Vector2.zero;
            //rect.anchorMax = Vector2.one;

            ////  Canvas can = go.AddComponent<Canvas>();
            ////  can.overrideSorting = true;
            ////  can.sortingOrder = sort;
            ////  go.AddComponent<GraphicRaycaster>();

            //return go;
            return null;
        }

        #region 增删改查方法
        public void ShwoUIPanel<T>(string uiFormName) where T : UIBase, new()
        {
            //UI类
            T t = LoadUIPanel<T>(uiFormName);

            //是否清空“栈集合”中得数据
            if (t.IsClearStack)
                ClearStackArray();

            //根据不同的UI窗体的显示模式，分别作不同的加载处理
            switch (t.mode)
            {
                case EUIMode.Normal:                 //“普通显示”窗口模式
                    //把当前窗体加载到“当前窗体”集合中。
                    LoadUIToCurrentCache<T>(uiFormName);
                    break;
                case EUIMode.ReverseChange:          //需要“反向切换”窗口模式
                    PushUIFormToStack(uiFormName);
                    break;
                case EUIMode.HideOther:              //“隐藏其他”窗口模式
                    EnterUIFormsAndHideOther(uiFormName);
                    break;
            }
            MonoComponent.Instance.OnAddUpdateEvent(t.UIUpdate);
        }//显示界面
        public void CloseUIForms(string uiFormName)
        {
            UIBase baseUiForm;                          //窗体基类

            //参数检查
            if (string.IsNullOrEmpty(uiFormName)) return;
            //“所有UI窗体”集合中，如果没有记录，则直接返回
            _DicALLUIForms.TryGetValue(uiFormName, out baseUiForm);
            if (baseUiForm == null) return;
            //根据窗体不同的显示类型，分别作不同的关闭处理
            switch (baseUiForm.mode)
            {
                case EUIMode.Normal:
                    //普通窗体的关闭
                    ExitUIForms(uiFormName);
                    break;
                case EUIMode.ReverseChange:
                    //反向切换窗体的关闭
                    PopUIFroms();
                    break;
                case EUIMode.HideOther:
                    //隐藏其他窗体关闭
                    ExitUIFormsAndDisplayOther(uiFormName);
                    break;

                default:
                    break;
            }
        }//界面关闭
        #endregion

        #region 显示界面
        private T LoadUIPanel<T>(string uiFormName) where T : UIBase, new()
        {
            T t = new T();
            //创建的UI克隆体预设
            //GameObject goCloneUIPrefabs = YooAssetLoadExpsion.YooaddetLoadSync(uiFormName);
            AssetOperationHandle handle = YooAssetLoadExpsion.YooaddetLoadSyncAOH(uiFormName);
            YooAssetHdnleDic.Add(uiFormName, handle);
            GameObject goCloneUIPrefabs = handle.InstantiateSync();//创建物体
            t.UIAwake();

            if (goCloneUIPrefabs == null)
                DLog.Error("加载预制体失败");
            t.gameObject = goCloneUIPrefabs;
            switch (t.type)
            {
                case EUIType.Normal: goCloneUIPrefabs.transform.SetParent(Normal, false); break;//普通窗体节点
                case EUIType.Fixed: goCloneUIPrefabs.transform.SetParent(Fixed, false); break;//固定窗体节点
                case EUIType.PopUp: goCloneUIPrefabs.transform.SetParent(PopUp, false); break;//弹出窗体节点
            }
            //设置隐藏
            goCloneUIPrefabs.SetActive(false);
            //把克隆体，加入到“所有UI窗体”（缓存）集合中。
            _DicALLUIForms.Add(uiFormName, t);
            return t;
        }
        /// <summary>
        /// 把当前窗体加载到“当前窗体”集合中
        /// </summary>
        /// <param name="uiFormName">窗体预设的名称</param>
	    private void LoadUIToCurrentCache<T>(string uiFormName) where T : UIBase
        {
            UIBase baseUiForm;                          //UI窗体基类
            UIBase baseUIFormFromAllCache;              //从“所有窗体集合”中得到的窗体

            //如果“正在显示”的集合中，存在整个UI窗体，则直接返回
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out baseUiForm);
            if (baseUiForm != null) return;
            //把当前窗体，加载到“正在显示”集合中
            _DicALLUIForms.TryGetValue(uiFormName, out baseUIFormFromAllCache);
            if (baseUIFormFromAllCache != null)
            {
                _DicCurrentShowUIForms.Add(uiFormName, baseUIFormFromAllCache as T);
                baseUIFormFromAllCache.UIOnEnable();           //显示当前窗体
            }
        }
        /// <summary>
        /// UI窗体入栈
        /// </summary>
        /// <param name="uiFormName">窗体的名称</param>
        private void PushUIFormToStack(string uiFormName)
        {
            UIBase baseUIForm;                          //UI窗体

            //判断“栈”集合中，是否有其他的窗体，有则“冻结”处理。
            if (_StaCurrentUIForms.Count > 0)
            {
                UIBase topUIForm = _StaCurrentUIForms.Peek();
                //栈顶元素作冻结处理
                topUIForm.Freeze();
            }
            //判断“UI所有窗体”集合是否有指定的UI窗体，有则处理。
            _DicALLUIForms.TryGetValue(uiFormName, out baseUIForm);
            if (baseUIForm != null)
            {
                //当前窗口显示状态
                baseUIForm.UIOnEnable();
                //把指定的UI窗体，入栈操作。
                _StaCurrentUIForms.Push(baseUIForm);
            }
            else
            {
                Debug.Log("baseUIForm==null,Please Check, 参数 uiFormName=" + uiFormName);
            }
        }
        /// <summary>
        /// (“隐藏其他”属性)打开窗体，且隐藏其他窗体
        /// </summary>
        /// <param name="strUIName">打开的指定窗体名称</param>
        private void EnterUIFormsAndHideOther(string strUIName)
        {
            UIBase baseUIForm;                          //UI窗体基类
            UIBase baseUIFormFromALL;                   //从集合中得到的UI窗体基类


            //参数检查
            if (string.IsNullOrEmpty(strUIName)) return;

            _DicCurrentShowUIForms.TryGetValue(strUIName, out baseUIForm);
            if (baseUIForm != null) return;

            //把“正在显示集合”与“栈集合”中所有窗体都隐藏。
            foreach (UIBase baseUI in _DicCurrentShowUIForms.Values)
            {
                baseUI.UIOnDisable();
            }
            foreach (UIBase staUI in _StaCurrentUIForms)
            {
                staUI.UIOnDisable();
            }

            //把当前窗体加入到“正在显示窗体”集合中，且做显示处理。
            _DicALLUIForms.TryGetValue(strUIName, out baseUIFormFromALL);
            if (baseUIFormFromALL != null)
            {
                _DicCurrentShowUIForms.Add(strUIName, baseUIFormFromALL);
                //窗体显示
                baseUIFormFromALL.UIOnEnable();
            }
        }
        #endregion

        #region 界面关闭
        /// <summary>
        /// 退出指定UI窗体
        /// </summary>
        /// <param name="strUIFormName"></param>
        private void ExitUIForms(string strUIFormName)
        {
            UIBase baseUIForm;                          //窗体基类

            //"正在显示集合"中如果没有记录，则直接返回。
            _DicCurrentShowUIForms.TryGetValue(strUIFormName, out baseUIForm);
            if (baseUIForm == null) return;
            //指定窗体，标记为“隐藏状态”，且从"正在显示集合"中移除。
            baseUIForm.UIOnDisable();
            _DicCurrentShowUIForms.Remove(strUIFormName);
            baseUIForm.UIOnDestroy();
            //资源卸载
            YooAssetHdnleDic.TryGetValue(strUIFormName, out AssetOperationHandle yooassetHandle);
            yooassetHandle?.Dispose();
        }
        /// <summary>
        /// （“反向切换”属性）窗体的出栈逻辑
        /// </summary>
        private void PopUIFroms()
        {
            if (_StaCurrentUIForms.Count >= 2)
            {
                //出栈处理
                UIBase topUIForms = _StaCurrentUIForms.Pop();
                //做隐藏处理
                topUIForms.UIOnDisable();
                //出栈后，下一个窗体做“重新显示”处理。
                UIBase nextUIForms = _StaCurrentUIForms.Peek();
                nextUIForms.UIOnEnable();
            }
            else if (_StaCurrentUIForms.Count == 1)
            {
                //出栈处理
                UIBase topUIForms = _StaCurrentUIForms.Pop();
                //做隐藏处理
                topUIForms.UIOnDisable();
            }
        }
        /// <summary>
        /// (“隐藏其他”属性)关闭窗体，且显示其他窗体
        /// </summary>
        /// <param name="strUIName">打开的指定窗体名称</param>
        private void ExitUIFormsAndDisplayOther(string strUIName)
        {
            UIBase baseUIForm;                          //UI窗体基类

            //参数检查
            if (string.IsNullOrEmpty(strUIName)) return;

            _DicCurrentShowUIForms.TryGetValue(strUIName, out baseUIForm);
            if (baseUIForm == null) return;

            //当前窗体隐藏状态，且“正在显示”集合中，移除本窗体
            baseUIForm.UIOnDisable();
            _DicCurrentShowUIForms.Remove(strUIName);

            //把“正在显示集合”与“栈集合”中所有窗体都定义重新显示状态。
            foreach (UIBase baseUI in _DicCurrentShowUIForms.Values)
            {
                baseUI.UIOnEnable();
            }
            foreach (UIBase staUI in _StaCurrentUIForms)
            {
                staUI.UIOnEnable();
            }
        }
        #endregion

        #region 其他
        /// <summary>
        /// 是否清空“栈集合”中得数据
        /// </summary>
        /// <returns></returns>
        private bool ClearStackArray()
        {
            if (_StaCurrentUIForms != null && _StaCurrentUIForms.Count >= 1)
            {
                //清空栈集合
                _StaCurrentUIForms.Clear();
                return true;
            }

            return false;
        }
        public Transform GetUITypeTransform(EUIType UIType)
        {
            return Instance.CanvasTransfrom.GetChild(UIType.ToString());
        }
        #endregion
    }
}
