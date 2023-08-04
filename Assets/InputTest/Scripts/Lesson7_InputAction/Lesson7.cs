using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson7 : MonoBehaviour
{
    [Header("Binding")]
    public InputAction move;
    [Header("1D Axis")]
    public InputAction axis;
    [Header("2D Vector")]
    public InputAction vector2D;
    [Header("3D Vector")]
    public InputAction vector3D;

    [Header("Button With One")]
    public InputAction btnOne;


    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 InputAction是什么？
        //顾名思义，InputAction是InputSystem帮助我们封装的输入动作类
        //它的主要作用，是不需要我们通过写代码的形式来处理输入
        //而是直接在Inspector窗口编辑想要处理的输入类型
        //当输入触发时，我们只需要把精力花在输入触发后的逻辑处理上

        //我们在想要用于处理输入动作的类中 
        //申明对应的InputAction类型的成员变量（注意：需要引用命名空间UnityEngine.InputSystem）
        #endregion

        #region 知识点二 InputAction参数相关

        #endregion

        #region 知识点三 InputAction的使用
        //1.启用输入检测
        move.Enable();

        //2.操作监听相关
        //开始操作
        move.started += TestFun;

        //真正触发
        move.performed += (context) =>
        {
            print("触发事件调用");
            //当前状态
            //没有启用 Disabled
            //等待 Waiting
            //开始 Started
            //触发 Performed
            //结束 Canceled
            //context.phase
            print(context.phase);

            //动作行为信息 
            print(context.action.name);

            //控件(设备)信息
            print(context.control.name);

            //获取值
            //context.ReadValue<float>

            //持续时间
            print(context.duration);

            //开始时间
            print(context.startTime);
        };

        //结束操作
        move.canceled += (context) =>
        {
            print("结束事件调用");
        };

        //3.关键参数 CallbackContext
        //当前状态

        //动作行为信息 

        //控件信息

        //获取值

        //持续时间

        //开始时间


        axis.Enable();
        vector2D.Enable();
        vector3D.Enable();

        btnOne.Enable();
        btnOne.performed += (context) =>
        {
            print("组合键触发");
        };

        #endregion

        #region 知识点四 特殊输入设置
        //1.Input System 基础设置（一些默认值设置）
        //2.设置特殊输入规则
        #endregion
    }

    private void TestFun(InputAction.CallbackContext context)
    {
        print("开始事件调用");
    }

    // Update is called once per frame
    void Update()
    {
        //print(axis.ReadValue<float>());

        //rint(vector2D.ReadValue<Vector2>());

        print(vector3D.ReadValue<Vector3>());
    }
}
