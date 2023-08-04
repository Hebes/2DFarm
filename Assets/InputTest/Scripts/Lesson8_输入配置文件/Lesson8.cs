using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson8 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 什么是输入配置文件？
        //输入系统中提供了一种输入配置文件
        //你可以理解它是InputAction的集合
        //可以在一个文件中编辑多个InputAction的信息

        //里面记录了想要处理的行为和动作（也就是InputAction的相关信息）
        //我们可以在其中自己定义 InputAction（比如：开火、移动、旋转等）
        //然后为这个InputAction关联对应的输入动作

        //之后将该配置文件和PlayerInput进行关联
        //PlayerInput会自动帮助我们解析该文件
        //当触发这些InputAction输入动作时会以分发事件的形式通知我们执行行为
        #endregion

        #region 知识点二 编辑输入配置文件
        //1.在Project窗口右键Create创建InputActions配置文件
        //2.双击创建出的文件
        //3.进行配置
        #endregion

        #region 知识点三 配置窗口参数相关

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
