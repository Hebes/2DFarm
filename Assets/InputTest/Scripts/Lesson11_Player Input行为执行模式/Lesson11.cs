using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson11 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 Send Messages
        //在自定义脚本中
        //申明名为 "On+行为名" 的函数
        //没有参数 或者 参数类型为InputValue
        //将该自定义脚本挂载到PlayerInput依附的对象上
        //当触发对应输入时 会自动调用函数

        //并且还有默认的3个和设备相关的函数可以调用
        //设备注册(当控制器从设备丢失中恢复并再次运行时会触发)：OnDeviceRegained(PlayerInput input)
        //设备丢失（玩家失去了分配给它的设备之一，例如，当无线设备耗尽电池时）：OnDeviceLost(PlayerInput input)
        //控制器切换：OnControlsChanged(PlayerInput input)
        #endregion

        #region 知识点二 Broadcast Messages	
        //基本和SendMessage规则一致
        //唯一的区别是，自定义脚本不仅可以挂载在PlayerInput依附的对象上
        //还可以挂载在其子对象下
        #endregion

        #region 知识点三 Invoke Unity Events
        //该模式可以让我们在Inspector窗口上通过拖拽的形式关联响应函数
        //但是注意：响应函数的参数类型 需要改为 InputAction.CallbackContext
        #endregion

        #region 知识点四 Invoke C Sharp Events
        //1.获取PlayerInput组件
        PlayerInput input = this.GetComponent<PlayerInput>();
        //2.获取对应事件进行委托函数添加
        input.onDeviceLost += OnDeviceLost;
        input.onDeviceRegained += OnDeviceRegained;
        input.onControlsChanged += OnControlsChanged;
        input.onActionTriggered += OnActionTrigger;

        //input.currentActionMap["Move"].ReadValue<Vector2>()
        //3.当触发输入时会自动触发事件调用对应函数
        #endregion

        #region 知识点五 关键参数InputValue和InputAction.CallbackContext
        //InputValue
        //是否按下

        //得到具体返回值
        //value.Get<>
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Send Message / Broadcast Messages	
    public void OnMove(InputValue value)
    {
        print("Move");
        print(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        print("Look");
        print(value.Get<Vector2>());
    }
    public void OnFire(InputValue value)
    {
        print("Fire");
        if(value.isPressed)
        {
            print("按下");
        }
    }

    public void OnDeviceLost( PlayerInput input)
    {
        print("设备丢失");
    }

    public void OnDeviceRegained(PlayerInput input)
    {
        print("设备注册");
    }

    public void OnControlsChanged(PlayerInput input)
    {
        print("控制器切换");
    }
    #endregion

    #region Invoke Unity Events
    public void MyFire(InputAction.CallbackContext context)
    {
        print("开火1");
    }

    public void MyMove(InputAction.CallbackContext context)
    {
        print("移动1");
    }

    public void MyLook(InputAction.CallbackContext context)
    {
        print("Look1");
    }

    #endregion

    public void OnActionTrigger(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Fire":
                //输入阶段的判断 触发阶段 才去做逻辑
                if(context.phase == InputActionPhase.Performed)
                    print("开火");
                break;
            case "Look":
                print("看向");
                print(context.ReadValue<Vector2>());
                break;
            case "Move":
                print("移动");
                print(context.ReadValue<Vector2>());
                break;
        }
    }
}
