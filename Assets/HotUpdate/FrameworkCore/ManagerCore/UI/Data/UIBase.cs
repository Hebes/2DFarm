﻿using Farm2D;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    窗口基类

-----------------------*/

namespace Core
{
    public class UIBase
    {
        public bool IsClearStack;                           //是否清空“栈集合”
        public EUIType type = EUIType.Normal;               //窗口的位置
        public EUIMode mode = EUIMode.Normal;               //窗口显示类型
        public EUILucenyType lucenyType = EUILucenyType.Lucency;   //窗口的透明度

        public string UIName { get; set; }                        //UI的名称
        public GameObject panelGameObject { get; set; }                //窗口的物体

        /// <summary>初始化方法</summary>
        /// <param name="type">窗口的位置</param>
        /// <param name="mod">窗口显示类型</param>
        /// <param name="lucenyType">窗口的透明度</param>
        /// <param name="isClearStack">是否清空“栈集合”</param>
        protected void InitUIBase(EUIType type, EUIMode mod, EUILucenyType lucenyType, bool isClearStack = false)
        {
            this.type = type;
            this.mode = mod;
            this.lucenyType = lucenyType;
            //this.name = gameObject.name.Replace("(Clone)", "");// this.GetType().ToString();
            IsClearStack = isClearStack;
        }

        //生命周期
        public virtual void UIAwake()
        {

        }       //初始化执行
        public virtual void UIUpdate()
        {

        }      //轮询执行
        public virtual void UIOnEnable()
        {
            this.panelGameObject.SetActive(true);
            //设置模态窗体调用(必须是弹出窗体)
            if (type == EUIType.PopUp)
                UIMaskMgr.Instance.SetMaskWindow(this.panelGameObject, lucenyType);
        }    //开启执行
        public virtual void UIOnDisable()
        {
            this.panelGameObject.SetActive(false);
            //取消模态窗体调用
            if (type == EUIType.PopUp)
                UIMaskMgr.Instance.CancelMaskWindow();
        }   //关闭执行
        public virtual void UIOnDestroy() { }   //销毁执行
        public virtual void Freeze()
        {
            this.panelGameObject.SetActive(true);
        }         //冻结状态（即：窗体显示在其他窗体下面）

        //面板操作
        protected void OpenUIForm<T>(string uiFormName) where T : UIBase, new()
        {
            CoreUI.Instance.ShwoUIPanel<T>(uiFormName);
        }
        protected T GetUIForm<T>(string uiFormName) where T : UIBase, new()
        {
           return CoreUI.Instance.GetUIPanl<T>(uiFormName);
        }
        protected void CloseUIForm()
        {
            //ACDebug.Log($"关闭的界面名称是:{UIName}");
            int intPosition = -1;
            string strUIFromName = UIName;  // GetType().ToString().Replace("Panel","");             //命名空间+类名 //处理后的UIFrom 名称
            intPosition = strUIFromName.IndexOf('.');
            if (intPosition != -1)
                strUIFromName = strUIFromName.Substring(intPosition + 1);//剪切字符串中“.”之间的部分
            //ACDebug.Log($"关闭的界面名称是:{strUIFromName}");
            CoreUI.Instance.CloseUIForms(strUIFromName);
        }
        protected void CloseOtherUIForm(string uiFormName)
        {
            CoreUI.Instance.CloseUIForms(uiFormName);
        }

        //事件推送

        protected void ButtonOnClickAddListener(string buttonName, EventTriggerListener.VoidDelegate delHandle)
        {
            GameObject goButton = this.panelGameObject.GetChild(buttonName);
            //给按钮注册事件方法
            if (goButton != null)
                EventTriggerListener.Get(goButton).onClick = delHandle;
        }
        protected void ButtonOnEnterAddListener(string buttonName, EventTriggerListener.VoidDelegate delHandle)
        {
            GameObject goButton = this.panelGameObject.GetChild(buttonName);
            //给按钮注册事件方法
            if (goButton != null)
                EventTriggerListener.Get(goButton).onEnter = delHandle;
        }

        /// <summary>发送消息</summary>
        /// <param name="msgType">消息的类型</param>
        /// <param name="msgName">消息名称</param>
        /// <param name="msgContent">消息内容</param>
        protected void SendMessage(string msgType, string msgName, object msgContent)
        {
            KeyValuesUpdate kvs = new KeyValuesUpdate(msgName, msgContent);
            MessageCenter.SendMessage(msgType, kvs);
        }
        /// <summary> 接收消息 </summary> 
        /// <param name="messagType">消息分类</param>
        /// <param name="handler">消息委托</param>
        public void ReceiveMessage(string messagType, MessageCenter.DelMessageDelivery handler)
        {
            MessageCenter.AddMsgListener(messagType, handler);
        }

        //其他方法
        public string Show(string message)
        {
            //TODO 后续需要自己写
            return string.Empty;
            //return LauguageMgr.GetInstance().ShowText(message);

        }//显示语言
    }
}
