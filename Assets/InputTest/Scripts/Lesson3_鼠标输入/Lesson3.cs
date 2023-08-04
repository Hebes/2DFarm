using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 获取当前鼠标设备（需要引用命名空间）
        Mouse mouse = Mouse.current;
        #endregion

        #region 知识点二 鼠标各键位 按下 抬起 长按
        //鼠标左键
        //mouse.leftButton
        //鼠标右键
        //mouse.rightButton
        //鼠标中键
        //mouse.middleButton
        //鼠标 向前向后键
        //mouse.forwardButton;
        //mouse.backButton;

        //按下
        if(mouse.leftButton.wasPressedThisFrame)
        {

        }
        //抬起
        if(mouse.leftButton.wasReleasedThisFrame)
        {

        }
        //长按
        if(mouse.rightButton.isPressed)
        {

        }

        #endregion

        #region 知识点三 鼠标位置相关
        //获取当前鼠标位置
        mouse.position.ReadValue();
        //得到鼠标两帧之间的一个偏移向量 
        mouse.delta.ReadValue();

        //鼠标中间 滚轮的方向向量
        mouse.scroll.ReadValue();
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            print("鼠标左键按下");
        }

        //抬起
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            print("鼠标左键抬起");
        }
        //长按
        if (Mouse.current.rightButton.isPressed)
        {
            print("鼠标右键长按");
        }

        //print(Mouse.current.position.ReadValue());

        //print(Mouse.current.delta.ReadValue());

        print(Mouse.current.scroll.ReadValue());
    }
}
