using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    输入系统模块
    总的思路就是更换配置文件信息

-----------------------*/

namespace ACFrameworkCore
{
    //按键类型
    public enum BTN_TYPE
    {
        UP,//上
        DOWN,//下
        LEFT,//左
        RIGHT,//右

        FIRE,//开火
        JUMP,//跳
    }



    public class InputSystemSystem : SingletonInit<InputSystemSystem>, ICore
    {
        public ConfigInputInfo inputInfo;//按键信息
        public PlayerInput playerInput;//玩家角色控制器

        private string jsonStr;//Json配置信息
        private BTN_TYPE nowType;                                       //记录当前改哪一个键
        private string INPUT_FILE_CONFIG_NAME = "InputSystemConfig";    //配置文件名称
        private InputActionAsset inputActions;
        //核心初始化
        public void ICroeInit()
        {
            jsonStr = Resources.Load<TextAsset>("Lesson17").text;//加载配置按键模板
            LoadInputConfig();
        }

        //保存输入配置
        public void SaveInputConfig()
        {
            ManagerDataExpansion.Save(inputInfo, INPUT_FILE_CONFIG_NAME, EDataType.Json);
        }
        //读取输入配置并启用
        public void LoadInputConfig()
        {
            ConfigInputInfo configInputInfo = ManagerDataExpansion.Load<ConfigInputInfo>(INPUT_FILE_CONFIG_NAME, EDataType.Json);
            inputInfo = configInputInfo == null ? new ConfigInputInfo() : configInputInfo;
            //替换按键
            string str = jsonStr.Replace(inputInfo.upDefault, inputInfo.upCurrent);//上键
            str = str.Replace(inputInfo.downDefault, inputInfo.downCurrent);//下
            str = str.Replace(inputInfo.leftDefault, inputInfo.leftCurrent);//左
            str = str.Replace(inputInfo.rightDefault, inputInfo.rightCurrent);//右
            str = str.Replace(inputInfo.fireDefault, inputInfo.fireCurrent);//开火
            str = str.Replace(inputInfo.jumpDefault, inputInfo.jumpCurrent);//跳跃
            inputActions = InputActionAsset.FromJson(str);
        }

        //启用配置
        public void EnableInputConfig()
        {
            playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            playerInput.actions = inputActions;
            playerInput.actions.Enable();//启用设置
        }

        //改建
        public void GetActionAsset() 
        {
            //替换按键
            string str = jsonStr.Replace(inputInfo.upCurrent, inputInfo.upTemp);//上键
            str = str.Replace(inputInfo.downCurrent, inputInfo.downTemp);//下
            str = str.Replace(inputInfo.leftCurrent, inputInfo.leftTemp);//左
            str = str.Replace(inputInfo.rightCurrent, inputInfo.rightTemp);//右
            str = str.Replace(inputInfo.fireCurrent, inputInfo.fireTemp);//开火
            str = str.Replace(inputInfo.jumpCurrent, inputInfo.jumpTemp);//跳跃
            inputActions = InputActionAsset.FromJson(str);
        }

        //重置按键
        public void ResetKeypad()
        {
            ////替换按键
            //string str = jsonStr.Replace(inputInfo.upCurrent, inputInfo.upDefault);//上键
            //str = str.Replace(inputInfo.downCurrent, inputInfo.downDefault);//下
            //str = str.Replace(inputInfo.leftCurrent, inputInfo.leftDefault);//左
            //str = str.Replace(inputInfo.rightCurrent, inputInfo.rightDefault);//右
            //str = str.Replace(inputInfo.fireCurrent, inputInfo.fireDefault);//开火
            //str = str.Replace(inputInfo.jumpCurrent, inputInfo.jumpDefault);//跳跃
            //InputActionAsset inputActions = InputActionAsset.FromJson(str);
            //playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            //playerInput.actions = inputActions;
            //playerInput.actions.Enable();//启用设置
        }

        //切换按键
        public void ChangeBtn(BTN_TYPE type)
        {
            nowType = type;
            //得到一次任意键输入
            InputSystem.onAnyButtonPress.CallOnce(ChangeBtnReally);
        }

        //切换真实按键
        private void ChangeBtnReally(InputControl control)
        {
            string[] strs = control.path.Split('/');
            string path = "<" + strs[1] + ">/" + strs[2];
            switch (nowType)
            {
                case BTN_TYPE.UP: inputInfo.upTemp = path; break;
                case BTN_TYPE.DOWN: inputInfo.downTemp = path; break;
                case BTN_TYPE.LEFT: inputInfo.leftTemp = path; break;
                case BTN_TYPE.RIGHT: inputInfo.rightTemp = path; break;
                case BTN_TYPE.FIRE: inputInfo.fireTemp = path; break;
                case BTN_TYPE.JUMP: inputInfo.jumpTemp = path; break;
            }
            Debug.Log($"更换按键: {path}");
            //让玩家产生改建效果
            GetActionAsset();
            //启用设置
            EnableInputConfig();   
            //把临时按键给到当前按键
            switch (nowType)
            {
                case BTN_TYPE.UP: inputInfo.upCurrent = inputInfo.upTemp; break;
                case BTN_TYPE.DOWN: inputInfo.downCurrent = inputInfo.downTemp; break;
                case BTN_TYPE.LEFT: inputInfo.leftCurrent = inputInfo.leftTemp; break;
                case BTN_TYPE.RIGHT: inputInfo.rightCurrent = inputInfo.rightTemp; break;
                case BTN_TYPE.FIRE: inputInfo.fireCurrent = inputInfo.fireTemp; break;
                case BTN_TYPE.JUMP: inputInfo.jumpCurrent = inputInfo.jumpTemp; break;
            }
            SaveInputConfig();//测试 可以放在改键页面的保存按钮
        }
    }
}
