using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;


/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    输入系统模块M->Model开头

-----------------------*/

namespace ACFrameworkCore
{
    public enum BTN_TYPE
    {
        UP,//上
        DOWN,//下
        LEFT,//左
        RIGHT,//右

        FIRE,//开火
        JUMP,//跳
    }

    public class MInputSystemManager:SingletonInit<MInputSystemManager>,ISingletonInit
    {

        //记录当前改哪一个键
        private BTN_TYPE nowType;

        public void Init()
        {

        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        public void LoadConfigFile()
        {
            //Resources.Load<TextAsset>()
        }

        /// <summary>
        /// 更换键位
        /// </summary>
        /// <param name="type"></param>
        private void ChangeBtn(BTN_TYPE type)
        {
            nowType = type;
            //得到一次任意键输入
            //InputSystem.onAnyButtonPress.CallOnce(ChangeBtnReally);
        }
    }
}
