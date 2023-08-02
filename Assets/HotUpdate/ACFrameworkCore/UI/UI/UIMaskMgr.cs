using SUIFW;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    实现模态窗口（即： 弹出子窗口时，不允许玩家操作父窗体）

-----------------------*/

namespace ACFrameworkCore
{
    public class UIMaskMgr : MonoSingleton<UIMaskMgr>
    {
        //UI根节点对象
        private GameObject _GoCanvasRoot = null;
        //UI脚本节点对象
        private Transform _TraUIScriptsNode = null;
        //顶层面板
        private GameObject _GoTopPanel;
        //遮罩面板
        private GameObject _GoMaskPanel;
        //UI摄像机
        private Camera _UICamera;
        //UI摄像机原始的“层深”
        private float _OriginalUICameralDepth;

        private void Awake()
        {
            //得到UI根节点对象、脚本节点对象
            _GoCanvasRoot = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS);
            _TraUIScriptsNode = UnityHelper.FindTheChildNode(_GoCanvasRoot, SysDefine.SYS_SCRIPTMANAGER_NODE);
            //把本脚本实例，作为“脚本节点对象”的子节点。
            UnityHelper.AddChildNodeToParentNode(_TraUIScriptsNode, this.gameObject.transform);
            //得到“顶层面板”、“遮罩面板”
            _GoTopPanel = _GoCanvasRoot;
            _GoMaskPanel = UnityHelper.FindTheChildNode(_GoCanvasRoot, "_UIMaskPanel").gameObject;
            //得到UI摄像机原始的“层深”
            _UICamera = GameObject.FindGameObjectWithTag("_TagUICamera").GetComponent<Camera>();
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
    }
}
