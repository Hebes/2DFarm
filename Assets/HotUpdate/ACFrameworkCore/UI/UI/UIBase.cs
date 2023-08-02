using SUIFW;
using System;
using System.Collections.Generic;
using System.IO;
using TinyTeam.UI;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    窗口基类

-----------------------*/

namespace ACFrameworkCore
{
    public class UIBase
    {
        
        public bool IsClearStack;                           //是否清空“栈集合”
        public EUIType type = EUIType.Normal;               //窗口的位置
        public EUIMode mode = EUIMode.Normal;               //窗口显示类型
        public EUILucenyType lucenyType = EUILucenyType.ImPenetrable;   //窗口的透明度
        public string name;                //UI的名称
        public GameObject gameObject;       //窗口的物体

        public UIBase(EUIType type, EUIMode mod, EUILucenyType lucenyType, bool isClearStack = false)
        {
            this.type = type;
            this.mode = mod;
            this.lucenyType = lucenyType;
            this.name = this.GetType().ToString();
            IsClearStack = isClearStack;
        }

        #region 生命周期
        public virtual void UIAwake() { }       //初始化执行
        public virtual void UIUpdate() 
        {

        }      //轮询执行
        public virtual void UIOnEnable() 
        {
            this.gameObject.SetActive(true);
            //设置模态窗体调用(必须是弹出窗体)
            if (type == EUIType.PopUp)
            {
                UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject, _CurrentUIType.UIForm_LucencyType);
            }
        }    //开启执行
        public virtual void UIOnDisable() { }   //关闭执行
        public virtual void UIOnDestroy() { }   //销毁执行
	    public virtual void Freeze()
        {
            this.gameObject.SetActive(true);
        }               //冻结状态（即：窗体显示在其他窗体下面）
        #endregion

        #region 拓展方法

        #endregion
    }
}
