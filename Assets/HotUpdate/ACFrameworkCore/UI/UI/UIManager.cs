using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    /// <summary> UI窗体（位置）类型 </summary>
    public enum UIFormType
    {
        /// <summary> 普通窗体 </summary>
        Normal,
        /// <summary> 固定窗体 </summary>
        Fixed,
        /// <summary> 弹出窗体 </summary>
        PopUp,
        /// <summary> 独立的窗口 </summary>
    }

    /// <summary>
    /// UI窗体的显示类型
    /// </summary>
    public enum UIFormShowMode
    {
        //普通
        Normal,
        //反向切换
        ReverseChange,
        //隐藏其他
        HideOther
    }

    /// <summary>
    /// UI窗体透明度类型
    /// </summary>
    public enum UIFormLucenyType
    {
        //完全透明，不能穿透
        Lucency,
        //半透明，不能穿透
        Translucence,
        //低透明度，不能穿透
        ImPenetrable,
        //可以穿透
        Pentrate
    }

    public class UIManager : ICore
    {
        private static UIManager m_Instance = null;
        public static UIManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    InitRoot();
                }
                return m_Instance;
            }
        }

        public Transform root;
        public Transform fixedRoot;
        public Transform normalRoot;
        public Transform popupRoot;
        public Camera uiCamera;

        public void ICroeInit()
        {
            m_Instance = this;
            InitRoot();
        }
        private static void InitRoot()
        {
            //GameObject go = new GameObject("UIRoot");
            //go.layer = LayerMask.NameToLayer("UI");
            //go.AddComponent<RectTransform>();

            //Canvas can = go.AddComponent<Canvas>();
            //can.renderMode = RenderMode.ScreenSpaceCamera;
            //can.pixelPerfect = true;

            //go.AddComponent<GraphicRaycaster>();

            //m_Instance.root = go.transform;

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


    }
}
