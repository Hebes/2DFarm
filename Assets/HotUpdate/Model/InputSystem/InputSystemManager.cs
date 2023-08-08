using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    输入系统模块

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

    public class InputSystemManager : SingletonInit<InputSystemManager>, ICore
    {
        private ConfigInputInfo inputInfo;//按键信息
        private string jsonStr;//Json配置信息
        private BTN_TYPE nowType;//记录当前改哪一个键
        public PlayerInput playerInput;

        //核心初始化
        public void ICroeInit()
        {
            inputInfo = new ConfigInputInfo();
            //jsonStr = Resources.Load<TextAsset>("Lesson17").text;//加载配置信息
        }
        //初始化玩家输入组件
        public void InitPlayerInput(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
        }
        //获取输入的动作资产
        public InputActionAsset GetActionAsset()
        {
            //替换按键
            string str = jsonStr.Replace("<up>", inputInfo.up);//上键
            str = str.Replace("<down>", inputInfo.down);//下
            str = str.Replace("<left>", inputInfo.left);//左
            str = str.Replace("<right>", inputInfo.right);//右
            str = str.Replace("<fire>", inputInfo.fire);//开火
            str = str.Replace("<jump>", inputInfo.jump);//跳跃
            return InputActionAsset.FromJson(str);
        }
        //更换按键
        private void ChangeBtn(BTN_TYPE type)
        {
            nowType = type;
            //得到一次任意键输入
            InputSystem.onAnyButtonPress.CallOnce(ChangeBtnReally);
        }
        //让玩家产生改建效果
        public void ChangeInput()
        {
            //改建就是改变 我们PlayerInput上关联的输入配置信息嘛
            playerInput.actions = GetActionAsset();
            playerInput.actions.Enable();
        }
        //切换真实的按键
        private void ChangeBtnReally(InputControl control)
        {
            ACDebug.Log(control.path);
            string[] strs = control.path.Split('/');
            string path = "<" + strs[1] + ">/" + strs[2];
            switch (nowType)
            {
                case BTN_TYPE.UP:
                    inputInfo.up = path;
                    break;
                case BTN_TYPE.DOWN:
                    inputInfo.down = path;
                    break;
                case BTN_TYPE.LEFT:
                    inputInfo.left = path;
                    break;
                case BTN_TYPE.RIGHT:
                    inputInfo.right = path;
                    break;
                case BTN_TYPE.FIRE:
                    inputInfo.fire = path;
                    break;
                case BTN_TYPE.JUMP:
                    inputInfo.jump = path;
                    break;
            }

            //让玩家产生改建效果
            ChangeInput();
        }
    }
}
