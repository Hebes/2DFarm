using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    UI继承的接口

-----------------------*/


namespace ACFrameworkCore
{
    public interface IUIState
    {
        /// <summary>
        /// UI界面的物体
        /// </summary>
        public GameObject UIGO { get; set; }

        /// <summary>
        /// UI初始化:只执行一次
        /// </summary>
        public abstract void UIAwake();

        /// <summary>
        /// 同monoUI
        /// </summary>
        public virtual void UIUpdate() { }

        /// <summary>
        /// UI开启调用
        /// </summary>
        public virtual void UIOnEnable() { }

        /// <summary>
        /// UI禁用调用
        /// </summary>
        public virtual void UIOnDisable() { }

        /// <summary>
        /// UI销毁调用
        /// </summary>
        public virtual void UIOnDestroy() { }
    }
}
