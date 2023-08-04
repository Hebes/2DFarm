using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 获取当前键盘设备(需要引用命名空间)
        //新输入系统 提供了对应的输入设备类 帮助我们对某一种设备输入进行检测
        Keyboard keyBoard = Keyboard.current;
        #endregion

        #region 知识点二 单个按键 按下抬起长按
        //首先要得到某一个按键 通过键盘类对象 点出 各种按键 来获取
        //keyBoard.aKey
        //按下
        if(keyBoard.enterKey.wasPressedThisFrame)
        {

        }
        //抬起
        if(keyBoard.dKey.wasReleasedThisFrame)
        {

        }

        //长按
        if(keyBoard.spaceKey.isPressed)
        {

        }

        #endregion

        #region 知识点三 通过事件监听按键按下
        //通过给keyboard对象中的 文本输入事件 添加委托函数
        //便可以获得每次输入的内容
        keyBoard.onTextInput += (c) =>
        {
            print("通过lambda表达式" + c);
        };
        keyBoard.onTextInput += TextInput;
        keyBoard.onTextInput -= TextInput;
        #endregion

        #region 知识点四 任意键按下监听
        //可以处理 任意键 按下 抬起 长按 相关的逻辑
        //keyBoard.anyKey
        //wasPressedThisFrame
        //wasReleasedThisFrame
        //isPressed
        #endregion
    }

    private void TextInput(char c)
    {
        print("通过函数进行事件监听" + c);
    }

    // Update is called once per frame
    void Update()
    {
        //空格键 当前帧 是否按下
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            print("空格键按下");
        }

        //判断D键是否释放
        if (Keyboard.current.dKey.wasReleasedThisFrame)
        {
            print("D键抬起");
        }

        //判断空格是否一直处于按下状态
        if (Keyboard.current.spaceKey.isPressed)
        {
            print("空格按下状态");
        }


        //任意键按下了
        if(Keyboard.current.anyKey.wasPressedThisFrame)
        {
            print("任意键按下");
        }
    }
}
