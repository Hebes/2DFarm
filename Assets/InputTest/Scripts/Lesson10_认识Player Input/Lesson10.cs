using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson10 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 PlayerInput是什么？
        //PlayerInput是InputSystem提供的
        //专门用于接受玩家输入来处理自定义逻辑的组件

        //主要工作原理
        //1.配置输入文件（InputActions文件）
        //2.通过PlayerInput关联配置文件，它会自动解析该配置文件
        //3.关联对应的响应函数，处理对应逻辑

        //好处：
        //不需要自己进行相关输入的逻辑书写
        //通过配置文件即可配置想要监听的对应行为
        //让我们专注于输入事件触发后的逻辑处理
        #endregion

        #region 知识点二 添加PlayerInput组件
        //选择任意对象（一般为一个玩家对象）
        //为其添加PlayerInput组件
        #endregion

        #region 知识点三 PlayerInput参数相关

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
