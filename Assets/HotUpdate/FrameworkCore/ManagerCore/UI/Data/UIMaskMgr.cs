using UnityEngine;
using UnityEngine.UI;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    实现模态窗口（即： 弹出子窗口时，不允许玩家操作父窗体）

-----------------------*/

namespace Core
{
    public class UIMaskMgr
    {
        private static UIMaskMgr instance = null;
        //UI根节点对象
        private Transform _GoCanvasRoot = null;
        //顶层面板
        private Transform _GoTopPanel;
        //遮罩面板
        private GameObject _GoMaskPanel;
        //UI摄像机
        private Camera _UICamera;
        //UI摄像机原始的“层深”
        private float _OriginalUICameralDepth;

        public static UIMaskMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIMaskMgr();
                    instance.Init();
                }
                return instance;
            }
        }

        private void Init()
        {
            //得到UI根节点对象、脚本节点对象
            _GoCanvasRoot = CoreUI.Instance.CanvasTransfrom; 
            //得到“顶层面板”、“遮罩面板”
            _GoTopPanel = _GoCanvasRoot;
            _GoMaskPanel = _GoCanvasRoot.GetChild("_UIMaskPanel").gameObject;
            //得到UI摄像机原始的“层深”
            _UICamera = CoreUI.Instance.UICamera; 
            if (_UICamera != null)
            {
                //得到UI摄像机原始“层深”
                _OriginalUICameralDepth = _UICamera.depth;
            }
            else
            {
                Debug.Log(GetType() + "/Start()/UI_Camera is Null!,Please Check! ");
            }
        }

        /// <summary>
        /// 设置遮罩状态
        /// </summary>
        /// <param name="goDisplayUIForms">需要显示的UI窗体</param>
        /// <param name="lucenyType">显示透明度属性</param>
	    public void SetMaskWindow(GameObject goDisplayUIForms, EUILucenyType lucenyType = EUILucenyType.Lucency)
        {
            //顶层窗体下移
            _GoTopPanel.transform.SetAsLastSibling();
            //启用遮罩窗体以及设置透明度
            switch (lucenyType)
            {
                //完全透明，不能穿透
                case EUILucenyType.Lucency:
                    _GoMaskPanel.SetActive(true);
                    Color newColor1 = new Color(UIConfig.SYS_UIMASK_LUCENCY_COLOR_RGB, UIConfig.SYS_UIMASK_LUCENCY_COLOR_RGB, UIConfig.SYS_UIMASK_LUCENCY_COLOR_RGB, UIConfig.SYS_UIMASK_LUCENCY_COLOR_RGB_A);
                    _GoMaskPanel.GetComponent<Image>().color = newColor1;
                    break;
                //半透明，不能穿透
                case EUILucenyType.Translucence:
                    _GoMaskPanel.SetActive(true);
                    Color newColor2 = new Color(UIConfig.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB, UIConfig.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB, UIConfig.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB, UIConfig.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB_A);
                    _GoMaskPanel.GetComponent<Image>().color = newColor2;
                    break;
                //低透明，不能穿透
                case EUILucenyType.ImPenetrable:
                    _GoMaskPanel.SetActive(true);
                    Color newColor3 = new Color(UIConfig.SYS_UIMASK_IMPENETRABLE_COLOR_RGB, UIConfig.SYS_UIMASK_IMPENETRABLE_COLOR_RGB, UIConfig.SYS_UIMASK_IMPENETRABLE_COLOR_RGB, UIConfig.SYS_UIMASK_IMPENETRABLE_COLOR_RGB_A);
                    _GoMaskPanel.GetComponent<Image>().color = newColor3;
                    break;
                //可以穿透
                case EUILucenyType.Pentrate:
                    if (_GoMaskPanel.activeInHierarchy)
                        _GoMaskPanel.SetActive(false);
                    break;
            }

            //遮罩窗体下移
            _GoMaskPanel.transform.SetAsLastSibling();
            //显示窗体的下移
            goDisplayUIForms.transform.SetAsLastSibling();
            //增加当前UI摄像机的层深（保证当前摄像机为最前显示）
            if (_UICamera != null)
                _UICamera.depth = _UICamera.depth + 100;    //增加层深
        }

        /// <summary>
        /// 取消遮罩状态
        /// </summary>
	    public void CancelMaskWindow()
        {
            //顶层窗体上移
            _GoTopPanel.transform.SetAsFirstSibling();
            //禁用遮罩窗体
            if (_GoMaskPanel.activeInHierarchy)
                _GoMaskPanel.SetActive(false);//隐藏

            //恢复当前UI摄像机的层深 
            if (_UICamera != null)
                _UICamera.depth = _OriginalUICameralDepth;  //恢复层深
        }
    }
}
