using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;
using UnityEngine.Windows;


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

    public class MInputSystemManager : SingletonInit<MInputSystemManager>, ICore
    {

        //记录当前改哪一个键
        private BTN_TYPE nowType;
        //private Lesson9Input input;

        public PlayerInput playerInput;

        public void ICroeInit()
        {
            //LoadConfigFile();
        }

        public void LoadConfigFile()
        {
            ////1.创建生成的代码对象
            //input = new Input();
            ////2.激活输入
            //input.Enable();
            ////3.事件监听
            //input.Action1.Frie.performed += (context) =>
            //{
            //    DLog.Log("开火");
            //};

            //input.Action2.Space.performed += (context) =>
            //{
            //    print("跳跃");
            //};
            //input = new Lesson9Input();
            ////GameObject goTemp = CResourceManager.Instance.LoadAsset<GameObject>(ConfigPrefab.CubePrefab);
            //// GameObject go = GameObject.Instantiate(goTemp);
            ////playerInput = go.AddComponent<PlayerInput>();
            ////playerInput.actions = input.asset;
            //input.Enable();
            //input.Action1.Fire.performed += (context) =>
            //{
            //    DLog.Log("开火");
            //};

            //input.Action2.Space.performed += (context) =>
            //{
            //    DLog.Log("跳跃");
            //};

            //playerInput.onActionTriggered += (context) =>
            //{
            //    if (context.phase == InputActionPhase.Performed)
            //    {
            //        switch (context.action.name)
            //        {
            //            case "Move":
            //                DLog.Log("移动");
            //                break;
            //            case "Look":
            //                DLog.Log("看向");
            //                break;
            //            case "Frie":
            //                DLog.Log("开火");
            //                break;
            //        }
            //    }
            //};
        }
        public void LoadConfigFileToJson()
        {
            //string json = Resources.Load<TextAsset>("PlayerInputTest").text;
            //InputActionAsset asset = InputActionAsset.FromJson(json);
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
