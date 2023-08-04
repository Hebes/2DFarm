using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 InputSystem包导入
        //packageManager中导入Input System
        #endregion

        #region 知识点二 选择开启哪一种输入模式
        //选择InputSystem和 老InputManager的启用情况
        //  File——>Build Setting——>Player Setting——>Other——>Active Input Handling
        //  可以同时启用也可以只启用其中之一,每次启用后会重启Unity
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            print("123");
        }
    }
}
