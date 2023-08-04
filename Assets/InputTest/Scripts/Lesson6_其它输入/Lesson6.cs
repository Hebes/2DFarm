using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson6 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 新输入系统中的输入设备类
        //常用的
        //Keyboard—键盘
        //Mouse—鼠标
        //Touchscreen—触屏
        //Gamepad—手柄

        //其它
        //Joystick—摇杆
        //Pen—电子笔

        //Sensor（传感器）
        //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.2/manual/Sensors.html#accelerometer
        //Gyroscope—陀螺仪
        //GravitySensor—重力传感器
        //加速传感器
        //光照传感器
        //等等
        #endregion

        #region 知识点二 关于没有细讲的设备
        //对于我们没有细讲的设备
        //平时在开发中不是特别常用
        //如果想要学习他们的使用
        //最直接的方式就是看官方的文档
        //或者直接进到类的内部查看具体成员
        //通过查看变量名和方法名即可了解使用方式

        //UnityEngine.InputSystem.Gyroscope g = UnityEngine.InputSystem.Gyroscope.current;
        //g.angularVelocity.ReadValue()
        #endregion

        #region 知识点三 注意事项
        //新输入系统的设计初衷就是想提升开发者的开发效率
        //我们不提倡写代码来处理输入逻辑
        //之后我们学了配置文件相关知识后
        //都是通过配置文件来设置监听（监视窃听）的输入事件类型
        //我们只需要把工作重心放在输入触发后的逻辑处理

        //所以我们目前讲解的知识都是为了之后最准备
        //实际开发中使用较少
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
