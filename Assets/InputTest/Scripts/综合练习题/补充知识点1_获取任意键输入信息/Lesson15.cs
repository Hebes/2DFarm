using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Lesson15 : MonoBehaviour
{
    public InputAction input;
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 回顾学过的获取任意键输入的方法
        //1.键盘任意键按下
        //input.Enable();
        //input.performed += (context) =>
        //{
        //    print("123");
        //    print(context.control.name);
        //    print(context.control.path);
        //};
        ////2.键盘任意键按下字符
        //Keyboard.current.onTextInput += (c) =>
        //{
        //    print(c);
        //};
        #endregion

        #region 知识点二 InputSystem中专门用于任意键按下的方案
        //如果用Call 按键盘会报错 但是也能正常执行
        //用CallOnce 只会执行一次 但是不会报错
        InputSystem.onAnyButtonPress.CallOnce((control) =>
        {
            print(control.path);
            print(control.name);
        });
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
